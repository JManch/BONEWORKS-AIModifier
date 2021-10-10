using System;
using System.Collections.Generic;
using StressLevelZero.AI;
using AIModifier.Utilities;
using ModThatIsNotMod;
using PuppetMasta;
using UnityEngine;
using System.Linq;
using System.IO;

namespace AIModifier.AI
{
    public static class AIDataManager
    {
        public static Dictionary<string, AIData> aiData = new Dictionary<string, AIData>();
        public static Dictionary<string, AIData> defaultAIConfigurations = new Dictionary<string, AIData>();

        public static void LoadDefaultAIData()
        {
            defaultAIConfigurations.Clear();
            List<AIData> aiDatas = XMLDataManager.LoadDefaultAIData();
            foreach (AIData aiData in aiDatas)
            {
                AIDataManager.defaultAIConfigurations.Add(aiData.name, aiData);
            }
        }

        public static void LoadAIData()
        {
            aiData.Clear();
            List<AIData> aiDatas = XMLDataManager.LoadXMLData<List<AIData>>(@"AISettings.xml");
            foreach (AIData aiData in aiDatas)
            {
                AIDataManager.aiData.Add(aiData.name, aiData);
            }
        }

        // Pass an empty string to write all AI data to disk
        public static void WriteAIDataToDisk(string aiName = "")
        {
            if(aiName == "")
            {
                // Just save. Not sure if this overwrites, test it.
                XMLDataManager.SaveXMLData(aiData.Values.ToList(), @"AISettings.xml");
                
            }
            else
            {
                List<AIData> aiDatas = XMLDataManager.LoadXMLData<List<AIData>>(@"AISettings.xml");
                File.Delete(Utilities.Utilities.aiModifierDirectory + @"AISettings.xml");
                for (int i = 0; i < aiDatas.Count; i++)
                {
                    if (aiDatas[i].name == aiName)
                    {
                        aiDatas[i] = aiData[aiName];
                        break;
                    }
                }
                XMLDataManager.SaveXMLData(aiDatas, @"AISettings.xml");
            }
        }

        public static AIData GenerateAIData(AIBrain aiBrain)
        {
            AIData aiData = new AIData();

            switch (SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name))
            {
                case ("NullBody"):
                case ("BrettEnemy"):
                case ("FordVrJunkie"):
                case ("Ford_EarlyExit"):
                case ("Ford_EarlyExit_headset"):
                case ("NullBodyCorrupted"):
                    GeneratePowerLegData(aiBrain, aiData);
                    break;
                case ("Crablet"):
                case ("CrabletPlus"):
                    GenerateCrabletData(aiBrain, aiData);
                    break;
                default:
                    GenerateOmniWheelData(aiBrain, aiData);
                    break;
            }

            return aiData;
        }

        // Applies AIData settings to an AIBrain. Will only apply data where the newData is different to the comparisonData. If comparison data is null, everything will apply.
        public static void ApplyAIData(AIBrain aiBrain, AIData aiData, AIData comparisonData = null)
        {
            switch (SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name))
            {
                case ("NullBody"):
                case ("BrettEnemy"):
                case ("FordVrJunkie"):
                case ("Ford_EarlyExit"):
                case ("Ford_EarlyExit_headset"):
                case ("NullBodyCorrupted"):
                    ApplyPowerLegData(aiBrain, aiData, comparisonData);
                    break;
                case ("Crablet"):
                case ("CrabletPlus"):
                    ApplyCrabletData(aiBrain, aiData, comparisonData);
                    break;
                default:
                    ApplyOmniWheelData(aiBrain, aiData, comparisonData);
                    break;
            }

            aiBrain.gameObject.GetComponent<AIDataComponent>().UpdateAIData(aiData.createClone());
        }
        public static void ApplyNonBaseConfigDefaultAIData(AIBrain aiBrain)
        {
            AIData aiData = aiBrain.GetComponent<AIDataComponent>().defaultAIData;

            aiBrain.behaviour.health.cur_hp = aiData.health / 100;
            aiBrain.behaviour.health.cur_leg_lf = aiData.leftLegHealth / 100;
            aiBrain.behaviour.health.cur_leg_rt = aiData.rightLegHealth / 100;
            aiBrain.behaviour.health.cur_arm_lf = aiData.leftArmHealth / 100;
            aiBrain.behaviour.health.cur_arm_rt = aiData.rightArmHealth / 100;

            if(aiBrain.behaviour is BehaviourPowerLegs)
            {
                ((BehaviourPowerLegs)aiBrain.behaviour).engagedSpeed = aiData.engagedSpeed;
                ((BehaviourPowerLegs)aiBrain.behaviour).meleeRange = aiData.meleeRange;
            }

            if(aiBrain.behaviour is BehaviourCrablet)
            {
                ((BehaviourCrablet)aiBrain.behaviour).enableJumpAttack = aiData.jumpAttackEnabled;
                ((BehaviourCrablet)aiBrain.behaviour).jumpCooldown = aiData.jumpCooldown;
            }

            if(aiBrain.behaviour is BehaviourOmniwheel)
            {
                ((BehaviourOmniwheel)aiBrain.behaviour).meleeRange = aiData.meleeRange;
                ((BehaviourOmniwheel)aiBrain.behaviour).chargeCooldown = aiData.chargeCooldown;
                ((BehaviourOmniwheel)aiBrain.behaviour).chargePrepSpeed = aiData.chargePrepSpeed;
                ((BehaviourOmniwheel)aiBrain.behaviour).chargeWindupDistance = aiData.chargeWindupDistance;
            }

            if (Enum.TryParse(aiData.agroOnNPCType, out TriggerRefProxy.NpcType npcType))
            {
                aiBrain.behaviour.agroOnNpcType = npcType;
            }
            else
            {
                aiBrain.behaviour.agroOnNpcType = 0;
            }

            aiBrain.behaviour.sensors.hearingSensitivity = aiData.hearingSensitivity;
            aiBrain.behaviour.sensors._visionSphere.radius = aiData.visionRadius;
            aiBrain.behaviour.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }

        private static void GeneratePowerLegData(AIBrain aiBrain, AIData aiData)
        {
            BehaviourPowerLegs behaviourPowerLegs = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourPowerLegs>();

            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourPowerLegs.health.cur_hp * 100;
            aiData.leftLegHealth = behaviourPowerLegs.health.cur_leg_lf * 100;
            aiData.rightLegHealth = behaviourPowerLegs.health.cur_leg_rt * 100;
            aiData.leftArmHealth = behaviourPowerLegs.health.cur_arm_lf * 100;
            aiData.rightArmHealth = behaviourPowerLegs.health.cur_arm_rt * 100;

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

            aiData.defaultMentalState = "Default";
            aiData.defaultEngagedMode = "Default";

            aiData.agroOnNPCType = behaviourPowerLegs.agroOnNpcType.ToString();
            aiData.meleeRange = behaviourPowerLegs.meleeRange;

            aiData.hearingSensitivity = behaviourPowerLegs.sensors.hearingSensitivity;
            aiData.visionRadius = behaviourPowerLegs.sensors._visionSphere.radius;
            aiData.pitchMultiplier = behaviourPowerLegs.sfx.pitchMultiplier;
        }

        private static void GenerateCrabletData(AIBrain aiBrain, AIData aiData)
        {
            BehaviourCrablet behaviourCrablet = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourCrablet>();

            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourCrablet.health.cur_hp * 100;
            aiData.leftLegHealth = behaviourCrablet.health.cur_leg_lf * 100;
            aiData.rightLegHealth = behaviourCrablet.health.cur_leg_rt * 100;
            aiData.leftArmHealth = behaviourCrablet.health.cur_arm_lf * 100;
            aiData.rightArmHealth = behaviourCrablet.health.cur_arm_rt * 100;

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

            aiData.roamRange = behaviourCrablet.roamRange.x;
            aiData.roamWanders = behaviourCrablet.roamWanders;

            aiData.defaultMentalState = "Default";
            aiData.defaultEngagedMode = "Default";

            aiData.baseColor = "Default";
            aiData.agroColor = "Default";
            aiData.jumpAttackEnabled = behaviourCrablet.enableJumpAttack;
            aiData.jumpCooldown = behaviourCrablet.jumpCooldown;

            aiData.agroOnNPCType = behaviourCrablet.agroOnNpcType.ToString();

            aiData.hearingSensitivity = behaviourCrablet.sensors.hearingSensitivity;
            aiData.visionRadius = behaviourCrablet.sensors._visionSphere.radius;
            aiData.pitchMultiplier = behaviourCrablet.sfx.pitchMultiplier;
        }

        private static void GenerateOmniWheelData(AIBrain aiBrain, AIData aiData)
        {
            BehaviourOmniwheel behaviourOmniwheel = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourOmniwheel>();

            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourOmniwheel.health.cur_hp * 100;
            aiData.leftLegHealth = behaviourOmniwheel.health.cur_leg_lf * 100;
            aiData.rightLegHealth = behaviourOmniwheel.health.cur_leg_rt * 100;
            aiData.leftArmHealth = behaviourOmniwheel.health.cur_arm_lf * 100;
            aiData.rightArmHealth = behaviourOmniwheel.health.cur_arm_rt * 100;
 
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
            
            aiData.roamRange = behaviourOmniwheel.roamRange.x;
            aiData.roamWanders = behaviourOmniwheel.roamWanders;
            
            aiData.defaultMentalState = "Default";
            aiData.defaultEngagedMode = "Default";
            
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
        }

        private static void ApplyPowerLegData(AIBrain aiBrain, AIData aiData, AIData comparisonData = null)
        {
            BehaviourPowerLegs behaviourPowerLegs = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourPowerLegs>();

            if (comparisonData == null || (aiData.health != comparisonData.health && behaviourPowerLegs.health.cur_hp != 0)) behaviourPowerLegs.health.cur_hp = (aiData.health / 100);
            if (comparisonData == null || aiData.leftLegHealth != comparisonData.leftLegHealth) behaviourPowerLegs.health.cur_leg_lf = aiData.leftLegHealth / 100;
            if (comparisonData == null || aiData.rightLegHealth != comparisonData.rightLegHealth) behaviourPowerLegs.health.cur_leg_rt = aiData.rightLegHealth / 100;
            if (comparisonData == null || aiData.leftArmHealth != comparisonData.leftArmHealth) behaviourPowerLegs.health.cur_arm_lf = aiData.leftArmHealth / 100;
            if (comparisonData == null || aiData.rightArmHealth != comparisonData.rightArmHealth) behaviourPowerLegs.health.cur_arm_rt = aiData.rightArmHealth / 100;

            if (comparisonData == null || aiData.accuracy != comparisonData.accuracy) behaviourPowerLegs.accuracy = aiData.accuracy;
            if (comparisonData == null || aiData.gunRange != comparisonData.gunRange) behaviourPowerLegs.gunRange = aiData.gunRange;
            if (comparisonData == null || aiData.reloadTime != comparisonData.reloadTime) behaviourPowerLegs.reloadTime = aiData.reloadTime;
            if (comparisonData == null || aiData.burstSize != comparisonData.burstSize) behaviourPowerLegs.burstSize = (int)aiData.burstSize;
            if (comparisonData == null || aiData.clipSize != comparisonData.clipSize) behaviourPowerLegs.clipSize = (int)aiData.clipSize;

            if (comparisonData == null || aiData.enableThrowAttack != comparisonData.enableThrowAttack) behaviourPowerLegs.enableThrowAttack = aiData.enableThrowAttack;
            if (comparisonData == null || aiData.throwCooldown != comparisonData.throwCooldown) behaviourPowerLegs.throwCooldown = aiData.throwCooldown;
            if (comparisonData == null || aiData.throwMaxRange != comparisonData.throwMaxRange) behaviourPowerLegs.throwMaxRange = aiData.throwMaxRange;
            if (comparisonData == null || aiData.throwMinRange != comparisonData.throwMinRange) behaviourPowerLegs.throwMinRange = aiData.throwMinRange;

            if (comparisonData == null || aiData.agroedSpeed != comparisonData.agroedSpeed) behaviourPowerLegs.agroedSpeed = aiData.agroedSpeed;
            if (comparisonData == null || aiData.roamSpeed != comparisonData.roamSpeed) behaviourPowerLegs.roamSpeed = aiData.roamSpeed;
            if (comparisonData == null || aiData.engagedSpeed != comparisonData.engagedSpeed) behaviourPowerLegs.engagedSpeed = aiData.engagedSpeed;
            if (comparisonData == null || aiData.roamRange != comparisonData.roamRange) behaviourPowerLegs.roamRange = new Vector2(aiData.roamRange, aiData.roamRange);
            if (comparisonData == null || aiData.roamWanders != comparisonData.roamWanders) behaviourPowerLegs.roamWanders = aiData.roamWanders;

            if (aiData.defaultMentalState != "Default") behaviourPowerLegs.SwitchMentalState((BehaviourPowerLegs.MentalState)Enum.Parse(typeof(BehaviourPowerLegs.MentalState), aiData.defaultMentalState));

            if (aiData.defaultEngagedMode != "Default") behaviourPowerLegs.SwitchEngagedState((BehaviourPowerLegs.EngagedMode)Enum.Parse(typeof(BehaviourPowerLegs.EngagedMode), aiData.defaultEngagedMode));

            if(comparisonData == null || aiData.agroOnNPCType != comparisonData.agroOnNPCType)
            {
                if (Enum.TryParse(aiData.agroOnNPCType, out TriggerRefProxy.NpcType npcType))
                {
                    behaviourPowerLegs.agroOnNpcType = npcType;
                }
                else
                {
                    behaviourPowerLegs.agroOnNpcType = 0;
                }
            }

            if (comparisonData == null || aiData.meleeRange != comparisonData.meleeRange) behaviourPowerLegs.meleeRange = aiData.meleeRange;

            if (comparisonData == null || aiData.hearingSensitivity != comparisonData.hearingSensitivity) behaviourPowerLegs.sensors.hearingSensitivity = aiData.hearingSensitivity;
            if (comparisonData == null || aiData.visionRadius != comparisonData.visionRadius) behaviourPowerLegs.sensors.SetVisionSphere(aiData.visionRadius);
            if (comparisonData == null || aiData.pitchMultiplier != comparisonData.pitchMultiplier) behaviourPowerLegs.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }

        private static void ApplyCrabletData(AIBrain aiBrain, AIData aiData, AIData comparisonData = null)
        {
            BehaviourCrablet behaviourCrablet = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourCrablet>();

            if (comparisonData == null || aiData.health != comparisonData.health) behaviourCrablet.health.cur_hp = (aiData.health / 100);
            if (comparisonData == null || aiData.leftLegHealth != comparisonData.leftLegHealth) behaviourCrablet.health.cur_leg_lf = aiData.leftLegHealth / 100;
            if (comparisonData == null || aiData.rightLegHealth != comparisonData.rightLegHealth) behaviourCrablet.health.cur_leg_rt = aiData.rightLegHealth / 100;
            if (comparisonData == null || aiData.leftArmHealth != comparisonData.leftArmHealth) behaviourCrablet.health.cur_arm_lf = aiData.leftArmHealth / 100;
            if (comparisonData == null || aiData.rightArmHealth != comparisonData.rightArmHealth) behaviourCrablet.health.cur_arm_rt = aiData.rightArmHealth / 100;

            if (comparisonData == null || aiData.accuracy != comparisonData.accuracy) behaviourCrablet.accuracy = aiData.accuracy;
            if (comparisonData == null || aiData.gunRange != comparisonData.gunRange) behaviourCrablet.gunRange = aiData.gunRange;
            if (comparisonData == null || aiData.reloadTime != comparisonData.reloadTime) behaviourCrablet.reloadTime = aiData.reloadTime;
            if (comparisonData == null || aiData.burstSize != comparisonData.burstSize) behaviourCrablet.burstSize = (int)aiData.burstSize;
            if (comparisonData == null || aiData.clipSize != comparisonData.clipSize) behaviourCrablet.clipSize = (int)aiData.clipSize;

            if (comparisonData == null || aiData.enableThrowAttack != comparisonData.enableThrowAttack) behaviourCrablet.enableThrowAttack = aiData.enableThrowAttack;
            if (comparisonData == null || aiData.throwCooldown != comparisonData.throwCooldown) behaviourCrablet.throwCooldown = aiData.throwCooldown;
            if (comparisonData == null || aiData.throwMaxRange != comparisonData.throwMaxRange) behaviourCrablet.throwMaxRange = aiData.throwMaxRange;
            if (comparisonData == null || aiData.throwMinRange != comparisonData.throwMinRange) behaviourCrablet.throwMinRange = aiData.throwMinRange;

            if (comparisonData == null || aiData.agroedSpeed != comparisonData.agroedSpeed) behaviourCrablet.agroedSpeed = aiData.agroedSpeed;
            if (comparisonData == null || aiData.roamSpeed != comparisonData.roamSpeed) behaviourCrablet.roamSpeed = aiData.roamSpeed;
            if (comparisonData == null || aiData.roamRange != comparisonData.roamRange) behaviourCrablet.roamRange = new Vector2(aiData.roamRange, aiData.roamRange);
            if (comparisonData == null || aiData.roamWanders != comparisonData.roamWanders) behaviourCrablet.roamWanders = aiData.roamWanders;

            if (aiData.defaultMentalState != "Default") behaviourCrablet.SwitchMentalState((BehaviourBaseNav.MentalState)Enum.Parse(typeof(BehaviourBaseNav.MentalState), aiData.defaultMentalState));

            if (aiData.baseColor != "Default")
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

            if (comparisonData == null || aiData.jumpAttackEnabled != comparisonData.jumpAttackEnabled) behaviourCrablet.enableJumpAttack = aiData.jumpAttackEnabled;
            if (comparisonData == null || aiData.jumpCooldown != comparisonData.jumpCooldown) behaviourCrablet.jumpCooldown = aiData.jumpCooldown;

            if (comparisonData == null || aiData.agroOnNPCType != comparisonData.agroOnNPCType)
            {
                if (Enum.TryParse(aiData.agroOnNPCType, out TriggerRefProxy.NpcType npcType))
                {
                    behaviourCrablet.agroOnNpcType = npcType;
                }
                else
                {
                    behaviourCrablet.agroOnNpcType = 0;
                }
            }

            if (comparisonData == null || aiData.hearingSensitivity != comparisonData.hearingSensitivity) behaviourCrablet.sensors.hearingSensitivity = aiData.hearingSensitivity;
            if (comparisonData == null || aiData.visionRadius != comparisonData.visionRadius) behaviourCrablet.sensors.SetVisionSphere(aiData.visionRadius);
            if (comparisonData == null || aiData.pitchMultiplier != comparisonData.pitchMultiplier) behaviourCrablet.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }

        private static void ApplyOmniWheelData(AIBrain aiBrain, AIData aiData, AIData comparisonData = null)
        {
            BehaviourOmniwheel behaviourOmniwheel = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourOmniwheel>();

            if (comparisonData == null || aiData.health != comparisonData.health) behaviourOmniwheel.health.cur_hp = (aiData.health / 100);
            if (comparisonData == null || aiData.leftLegHealth != comparisonData.leftLegHealth) behaviourOmniwheel.health.cur_leg_lf = aiData.leftLegHealth / 100;
            if (comparisonData == null || aiData.rightLegHealth != comparisonData.rightLegHealth) behaviourOmniwheel.health.cur_leg_rt = aiData.rightLegHealth / 100;
            if (comparisonData == null || aiData.leftArmHealth != comparisonData.leftArmHealth) behaviourOmniwheel.health.cur_arm_lf = aiData.leftArmHealth / 100;
            if (comparisonData == null || aiData.rightArmHealth != comparisonData.rightArmHealth) behaviourOmniwheel.health.cur_arm_rt = aiData.rightArmHealth / 100;

            if (comparisonData == null || aiData.accuracy != comparisonData.accuracy) behaviourOmniwheel.accuracy = aiData.accuracy;
            if (comparisonData == null || aiData.gunRange != comparisonData.gunRange) behaviourOmniwheel.gunRange = aiData.gunRange;
            if (comparisonData == null || aiData.reloadTime != comparisonData.reloadTime) behaviourOmniwheel.reloadTime = aiData.reloadTime;
            if (comparisonData == null || aiData.burstSize != comparisonData.burstSize) behaviourOmniwheel.burstSize = (int)aiData.burstSize;
            if (comparisonData == null || aiData.clipSize != comparisonData.clipSize) behaviourOmniwheel.clipSize = (int)aiData.clipSize;

            if (comparisonData == null || aiData.enableThrowAttack != comparisonData.enableThrowAttack) behaviourOmniwheel.enableThrowAttack = aiData.enableThrowAttack;
            if (comparisonData == null || aiData.throwCooldown != comparisonData.throwCooldown) behaviourOmniwheel.throwCooldown = aiData.throwCooldown;
            if (comparisonData == null || aiData.throwMaxRange != comparisonData.throwMaxRange) behaviourOmniwheel.throwMaxRange = aiData.throwMaxRange;
            if (comparisonData == null || aiData.throwMinRange != comparisonData.throwMinRange) behaviourOmniwheel.throwMinRange = aiData.throwMinRange;

            if (comparisonData == null || aiData.agroedSpeed != comparisonData.agroedSpeed) behaviourOmniwheel.agroedSpeed = aiData.agroedSpeed;
            if (comparisonData == null || aiData.roamSpeed != comparisonData.roamSpeed) behaviourOmniwheel.roamSpeed = aiData.roamSpeed;
            if (comparisonData == null || aiData.roamRange != comparisonData.roamRange) behaviourOmniwheel.roamRange = new Vector2(aiData.roamRange, aiData.roamRange);
            if (comparisonData == null || aiData.roamWanders != comparisonData.roamWanders) behaviourOmniwheel.roamWanders = aiData.roamWanders;

            if (aiData.defaultMentalState != "Default") behaviourOmniwheel.SwitchMentalState((BehaviourBaseNav.MentalState)Enum.Parse(typeof(BehaviourBaseNav.MentalState), aiData.defaultMentalState));

            if (comparisonData == null || aiData.agroOnNPCType != comparisonData.agroOnNPCType)
            {
                if (Enum.TryParse(aiData.agroOnNPCType, out TriggerRefProxy.NpcType npcType))
                {
                    behaviourOmniwheel.agroOnNpcType = npcType;
                }
                else
                {
                    behaviourOmniwheel.agroOnNpcType = 0;
                }
            }

            if (comparisonData == null || aiData.meleeRange != comparisonData.meleeRange) behaviourOmniwheel.meleeRange = aiData.meleeRange;

            if (comparisonData == null || aiData.chargeAttackSpeed != comparisonData.chargeAttackSpeed) behaviourOmniwheel.chargeAttackSpeed = aiData.chargeAttackSpeed;
            if (comparisonData == null || aiData.chargeCooldown != comparisonData.chargeCooldown) behaviourOmniwheel.chargeCooldown = aiData.chargeCooldown;
            if (comparisonData == null || aiData.chargePrepSpeed != comparisonData.chargePrepSpeed) behaviourOmniwheel.chargePrepSpeed = aiData.chargePrepSpeed;
            if (comparisonData == null || aiData.chargeWindupDistance != comparisonData.chargeWindupDistance) behaviourOmniwheel.chargeWindupDistance = aiData.chargeWindupDistance;

            if (aiData.defaultOmniEngagedMode != "Default") behaviourOmniwheel.engagedMode = (BehaviourOmniwheel.EngagedMode)Enum.Parse(typeof(BehaviourOmniwheel.EngagedMode), aiData.defaultOmniEngagedMode);
            
            if (comparisonData == null || aiData.hearingSensitivity != comparisonData.hearingSensitivity) behaviourOmniwheel.sensors.hearingSensitivity = aiData.hearingSensitivity;
            if (comparisonData == null || aiData.visionRadius != comparisonData.visionRadius) behaviourOmniwheel.sensors.SetVisionSphere(aiData.visionRadius);
            if (comparisonData == null || aiData.pitchMultiplier != comparisonData.pitchMultiplier) behaviourOmniwheel.sfx.pitchMultiplier = aiData.pitchMultiplier;
        }
    }
}
