using UnityEngine;
using StressLevelZero.Zones;
using StressLevelZero.AI;
using StressLevelZero.Props.Weapons;
using StressLevelZero.Interaction;
using StressLevelZero.Pool;
using PuppetMasta;
using MelonLoader;
using HarmonyLib;
using ModThatIsNotMod;
using UnhollowerRuntimeLib;
using AIModifier.UI;
using System.Reflection;
using System.IO;
using System;
using TMPro;
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

        public static void CreateHooks()
        {
            Hooking.CreateHook(typeof(ZoneSpawner).GetMethod("Spawn", AccessTools.all), typeof(AI.AIManager).GetMethod("OnZoneSpawnerSpawnPostfix", AccessTools.all), false);
            Hooking.CreateHook(typeof(SpawnGun).GetMethod("OnFire", AccessTools.all), typeof(AI.AIManager).GetMethod("OnFireSpawnGun", AccessTools.all), false);
        }

        public static void RegisterClasses()
        {
            //ClassInjector.RegisterTypeInIl2Cpp<HealthPlateController>();
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
            Stream stream = assembly.GetManifestResourceStream("AIModifier.Resources.aimodifier_assets.asset");
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            //

            AssetBundle assetBundle = AssetBundle.LoadFromMemory(memoryStream.ToArray());
            UnityEngine.Object[] data = assetBundle.LoadAllAssets();
            headPlatePrefab = Array.Find(data, element => element.name == "HeadPlate").Cast<GameObject>();
            headPlatePrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
            aiMenuPrefab = Array.Find(data, element => element.name == "AIMenu").Cast<GameObject>();
            aiMenuPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
            numpadPrefab = Array.Find(data, element => element.name == "Numpad").Cast<GameObject>();
            numpadPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
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

        public static void InitialiseXMLData()
        {
            if(!File.Exists(boneworksDirectory + @"\Mods\AIModifier.xml"))
            {
                AIDataManager.GenerateDefaultAIXML();
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

        public static void SaveXMLData<T>(T data)
        {
            var serializer = new XmlSerializer(typeof(T));
            FileStream xmlFile = File.OpenWrite(boneworksDirectory + @"\Mods\AIModifier.xml");
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
            /*
            for(int i = 0; i < 32; i++)
            {
                for(int j = 0; j < 32; j++)
                {
                    MelonLogger.Msg("Ignore collision value between " + LayerMask.LayerToName(i) + " number " + i + " and " + LayerMask.LayerToName(j) + " number " + j + " is " + Physics.GetIgnoreLayerCollision(i, j));
                }
            }
            */

            for (int j = 0; j < 32; j++)
            {
                if(j == 5)
                {
                    Physics.IgnoreLayerCollision(30, j, false);
                }
                else
                {
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
    }
}
