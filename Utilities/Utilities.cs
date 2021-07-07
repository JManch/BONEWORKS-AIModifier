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
    public static class Utilities
    {
        public static GameObject headPlatePrefab { get; private set; }
        public static GameObject aiMenuPrefab { get; private set; }
        public static Sprite aiMenuBackground { get; private set; }
        public static GameObject numpadPrefab { get; private set; }
        public static Hand leftHand { get; private set; }
        public static Hand rightHand { get; private set; }

        public static string boneworksDirectory;
        private static readonly string aiDataPath = @"\Mods\AIModifier.xml";

        public static void CreateHooks()
        {
            Hooking.CreateHook(typeof(ZoneSpawner).GetMethod("Spawn", AccessTools.all), typeof(AI.AIManager).GetMethod("OnZoneSpawnerSpawnPostfix", AccessTools.all), false);
            Hooking.CreateHook(typeof(SpawnGun).GetMethod("OnFire", AccessTools.all), typeof(AI.AIManager).GetMethod("OnFireSpawnGun", AccessTools.all), false);
        }

        public static void RegisterClasses()
        {
            ClassInjector.RegisterTypeInIl2Cpp<MenuPointerController>();
            ClassInjector.RegisterTypeInIl2Cpp<ButtonController>();
            ClassInjector.RegisterTypeInIl2Cpp<KeyboardController>();
            ClassInjector.RegisterTypeInIl2Cpp<AISelectorController>();
            ClassInjector.RegisterTypeInIl2Cpp<AIPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<AIHeadPlateController>();
        }

        public static void LoadAssetBundles()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AIModifier.Resources.aimodifier_assets.asset"))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    AssetBundle assetBundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());
                    UnityEngine.Object[] data = assetBundle.LoadAllAssets();
                    headPlatePrefab = Array.Find(data, element => element.name == "HeadPlate").Cast<GameObject>();
                    headPlatePrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    aiMenuPrefab = Array.Find(data, element => element.name == "AIMenu").Cast<GameObject>();
                    aiMenuPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    numpadPrefab = Array.Find(data, element => element.name == "Numpad").Cast<GameObject>();
                    numpadPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }
            }
        }

        public static T GetSubAsset<T>(UnityEngine.Object[] allAssets) where T : class
        {
            for (int i = 0; i < allAssets.Length; i++)
            {
                if (allAssets[i].GetType() == typeof(T))
                {
                    return allAssets[i] as T;
                }
            }
            return null;
        }

        public static void LoadBoneworksDirectory()
        {
            boneworksDirectory = Directory.GetCurrentDirectory().ToString();
        }
        private static void ReplaceAIXMLWithDefault()
        {
            // Just copies the "built-in" default AI XML to the active directory and replaces the existing one
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AIModifier.Resources.DefaultAIData.xml"))
            {
                using (FileStream fileStream = new FileStream(boneworksDirectory + aiDataPath, FileMode.Create))
                {
                    if (stream == null)
                    {
                        MelonLogger.Msg("Stream is null");
                    }
                    stream.CopyTo(fileStream);
                }
            }
        }

        public static void InitialiseAIDataXML()
        {
            if(!File.Exists(boneworksDirectory + @"\Mods\AIModifier.xml"))
            {
                ReplaceAIXMLWithDefault();
            }
            AIDataManager.LoadAIData();
        }

        public static List<AIData> LoadDefaultAIData()
        {
            var deserializer = new XmlSerializer(typeof(List<AIData>));

            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AIModifier.Resources.DefaultAIData.xml"))
            {
                return (List<AIData>)deserializer.Deserialize(stream);
            }
        }

        public static T LoadXMLData<T>(string path)
        {
            var deserializer = new XmlSerializer(typeof(T));
            using (FileStream xmlFile = File.OpenRead(boneworksDirectory + path))
            {
                return (T)deserializer.Deserialize(xmlFile);
            }
        }

        public static void SaveXMLData<T>(T data, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (FileStream xmlFile = File.OpenWrite(boneworksDirectory + path))
            {
                serializer.Serialize(xmlFile, data);
            }
        }

        public static void DebugLocalAIBrains()
        {
            AIBrain[] aiBrains = GameObject.FindObjectsOfType<AIBrain>();
            foreach(AIBrain aiBrain in aiBrains)
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

        public static void SetupCollisionLayers()
        {
            // Setup layer 30 so it only collides with layer 31
            for (int j = 0; j < 32; j++)
            {
                if(j == 31)
                {
                    Physics.IgnoreLayerCollision(30, j, false);
                }
                else
                {
                    Physics.IgnoreLayerCollision(31, j, true);
                    Physics.IgnoreLayerCollision(30, j, true);
                }
                
            }
        }

        public static void GetHands()
        {
            try
            {
                leftHand = Player.GetRigManager().transform.FindChild("[PhysicsRig]/Hand (left)").GetComponent<Hand>();
                rightHand = Player.GetRigManager().transform.FindChild("[PhysicsRig]/Hand (right)").GetComponent<Hand>();
            }
            catch
            {

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
                switch(SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)) 
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

            SaveXMLData(aiDatas, @"\Mods\AIDataDebug.xml");
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
            aiData.gunCooldown = behaviourPowerLegs.gunCooldown;
            aiData.reloadTime = behaviourPowerLegs.reloadTime;
            aiData.burstSize = behaviourPowerLegs.burstSize;
            aiData.clipSize = behaviourPowerLegs.clipSize;

            aiData.enableThrowAttack = behaviourPowerLegs.enableThrowAttack;
            aiData.throwCooldown = behaviourPowerLegs.throwCooldown;
            aiData.throwMaxRange = behaviourPowerLegs.throwMaxRange;
            aiData.throwMinRange = behaviourPowerLegs.throwMinRange;
            aiData.throwVelocity = behaviourPowerLegs.throwVelocity;

            aiData.agroedSpeed = behaviourPowerLegs.agroedSpeed;
            aiData.roamSpeed = behaviourPowerLegs.roamSpeed;
            aiData.engagedSpeed = behaviourPowerLegs.engagedSpeed;
            aiData.roamRange = behaviourPowerLegs.roamRange.x;
            aiData.roamWanders = behaviourPowerLegs.roamWanders;

            aiData.defaultMentalState = behaviourPowerLegs.mentalState.ToString();
            aiData.defaultEngagedMode = behaviourPowerLegs.mentalState.ToString();
            aiData.mirrorSkill = behaviourPowerLegs.mirrorSkill;

            aiData.agroOnNPCType = behaviourPowerLegs.agroOnNpcType.ToString();
            aiData.combatProficiency = behaviourPowerLegs.combatProficiency.ToString();
            aiData.meleeRange = behaviourPowerLegs.meleeRange;
            aiData.emissionColor = behaviourPowerLegs.emissColor.ToString();
            aiData.faceExpressionCooldownTime = behaviourPowerLegs.faceAnim._cooldownTime;

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
            aiData.gunCooldown = behaviourCrablet.gunCooldown;
            aiData.reloadTime = behaviourCrablet.reloadTime;
            aiData.burstSize = behaviourCrablet.burstSize;
            aiData.clipSize = behaviourCrablet.clipSize;

            aiData.enableThrowAttack = behaviourCrablet.enableThrowAttack;
            aiData.throwCooldown = behaviourCrablet.throwCooldown;
            aiData.throwMaxRange = behaviourCrablet.throwMaxRange;
            aiData.throwMinRange = behaviourCrablet.throwMinRange;
            aiData.throwVelocity = behaviourCrablet.throwVelocity;

            aiData.agroedSpeed = behaviourCrablet.agroedSpeed;
            aiData.roamSpeed = behaviourCrablet.roamSpeed;
            //aiData.engagedSpeed = behaviourCrablet.engagedSpeed;
            aiData.roamRange = behaviourCrablet.roamRange.x;
            aiData.roamWanders = behaviourCrablet.roamWanders;

            aiData.defaultMentalState = behaviourCrablet.mentalState.ToString();
            aiData.defaultEngagedMode = behaviourCrablet.mentalState.ToString();
            //aiData.mirrorSkill = behaviourCrablet.mirrorSkill;

            aiData.baseColor = behaviourCrablet.baseColor.ToString();
            aiData.agroColor = behaviourCrablet.agroColor.ToString();
            aiData.jumpAttackEnabled = behaviourCrablet.enableJumpAttack;
            aiData.jumpCharge = behaviourCrablet.jumpCharge;
            aiData.jumpCooldown = behaviourCrablet.jumpCooldown;
            aiData.jumpForce = behaviourCrablet.linkJumpForce;

            aiData.agroOnNPCType = behaviourCrablet.agroOnNpcType.ToString();
            //aiData.combatProficiency = behaviourCrablet.combatProficiency.ToString();
            //aiData.meleeRange = behaviourCrablet.meleeRange;
            aiData.emissionColor = behaviourCrablet.emissColor.ToString();
            //aiData.faceExpressionCooldownTime = behaviourCrablet.faceAnim._cooldownTime;

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
            aiData.gunCooldown = behaviourOmniwheel.gunCooldown;
            aiData.reloadTime = behaviourOmniwheel.reloadTime;
            aiData.burstSize = behaviourOmniwheel.burstSize;
            aiData.clipSize = behaviourOmniwheel.clipSize;

            aiData.enableThrowAttack = behaviourOmniwheel.enableThrowAttack;
            aiData.throwCooldown = behaviourOmniwheel.throwCooldown;
            aiData.throwMaxRange = behaviourOmniwheel.throwMaxRange;
            aiData.throwMinRange = behaviourOmniwheel.throwMinRange;
            aiData.throwVelocity = behaviourOmniwheel.throwVelocity;

            aiData.agroedSpeed = behaviourOmniwheel.agroedSpeed;
            aiData.roamSpeed = behaviourOmniwheel.roamSpeed;
            //aiData.engagedSpeed = behaviourOmniwheel.engagedSpeed;
            aiData.roamRange = behaviourOmniwheel.roamRange.x;
            aiData.roamWanders = behaviourOmniwheel.roamWanders;

            aiData.defaultMentalState = behaviourOmniwheel.mentalState.ToString();
            aiData.defaultEngagedMode = behaviourOmniwheel.mentalState.ToString();
            //aiData.mirrorSkill = behaviourOmniwheel.mirrorSkill;

            //aiData.baseColor = behaviourOmniwheel.baseColor.ToString();
            //aiData.agroColor = behaviourOmniwheel.agroColor.ToString();
            //aiData.jumpAttackEnabled = behaviourOmniwheel.enableJumpAttack;
            //aiData.jumpCharge = behaviourOmniwheel.jumpCharge;
            //aiData.jumpCooldown = behaviourOmniwheel.jumpCooldown;
            //aiData.jumpForce = behaviourOmniwheel.linkJumpForce;

            aiData.agroOnNPCType = behaviourOmniwheel.agroOnNpcType.ToString();
            //aiData.combatProficiency = behaviourOmniwheel.combatProficiency.ToString();
            aiData.meleeRange = behaviourOmniwheel.meleeRange;
            aiData.emissionColor = behaviourOmniwheel.emissColor.ToString();
            //aiData.faceExpressionCooldownTime = behaviourOmniwheel.faceAnim._cooldownTime;

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
