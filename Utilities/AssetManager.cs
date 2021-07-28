using UnityEngine;
using StressLevelZero.Interaction;
using ModThatIsNotMod;
using System.Reflection;
using System.IO;
using System;

namespace AIModifier.Utilities
{
    public static class AssetManager
    {
        // Loaded assets
        public static GameObject healthPlatePrefab { get; private set; }
        public static GameObject selectedPlatePrefab { get; private set; }
        public static GameObject aiMenuPrefab { get; private set; }
        public static GameObject numpadPrefab { get; private set; }
        public static GameObject aiSelectorPrefab { get; private set; }

        // Game references
        public static Hand leftHand { get; private set; }
        public static Hand rightHand { get; private set; }
        public static Transform playerPelvis { get { return Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]").FindChild("Pelvis"); } }

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
                    healthPlatePrefab = Array.Find(data, element => element.name == "HealthPlate").Cast<GameObject>();
                    healthPlatePrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    selectedPlatePrefab = Array.Find(data, element => element.name == "SelectedPlate").Cast<GameObject>();
                    selectedPlatePrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    aiMenuPrefab = Array.Find(data, element => element.name == "AIMenu").Cast<GameObject>();
                    aiMenuPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    numpadPrefab = Array.Find(data, element => element.name == "Numpad").Cast<GameObject>();
                    numpadPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    aiSelectorPrefab = Array.Find(data, element => element.name == "AISelector").Cast<GameObject>();
                    aiSelectorPrefab.hideFlags = HideFlags.DontUnloadUnusedAsset;
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
            catch{ }
        }
    }
}
