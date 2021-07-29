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
using System.Linq;

namespace AIModifier.AI
{
    public static class AIManager
    {
        private static Dictionary<string, AIBrain> selectedAI = new Dictionary<string, AIBrain>();
        private static Dictionary<string, AIBrain> selectedTargetAI = new Dictionary<string, AIBrain>();

        #region Selected AI stuff
        public static void ClearSelectedAI()
        {
            foreach(AIBrain aiBrain in selectedAI.Values.ToList())
            {
                if(aiBrain != null)
                {
                    aiBrain.GetComponent<AISelectedPlateController>().DisableSelectedIcon();
                }
            }
            selectedAI.Clear();
        }
        public static void ClearSelectedTargetAI()
        {
            foreach (AIBrain aiBrain in selectedTargetAI.Values.ToList())
            {
                if (aiBrain != null)
                {
                    aiBrain.GetComponent<AISelectedPlateController>().DisableSelectedIcon();
                }
            }
            selectedTargetAI.Clear();
        }
        public static void ToggleSelectedAI(AIBrain aiBrain)
        {
            if(!selectedAI.ContainsKey(aiBrain.name))
            {
                selectedAI.Add(aiBrain.name, aiBrain);
                aiBrain.GetComponent<AISelectedPlateController>().EnableSelectedIcon(AISelectedPlateController.SelectedType.Standard);
            }
            else
            {
                selectedAI.Remove(aiBrain.name);
                aiBrain.GetComponent<AISelectedPlateController>().DisableSelectedIcon();
            }

            // Update UI
            AIMenuManager.aiMenu.GetPage("ControlAIPage").GetElement("ControlAIButton").SetValue("Control " + selectedAI.Count + " Selected AI");
        }
        public static void ToggleSelectedTargetAI(AIBrain aiBrain)
        {
            if (!selectedTargetAI.ContainsKey(aiBrain.name))
            {
                selectedTargetAI.Add(aiBrain.name, aiBrain);
                aiBrain.GetComponent<AISelectedPlateController>().EnableSelectedIcon(AISelectedPlateController.SelectedType.Target);
            }
            else
            {
                selectedTargetAI.Remove(aiBrain.name);
                aiBrain.GetComponent<AISelectedPlateController>().DisableSelectedIcon();
            }
        }
        public static List<AIBrain> GetSelectedAI()
        {
            return selectedAI.Values.ToList();
        }
        public static List<AIBrain> GetSelectedTargetAI()
        {
            return selectedTargetAI.Values.ToList();
        }

        #endregion

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
            if(aiBrain.transform.FindChild("HealthPlate(Clone)") == null)
            {
                var headPlate = aiBrain.gameObject.AddComponent<AIHealthPlateController>();
                headPlate.OnSpawn();
            }

            if (aiBrain.transform.FindChild("SelectedPlate(Clone)") == null)
            {
                aiBrain.gameObject.AddComponent<AISelectedPlateController>();
            }
        }
    }
}
