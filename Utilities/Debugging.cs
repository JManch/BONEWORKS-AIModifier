using UnityEngine;
using StressLevelZero.Zones;
using StressLevelZero.AI;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Interaction;
using PuppetMasta;
using MelonLoader;
using HarmonyLib;
using ModThatIsNotMod;
using UnhollowerRuntimeLib;
using AIModifier.UI;
using System.Reflection;
using System.IO;
using System;
using System.Collections.Generic;
using AIModifier.AI;
using System.Xml.Serialization;

namespace AIModifier.Utilities
{
    class Debugging
    {
        public static void DebugLocalAIBrains()
        {
            AIBrain[] aiBrains = GameObject.FindObjectsOfType<AIBrain>();
            foreach (AIBrain aiBrain in aiBrains)
            {
                MelonLogger.Msg(aiBrain.gameObject.name);
                MelonLogger.Msg("cur_hp is " + aiBrain.behaviour.health.cur_hp);
                MelonLogger.Msg("maxAppendageHp is " + aiBrain.behaviour.health.maxAppendageHp);
                MelonLogger.Msg("maxHitPoints is " + aiBrain.behaviour.health.maxHitPoints);
                MelonLogger.Msg("cur_arm_lf is " + aiBrain.behaviour.health.cur_arm_lf);
                MelonLogger.Msg("cur_arm_rt is " + aiBrain.behaviour.health.cur_arm_rt);
                MelonLogger.Msg("cur_leg_lf is " + aiBrain.behaviour.health.cur_leg_lf);
                MelonLogger.Msg("cur_leg_rt is " + aiBrain.behaviour.health.cur_leg_rt);
            }
        }

        public static void DebugAIData()
        {
            AIBrain[] aiBrains = GameObject.FindObjectsOfType<AIBrain>();
            BehaviourCrablet[] behaviourCrablets = GameObject.FindObjectsOfType<BehaviourCrablet>();

            MelonLogger.Msg(behaviourCrablets.Length);

            List<AIData> aiDatas = new List<AIData>();

            foreach (AIBrain aiBrain in aiBrains)
            {
                switch (SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name))
                {
                    case ("NullBody"):
                    case ("BrettEnemy"):
                    case ("FordVrJunkie"):
                    case ("Ford_EarlyExit"):
                    case ("Ford_EarlyExit_headset"):
                    case ("NullBodyCorrupted"):
                        aiDatas.Add(BehaviourPowerLegsAIData(aiBrain));
                        break;
                    case ("Crablet"):
                    case ("CrabletPlus"):
                        aiDatas.Add(BehaviourCrabletAIData(aiBrain));
                        break;
                    default:
                        aiDatas.Add(BehaviourOmniWheelAIData(aiBrain));
                        break;
                }
            }

            XMLDataManager.SaveXMLData(aiDatas, @"\Mods\AIDataDebug.xml");
        }

        private static AIData BehaviourPowerLegsAIData(AIBrain aiBrain)
        {
            BehaviourPowerLegs behaviourPowerLegs = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourPowerLegs>();

            AIData aiData = new AIData();

            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourPowerLegs.health.cur_hp;
            aiData.leftLegHealth = behaviourPowerLegs.health.cur_leg_lf;
            aiData.rightLegHealth = behaviourPowerLegs.health.cur_leg_rt;
            aiData.leftArmHealth = behaviourPowerLegs.health.cur_arm_lf;
            aiData.rightArmHealth = behaviourPowerLegs.health.cur_arm_rt;

            aiData.accuracy = behaviourPowerLegs.accuracy;
            aiData.gunRange = behaviourPowerLegs.gunRange;
            aiData.reloadTime = behaviourPowerLegs.reloadTime;
            aiData.burstSize = behaviourPowerLegs.burstSize;
            aiData.clipSize = behaviourPowerLegs.clipSize;

            aiData.enableThrowAttack = behaviourPowerLegs.enableThrowAttack;
            aiData.throwCooldown = behaviourPowerLegs.throwCooldown;
            aiData.throwMaxRange = behaviourPowerLegs.throwMaxRange;
            aiData.throwMinRange = behaviourPowerLegs.throwMinRange;

            aiData.agroedSpeed = behaviourPowerLegs.agroedSpeed;
            aiData.roamSpeed = behaviourPowerLegs.roamSpeed;
            aiData.engagedSpeed = behaviourPowerLegs.engagedSpeed;
            aiData.roamRange = behaviourPowerLegs.roamRange.x;
            aiData.roamWanders = behaviourPowerLegs.roamWanders;

            aiData.defaultMentalState = behaviourPowerLegs.mentalState.ToString();
            aiData.defaultEngagedMode = behaviourPowerLegs.mentalState.ToString();

            aiData.agroOnNPCType = behaviourPowerLegs.agroOnNpcType.ToString();
            aiData.meleeRange = behaviourPowerLegs.meleeRange;

            aiData.hearingSensitivity = behaviourPowerLegs.sensors.hearingSensitivity;
            aiData.visionRadius = behaviourPowerLegs.sensors._visionSphere.radius;
            aiData.pitchMultiplier = behaviourPowerLegs.sfx.pitchMultiplier;

            return aiData;
        }

        private static AIData BehaviourCrabletAIData(AIBrain aiBrain)
        {
            BehaviourCrablet behaviourCrablet = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourCrablet>();

            AIData aiData = new AIData();
            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourCrablet.health.cur_hp;
            aiData.leftLegHealth = behaviourCrablet.health.cur_leg_lf;
            aiData.rightLegHealth = behaviourCrablet.health.cur_leg_rt;
            aiData.leftArmHealth = behaviourCrablet.health.cur_arm_lf;
            aiData.rightArmHealth = behaviourCrablet.health.cur_arm_rt;

            aiData.accuracy = behaviourCrablet.accuracy;
            aiData.gunRange = behaviourCrablet.gunRange;
            aiData.reloadTime = behaviourCrablet.reloadTime;
            aiData.burstSize = behaviourCrablet.burstSize;
            aiData.clipSize = behaviourCrablet.clipSize;

            aiData.enableThrowAttack = behaviourCrablet.enableThrowAttack;
            aiData.throwCooldown = behaviourCrablet.throwCooldown;
            aiData.throwMaxRange = behaviourCrablet.throwMaxRange;
            aiData.throwMinRange = behaviourCrablet.throwMinRange;

            aiData.agroedSpeed = behaviourCrablet.agroedSpeed;
            aiData.roamSpeed = behaviourCrablet.roamSpeed;
            //aiData.engagedSpeed = behaviourCrablet.engagedSpeed;
            aiData.roamRange = behaviourCrablet.roamRange.x;
            aiData.roamWanders = behaviourCrablet.roamWanders;

            aiData.defaultMentalState = behaviourCrablet.mentalState.ToString();
            aiData.defaultEngagedMode = behaviourCrablet.mentalState.ToString();

            aiData.baseColor = behaviourCrablet.baseColor.ToString();
            aiData.agroColor = behaviourCrablet.agroColor.ToString();
            aiData.jumpAttackEnabled = behaviourCrablet.enableJumpAttack;
            aiData.jumpCooldown = behaviourCrablet.jumpCooldown;

            aiData.agroOnNPCType = behaviourCrablet.agroOnNpcType.ToString();
            //aiData.meleeRange = behaviourCrablet.meleeRange;

            aiData.hearingSensitivity = behaviourCrablet.sensors.hearingSensitivity;
            aiData.visionRadius = behaviourCrablet.sensors._visionSphere.radius;
            aiData.pitchMultiplier = behaviourCrablet.sfx.pitchMultiplier;

            return aiData;
        }

        private static AIData BehaviourOmniWheelAIData(AIBrain aiBrain)
        {
            BehaviourOmniwheel behaviourOmniwheel = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourOmniwheel>();

            AIData aiData = new AIData();
            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourOmniwheel.health.cur_hp;
            aiData.leftLegHealth = behaviourOmniwheel.health.cur_leg_lf;
            aiData.rightLegHealth = behaviourOmniwheel.health.cur_leg_rt;
            aiData.leftArmHealth = behaviourOmniwheel.health.cur_arm_lf;
            aiData.rightArmHealth = behaviourOmniwheel.health.cur_arm_rt;

            aiData.accuracy = behaviourOmniwheel.accuracy;
            aiData.gunRange = behaviourOmniwheel.gunRange;
            aiData.reloadTime = behaviourOmniwheel.reloadTime;
            aiData.burstSize = behaviourOmniwheel.burstSize;
            aiData.clipSize = behaviourOmniwheel.clipSize;

            aiData.enableThrowAttack = behaviourOmniwheel.enableThrowAttack;
            aiData.throwCooldown = behaviourOmniwheel.throwCooldown;
            aiData.throwMaxRange = behaviourOmniwheel.throwMaxRange;
            aiData.throwMinRange = behaviourOmniwheel.throwMinRange;

            aiData.agroedSpeed = behaviourOmniwheel.agroedSpeed;
            aiData.roamSpeed = behaviourOmniwheel.roamSpeed;
            //aiData.engagedSpeed = behaviourOmniwheel.engagedSpeed;
            aiData.roamRange = behaviourOmniwheel.roamRange.x;
            aiData.roamWanders = behaviourOmniwheel.roamWanders;

            aiData.defaultMentalState = behaviourOmniwheel.mentalState.ToString();
            aiData.defaultEngagedMode = behaviourOmniwheel.mentalState.ToString();

            //aiData.baseColor = behaviourOmniwheel.baseColor.ToString();
            //aiData.agroColor = behaviourOmniwheel.agroColor.ToString();
            //aiData.jumpAttackEnabled = behaviourOmniwheel.enableJumpAttack;
            //aiData.jumpCooldown = behaviourOmniwheel.jumpCooldown;

            aiData.agroOnNPCType = behaviourOmniwheel.agroOnNpcType.ToString();
            aiData.meleeRange = behaviourOmniwheel.meleeRange;

            aiData.chargeAttackSpeed = behaviourOmniwheel.chargeAttackSpeed;
            aiData.chargeCooldown = behaviourOmniwheel.chargeCooldown;
            aiData.chargePrepSpeed = behaviourOmniwheel.chargePrepSpeed;
            aiData.chargeWindupDistance = behaviourOmniwheel.chargeWindupDistance;
            aiData.defaultOmniEngagedMode = behaviourOmniwheel.engagedMode.ToString();

            aiData.hearingSensitivity = behaviourOmniwheel.sensors.hearingSensitivity;
            aiData.visionRadius = behaviourOmniwheel.sensors._visionSphere.radius;
            aiData.pitchMultiplier = behaviourOmniwheel.sfx.pitchMultiplier;

            return aiData;
        }
    }
}
