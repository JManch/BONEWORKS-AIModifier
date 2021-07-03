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

        public static void LoadDefaultAIXML()
        {
            // Just copies the "built-in" default AI XML to the active directory and replaces the existing one
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("DefaultAIData.xml"))
            {
                using (FileStream fileStream = new FileStream(boneworksDirectory + aiDataPath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
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

        public static void InitialiseAIDataXML()
        {
            if(!File.Exists(boneworksDirectory + @"\Mods\AIModifier.xml"))
            {
                LoadDefaultAIXML();
            }
            AIDataManager.LoadAIData();
        }

        public static T LoadXMLData<T>()
        {
            var deserializer = new XmlSerializer(typeof(T));
            FileStream xmlFile = File.OpenRead(boneworksDirectory + @"\Mods\AIModifier.xml");
            object obj = deserializer.Deserialize(xmlFile);
            xmlFile.Close();
            T xmlData = (T)obj;
            return xmlData;
        }

        public static void SaveXMLData<T>(T data, string path)
        {
            var serializer = new XmlSerializer(typeof(T));
            FileStream xmlFile = File.OpenWrite(boneworksDirectory + path);
            serializer.Serialize(xmlFile, data);
            xmlFile.Close();
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

        public static void GenerateDefaultAIData()
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

            SaveXMLData(aiDatas, @"\Mods\DefaultAIData.xml");
        }

        private static AIData BehaviourPowerLegsAIData(AIBrain aiBrain)
        {
            BehaviourPowerLegs behaviourPowerLegs = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourPowerLegs>();

            AIData aiData = new AIData();

            aiData.name = SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name);
            aiData.health = behaviourPowerLegs.health.cur_hp;
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
            aiData.agroSpeed = behaviourPowerLegs.agroedSpeed;
            aiData.roamSpeed = behaviourPowerLegs.roamSpeed;
            aiData.roamByDefault = false;
            aiData.roamWanders = behaviourPowerLegs.roamWanders;
            aiData.breakAgroHomeDistance = behaviourPowerLegs.breakAgroHomeDistance;
            aiData.breakAgroTargetDistance = behaviourPowerLegs.breakAgroTargetDistance;
            aiData.investigateRange = behaviourPowerLegs.investigateRange;
            aiData.investigationCooldown = behaviourPowerLegs._investigationCooldown;
            aiData.restingRange = behaviourPowerLegs.restingRange;
            aiData.baseColor = "Default";
            aiData.agroColor = "Default";
            aiData.emissionColor = "Default";
            aiData.aiTickFrequency = behaviourPowerLegs.aiTickFreq;
            aiData.hearingSensitivity = behaviourPowerLegs.sensors.hearingSensitivity;
            aiData.visionFOV = behaviourPowerLegs.sensors.visionFov;
            aiData.pitchMultiplier = behaviourPowerLegs.sfx.pitchMultiplier;
            aiData.hitScaleFactor = behaviourPowerLegs.visualDamage.hitScaleFactor;

            return aiData;
        }

        private static AIData BehaviourCrabletAIData(AIBrain aiBrain)
        {
            BehaviourCrablet behaviourCrablet = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourCrablet>();

            AIData aiData = new AIData();
            aiData.health = behaviourCrablet.health.cur_hp;
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
            aiData.agroSpeed = behaviourCrablet.agroedSpeed;
            aiData.roamSpeed = behaviourCrablet.roamSpeed;
            aiData.roamByDefault = false;
            aiData.roamWanders = behaviourCrablet.roamWanders;
            aiData.breakAgroHomeDistance = behaviourCrablet.breakAgroHomeDistance;
            aiData.breakAgroTargetDistance = behaviourCrablet.breakAgroTargetDistance;
            aiData.investigateRange = behaviourCrablet.investigateRange;
            aiData.investigationCooldown = behaviourCrablet._investigationCooldown;
            aiData.restingRange = behaviourCrablet.restingRange;
            aiData.baseColor = "Default";
            aiData.agroColor = "Default";
            aiData.jumpAttackEnabled = behaviourCrablet.enableJumpAttack;
            aiData.jumpCharge = behaviourCrablet.jumpCharge;
            aiData.jumpCooldown = behaviourCrablet.jumpCooldown;
            aiData.emissionColor = "Default";
            aiData.aiTickFrequency = behaviourCrablet.aiTickFreq;

            if (aiBrain.behaviour.sensors != null)
            {
                aiData.hearingSensitivity = behaviourCrablet.sensors.hearingSensitivity;
                aiData.visionFOV = behaviourCrablet.sensors.visionFov;
            }
            else
            {
                MelonLogger.Msg("sensors is null for " + aiBrain.gameObject.name);
            }


            if (aiBrain.behaviour.sfx != null)
            {
                aiData.pitchMultiplier = behaviourCrablet.sfx.pitchMultiplier;
            }
            else
            {
                MelonLogger.Msg("sfx is null for " + aiBrain.gameObject.name);
            }

            if (aiBrain.behaviour.visualDamage != null)
            {
                aiData.hitScaleFactor = behaviourCrablet.visualDamage.hitScaleFactor;
            }
            else
            {
                MelonLogger.Msg("visual damage is null for " + aiBrain.gameObject.name);
            }
            return aiData;
        }

        private static AIData BehaviourOmniWheelAIData(AIBrain aiBrain)
        {
            BehaviourOmniwheel behaviourOmniwheel = aiBrain.transform.GetChild(0).GetChild(0).GetComponent<BehaviourOmniwheel>();

            AIData aiData = new AIData();
            aiData.health = behaviourOmniwheel.health.cur_hp;
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
            aiData.agroSpeed = behaviourOmniwheel.agroedSpeed;
            aiData.roamSpeed = behaviourOmniwheel.roamSpeed;
            aiData.roamByDefault = false;
            aiData.roamWanders = behaviourOmniwheel.roamWanders;
            aiData.breakAgroHomeDistance = behaviourOmniwheel.breakAgroHomeDistance;
            aiData.breakAgroTargetDistance = behaviourOmniwheel.breakAgroTargetDistance;
            aiData.investigateRange = behaviourOmniwheel.investigateRange;
            aiData.investigationCooldown = behaviourOmniwheel._investigationCooldown;
            aiData.restingRange = behaviourOmniwheel.restingRange;
            aiData.baseColor = "Default";
            aiData.agroColor = "Default";
            aiData.emissionColor = "Default";
            aiData.aiTickFrequency = behaviourOmniwheel.aiTickFreq;

            if (aiBrain.behaviour.sensors != null)
            {
                aiData.hearingSensitivity = behaviourOmniwheel.sensors.hearingSensitivity;
                aiData.visionFOV = behaviourOmniwheel.sensors.visionFov;
            }
            else
            {
                MelonLogger.Msg("sensors is null for " + aiBrain.gameObject.name);
            }


            if (aiBrain.behaviour.sfx != null)
            {
                aiData.pitchMultiplier = behaviourOmniwheel.sfx.pitchMultiplier;
            }
            else
            {
                MelonLogger.Msg("sfx is null for " + aiBrain.gameObject.name);
            }

            if (aiBrain.behaviour.visualDamage != null)
            {
                aiData.hitScaleFactor = behaviourOmniwheel.visualDamage.hitScaleFactor;
            }
            else
            {
                MelonLogger.Msg("visual damage is null for " + aiBrain.gameObject.name);
            }
            return aiData;
        }
    }
}
