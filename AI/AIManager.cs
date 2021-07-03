using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ModThatIsNotMod;
using StressLevelZero.AI;
using StressLevelZero.Zones;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Pool;
using StressLevelZero.Combat;
using UnityEngine;
using HarmonyLib;
using MelonLoader;
using PuppetMasta;
using AIModifier.Utilities;
using AIModifier.UI;

namespace AIModifier.AI
{
    public static class AIManager
    {
        public static Dictionary<string, AIBrain> selectedAI = new Dictionary<string, AIBrain>();

        #region Utility gun AI
        public static void OnFireSpawnGun(SpawnGun __instance)
        {
            if(__instance._selectedSpawnable != null && __instance._selectedSpawnable.category == StressLevelZero.Data.CategoryFilters.NPCS)
            {
                MelonCoroutines.Start(CoGetLastSpawnedPoolee(PoolManager.DynamicPools[__instance._selectedSpawnable.title]));
            }
        }

        private static IEnumerator CoGetLastSpawnedPoolee(Pool pool)
        {
            yield return new WaitForSeconds(0.1f);
            if(pool._spawnedObjects.Count > 0)
            {
                Poolee poolee = pool._spawnedObjects[pool._spawnedObjects.Count - 1];
                if(poolee != null)
                {
                    AIBrain spawnedAIBrain = poolee.GetComponent<AIBrain>();
                    MelonLogger.Msg("Detected last spawned AI brain as " + spawnedAIBrain.gameObject.name);
                    ConfigureNewAI(spawnedAIBrain);
                }
            }
        }
        #endregion

        #region Zone AI
        public static void OnZoneSpawnerSpawnPostfix(ZoneSpawner __instance)
        {
            MelonLogger.Msg("Zonespawner " + __instance.gameObject.name + " spawned an object");

            if(__instance.spawns.Count != 0)
            {
                AIBrain spawnedAIBrain = __instance.spawns[__instance.spawns.Count - 1].GetComponent<AIBrain>();
                ConfigureNewAI(spawnedAIBrain);
            }
        }
        #endregion

        // Called once on each AIBrain when it is first spawned in
        private static void ConfigureNewAI(AIBrain aiBrain)
        {
            AIData aiData = AIDataManager.aiDataDictionary[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];
            
            if(aiData == null)
            {
                MelonLogger.Error("Failed to load AI data for AI " + SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name));
            }

            // Set AI properties
            aiBrain.behaviour.health.cur_hp = aiData.health/100;

            // Only add a health plate if it doesnt have one as it seems like zone spawners reuse gameobjects?
            if(aiBrain.transform.FindChild("HeadPlate(Clone)") == null)
            {
                aiBrain.gameObject.AddComponent<AIHeadPlateController>();
            }
        }
    }
}
