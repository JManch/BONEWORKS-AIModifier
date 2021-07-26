﻿using System;
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
            AIData aiData = AIDataManager.aiData[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];

            // Set AI properties
            switch (SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name))
            {
                case ("NullBody"):
                case ("BrettEnemy"):
                case ("FordVrJunkie"):
                case ("Ford_EarlyExit"):
                case ("Ford_EarlyExit_headset"):
                case ("NullBodyCorrupted"):
                    ApplyBehaviourPowerLegsSettings(aiBrain, aiData);
                    break;
                case ("Crablet"):
                case ("CrabletPlus"):
                    ApplyBehaviourCrabletSettings(aiBrain, aiData);
                    break;
                default:
                    ApplyBehaviourOmniWheelSettings(aiBrain, aiData);
                    break;
            }

            // Only add a health plate if it doesnt have one as it seems like zone spawners reuse gameobjects?
            if(aiBrain.transform.FindChild("HeadPlate(Clone)") == null)
            {
                var headPlate = aiBrain.gameObject.AddComponent<AIHeadPlateController>();
                headPlate.OnSpawn();
            }
        }

        private static void ApplyBehaviourPowerLegsSettings(AIBrain aiBrain, AIData aiData)
        {
            BehaviourPowerLegs behaviourPowerLegs = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourPowerLegs>();

            behaviourPowerLegs.health.cur_hp = aiData.health / 100;
            behaviourPowerLegs.health.cur_leg_lf = aiData.leftLegHealth / 100;
            behaviourPowerLegs.health.cur_leg_rt = aiData.rightLegHealth / 100;
            behaviourPowerLegs.health.cur_arm_lf = aiData.leftArmHealth / 100;
            behaviourPowerLegs.health.cur_arm_rt = aiData.rightArmHealth / 100;

            behaviourPowerLegs.accuracy = aiData.accuracy;
            behaviourPowerLegs.gunRange = aiData.gunRange;
            behaviourPowerLegs.reloadTime = aiData.reloadTime;
            behaviourPowerLegs.burstSize = (int)aiData.burstSize;
            behaviourPowerLegs.clipSize = (int)aiData.clipSize;

            behaviourPowerLegs.enableThrowAttack = aiData.enableThrowAttack;
            behaviourPowerLegs.throwCooldown = aiData.throwCooldown;
            behaviourPowerLegs.throwMaxRange = aiData.throwMaxRange;
            behaviourPowerLegs.throwMinRange = aiData.throwMinRange;

            behaviourPowerLegs.agroedSpeed = aiData.agroedSpeed;
            behaviourPowerLegs.roamSpeed = aiData.roamSpeed;
            behaviourPowerLegs.engagedSpeed = aiData.engagedSpeed;
            behaviourPowerLegs.roamRange = new Vector2(aiData.roamRange, aiData.roamRange);
            behaviourPowerLegs.roamWanders = aiData.roamWanders;

            if(aiData.defaultMentalState != "Default")
            {
                behaviourPowerLegs.SwitchMentalState((BehaviourBaseNav.MentalState)Enum.Parse(typeof(BehaviourBaseNav.MentalState), aiData.defaultMentalState));
            }
            else
            {
                // Set mental state to default from default data file
            }

            if (aiData.defaultEngagedMode != "Default")
            {
                behaviourPowerLegs.SwitchEngagedState((BehaviourPowerLegs.EngagedMode)Enum.Parse(typeof(BehaviourPowerLegs.EngagedMode), aiData.defaultEngagedMode));
            }
            else
            {
                // Set engaged mode to default from default data file
            }

            // AGRO ON NPC TYPE NEEDS TO BE IMPLEMENTED
            TriggerRefProxy.NpcType npcType;
            if(Enum.TryParse(aiData.agroOnNPCType, out npcType))
            {
                behaviourPowerLegs.agroOnNpcType = npcType;
            }
            else
            {
                behaviourPowerLegs.agroOnNpcType = TriggerRefProxy.NpcType.Voidman;
            }
            behaviourPowerLegs.meleeRange = aiData.meleeRange;

            behaviourPowerLegs.sensors.hearingSensitivity = aiData.hearingSensitivity;
            behaviourPowerLegs.sensors.SetVisionSphere(aiData.visionRadius);
            behaviourPowerLegs.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }

        private static void ApplyBehaviourCrabletSettings(AIBrain aiBrain, AIData aiData)
        {
            BehaviourCrablet behaviourCrablet = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourCrablet>();

            behaviourCrablet.health.cur_hp = aiData.health / 100;
            behaviourCrablet.health.cur_leg_lf = aiData.leftLegHealth / 100;
            behaviourCrablet.health.cur_leg_rt = aiData.rightLegHealth / 100;
            behaviourCrablet.health.cur_arm_lf = aiData.leftArmHealth / 100;
            behaviourCrablet.health.cur_arm_rt = aiData.rightArmHealth / 100;

            behaviourCrablet.accuracy = aiData.accuracy;
            behaviourCrablet.gunRange = aiData.gunRange;
            behaviourCrablet.reloadTime = aiData.reloadTime;
            behaviourCrablet.burstSize = (int)aiData.burstSize;
            behaviourCrablet.clipSize = (int)aiData.clipSize;

            behaviourCrablet.enableThrowAttack = aiData.enableThrowAttack;
            behaviourCrablet.throwCooldown = aiData.throwCooldown;
            behaviourCrablet.throwMaxRange = aiData.throwMaxRange;
            behaviourCrablet.throwMinRange = aiData.throwMinRange;

            behaviourCrablet.agroedSpeed = aiData.agroedSpeed;
            behaviourCrablet.roamSpeed = aiData.roamSpeed;
            behaviourCrablet.roamRange = new Vector2(aiData.roamRange, aiData.roamRange);
            behaviourCrablet.roamWanders = aiData.roamWanders;

            if (aiData.defaultMentalState != "Default")
            {
                behaviourCrablet.SwitchMentalState((BehaviourBaseNav.MentalState)Enum.Parse(typeof(BehaviourBaseNav.MentalState), aiData.defaultMentalState));
            }

            if(aiData.baseColor != "Default")
            {
                if (ColorUtility.TryParseHtmlString(aiData.baseColor, out Color color))
                {
                    behaviourCrablet.baseColor = color;
                }
            }
            if (aiData.agroColor != "Default")
            {
                if (ColorUtility.TryParseHtmlString(aiData.agroColor, out Color color))
                {
                    behaviourCrablet.agroColor = color;
                }
            }
            behaviourCrablet.enableJumpAttack = aiData.jumpAttackEnabled;
            behaviourCrablet.jumpCooldown = aiData.jumpCooldown;

            TriggerRefProxy.NpcType npcType;
            if (Enum.TryParse(aiData.agroOnNPCType, out npcType))
            {
                behaviourCrablet.agroOnNpcType = npcType;
            }
            else
            {
                behaviourCrablet.agroOnNpcType = TriggerRefProxy.NpcType.Voidman;
            }

            behaviourCrablet.sensors.hearingSensitivity = aiData.hearingSensitivity;
            behaviourCrablet.sensors.SetVisionSphere(aiData.visionRadius);
            behaviourCrablet.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }

        private static void ApplyBehaviourOmniWheelSettings(AIBrain aiBrain, AIData aiData)
        {
            BehaviourOmniwheel behaviourOmniwheel = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourOmniwheel>();

            behaviourOmniwheel.health.cur_hp = aiData.health / 100;
            behaviourOmniwheel.health.cur_leg_lf = aiData.leftLegHealth / 100;
            behaviourOmniwheel.health.cur_leg_rt = aiData.rightLegHealth / 100;
            behaviourOmniwheel.health.cur_arm_lf = aiData.leftArmHealth / 100;
            behaviourOmniwheel.health.cur_arm_rt = aiData.rightArmHealth / 100;

            behaviourOmniwheel.accuracy = aiData.accuracy;
            behaviourOmniwheel.gunRange = aiData.gunRange;
            behaviourOmniwheel.reloadTime = aiData.reloadTime;
            behaviourOmniwheel.burstSize = (int)aiData.burstSize;
            behaviourOmniwheel.clipSize = (int)aiData.clipSize;

            behaviourOmniwheel.enableThrowAttack = aiData.enableThrowAttack;
            behaviourOmniwheel.throwCooldown = aiData.throwCooldown;
            behaviourOmniwheel.throwMaxRange = aiData.throwMaxRange;
            behaviourOmniwheel.throwMinRange = aiData.throwMinRange;

            behaviourOmniwheel.agroedSpeed = aiData.agroedSpeed;
            behaviourOmniwheel.roamSpeed = aiData.roamSpeed;
            behaviourOmniwheel.roamRange = new Vector2(aiData.roamRange, aiData.roamRange);
            behaviourOmniwheel.roamWanders = aiData.roamWanders;

            if (aiData.defaultMentalState != "Default")
            {
                behaviourOmniwheel.SwitchMentalState((BehaviourBaseNav.MentalState)Enum.Parse(typeof(BehaviourBaseNav.MentalState), aiData.defaultMentalState));
            }

            TriggerRefProxy.NpcType npcType;
            if (Enum.TryParse(aiData.agroOnNPCType, out npcType))
            {
                behaviourOmniwheel.agroOnNpcType = npcType;
            }
            else
            {
                behaviourOmniwheel.agroOnNpcType = TriggerRefProxy.NpcType.Voidman;
            }

            behaviourOmniwheel.meleeRange = aiData.meleeRange;

            behaviourOmniwheel.chargeAttackSpeed = aiData.chargeAttackSpeed;
            behaviourOmniwheel.chargeCooldown = aiData.chargeCooldown;
            behaviourOmniwheel.chargePrepSpeed = aiData.chargePrepSpeed;
            behaviourOmniwheel.chargeWindupDistance = aiData.chargeWindupDistance;
            if (aiData.defaultOmniEngagedMode != "Default")
            {
                behaviourOmniwheel.engagedMode = (BehaviourOmniwheel.EngagedMode)Enum.Parse(typeof(BehaviourOmniwheel.EngagedMode), aiData.defaultOmniEngagedMode);
            }

            behaviourOmniwheel.sensors.hearingSensitivity = aiData.hearingSensitivity;
            behaviourOmniwheel.sensors.SetVisionSphere(aiData.visionRadius);
            behaviourOmniwheel.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }
    }
}
