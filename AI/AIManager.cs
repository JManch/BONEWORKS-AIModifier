using System.Collections;
using System.Collections.Generic;
using ModThatIsNotMod;
using StressLevelZero.AI;
using StressLevelZero.Zones;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Pool;
using UnityEngine;
using MelonLoader;
using AIModifier.UI;
using System.Linq;

namespace AIModifier.AI
{
    public static class AIManager
    {
        private static Dictionary<string, AIBrain> selectedAI = new Dictionary<string, AIBrain>();
        private static Dictionary<string, AIBrain> selectedTargetAI = new Dictionary<string, AIBrain>();

        public static List<AIBrain> selectedAIList { get { return selectedAI.Values.ToList(); } }
        public static List<AIBrain> selectedTargetAIList { get { return selectedTargetAI.Values.ToList(); } }

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

            AIMenuManager.aiMenu.GetPage("ControlAIPage").GetElement("ControlAIButton").SetValue("Control " + 0 + " Selected AI");
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
        public static bool SelectedAIContains(string aiBrainName)
        {
            return selectedAI.ContainsKey(aiBrainName);
        }
        public static bool SelectedTargetAIContrains(string aiBrainName)
        {
            return selectedTargetAI.ContainsKey(aiBrainName);
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
#if DEBUG
                    MelonLogger.Msg("Detected last spawned AI brain as " + spawnedAIBrain.gameObject.name);
#endif
                    ConfigureNewAI(spawnedAIBrain);
                }
            }
        }
        #endregion

        #region Zone AI
        public static void OnZoneSpawnerSpawn(ZoneSpawner __instance)
        {
#if DEBUG
            MelonLogger.Msg("Zonespawner " + __instance.gameObject.name + " spawned an object");
#endif
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
            if (aiBrain == null || SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name) == "enemy_genericTurret_V4")
            {
                return;
            }

#if DEBUG
            
            
            
            
            
            ("Configuring AI " + aiBrain.name);
#endif
            // Add an AIDataComponent if the AI does not have one. This will store the AI's default settings which it will fall back on.
            // If the AI already has an AIDataComponent, restore the AI's settings to default. Default settings are stored in the AIDataComponent component.
            if (aiBrain.gameObject.GetComponent<AIDataComponent>() == null)
            {
                AIDataComponent aiDataComponent = aiBrain.gameObject.AddComponent<AIDataComponent>();

                aiDataComponent.GenerateDefaultAIData();
            }
            else
            {
                // Only applies default values for variables that are not reset by the AI's base config system
                AIDataManager.ApplyNonBaseConfigDefaultAIData(aiBrain);
            }

            // Retrieve stored ai data for the ai
            AIData aiData = AIDataManager.aiData[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];

            // Apply the AI data compared to the default configuration for that AI
            AIDataManager.ApplyAIData(aiBrain, aiData, AIDataManager.defaultAIConfigurations[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)]);

            // Only add a health plate if it doesnt have one as it seems like zone spawners reuse gameobjects?
            if(Utilities.UserPreferences.healthBars.Value)
            {
                if (aiBrain.transform.FindChild("HealthPlate(Clone)") == null)
                {
                    var healthPlate = aiBrain.gameObject.AddComponent<AIHealthPlateController>();
                    healthPlate.OnSpawn();
                }
                else
                {
                    var healthPlate = aiBrain.gameObject.GetComponent<AIHealthPlateController>();
                    healthPlate.OnSpawn();
                }
            }
            else if (aiBrain.gameObject.GetComponent<AIHealthPlateController>() != null)
            {
                GameObject.Destroy(aiBrain.gameObject.GetComponent<AIHealthPlateController>());
            }

            if (aiBrain.transform.FindChild("SelectedPlate(Clone)") == null)
            {
                aiBrain.gameObject.AddComponent<AISelectedPlateController>();
            }
        }

        public static void ConfigureNewAI(AIBrain aiBrain, AIData aiData)
        {
            if (aiBrain == null || SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name) == "enemy_genericTurret_V4")
            {
                return;
            }

            if (aiBrain.gameObject.GetComponent<AIDataComponent>() == null)
            {
                AIDataComponent aiDataComponent = aiBrain.gameObject.AddComponent<AIDataComponent>();
                aiDataComponent.GenerateDefaultAIData();
            }
            else
            {
                AIDataManager.ApplyNonBaseConfigDefaultAIData(aiBrain);
            }

            // Apply all the AI data regardless of default config
            AIDataManager.ApplyAIData(aiBrain, aiData);

            // Only add a health plate if it doesnt have one as it seems like zone spawners reuse gameobjects?
            if (Utilities.UserPreferences.healthBars.Value)
            {
                if (aiBrain.transform.FindChild("HealthPlate(Clone)") == null)
                {
                    var healthPlate = aiBrain.gameObject.AddComponent<AIHealthPlateController>();
                    healthPlate.OnSpawn();
                }
                else
                {
                    var healthPlate = aiBrain.gameObject.GetComponent<AIHealthPlateController>();
                    healthPlate.OnSpawn();
                }
            }
            else if (aiBrain.gameObject.GetComponent<AIHealthPlateController>() != null)
            {
                GameObject.Destroy(aiBrain.gameObject.GetComponent<AIHealthPlateController>());
            }

            if (aiBrain.transform.FindChild("SelectedPlate(Clone)") == null)
            {
                aiBrain.gameObject.AddComponent<AISelectedPlateController>();
            }
        }
    }
}
