using System;
using System.Collections;
using System.Collections.Generic;
using ModThatIsNotMod;
using StressLevelZero.AI;
using StressLevelZero.Zones;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Pool;
using UnityEngine;
using MelonLoader;
using PuppetMasta;
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
            // Add an AIDataComponent if the AI does not have one. This will store the AI's default settings which it will fall back on.
            // If the AI already has an AIDataComponent, restore the AI's settings to default. Default settings are stored in the AIDataComponent component.

            if (aiBrain.gameObject.GetComponent<AIDataComponent>() == null)
            {
                aiBrain.gameObject.AddComponent<AIDataComponent>();
            }
            else
            {
                AIDataManager.ApplyAIData(aiBrain, aiBrain.gameObject.GetComponent<AIDataComponent>().defaultAIData);
            }

            // Retrieve stored ai data for the ai
            AIData aiData = AIDataManager.aiData[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];

            // Apply the AI data compared to the default configuration for that AI
            AIDataManager.ApplyAIData(aiBrain, aiData, AIDataManager.defaultAIConfigurations[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)]);

            // Only add a health plate if it doesnt have one as it seems like zone spawners reuse gameobjects?
            if(aiBrain.transform.FindChild("HeadPlate(Clone)") == null)
            {
                var headPlate = aiBrain.gameObject.AddComponent<AIHeadPlateController>();
                headPlate.OnSpawn();
            }
        }
    }
}
