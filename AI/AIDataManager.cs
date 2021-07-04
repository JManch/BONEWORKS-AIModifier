using System;
using System.Collections.Generic;
using AIModifier.Utilities;
using MelonLoader;


namespace AIModifier.AI
{
    public static class AIDataManager
    {
        public static Dictionary<string, AIData> aiDataDictionary = new Dictionary<string, AIData>();

        public static void LoadAIData()
        {
            aiDataDictionary.Clear();
            List<AIData> aiDatas = Utilities.Utilities.LoadXMLData<List<AIData>>();
            foreach (AIData aiData in aiDatas)
            {
                aiDataDictionary.Add(aiData.name, aiData);
            }
        }

        /*
        public static void LoadDefaultAIData()
        {
            List<AIData> aiDatas = new List<AIData>();

            // Nullbody
            aiDatas.Add(new AIData
            {
                name = "NullBody",
                headPlateTransformChildPath = "Physics/Root_M/Spine_M/Chest_M/Head_M",
                headPlateHeightOffset = 0.225f,
                health = 100
            });

            // Crablet
            aiDatas.Add(new AIData
            {
                name = "Crablet",
                headPlateTransformChildPath = "PhysicsRig/Head",
                headPlateHeightOffset = 0.4f,
                health = 100
            });


            // Omniturret
            aiDatas.Add(new AIData
            {
                name = "OmniTurret",
                headPlateTransformChildPath = "Physics/Body/Chest",
                headPlateHeightOffset = 0.6f,
                health = 100
            });

            // BrettEnemy
            aiDatas.Add(new AIData
            {
                name = "BrettEnemy",
                headPlateTransformChildPath = "Physics/Root_M/Spine_M/Chest_M/Head_M",
                headPlateHeightOffset = 0.25f,
                health = 100
            });

            // FordVrJunkie
            aiDatas.Add(new AIData
            {
                name = "FordVrJunkie",
                headPlateTransformChildPath = "Physics/Root_M/Spine_M/Chest_M/Head_M",
                headPlateHeightOffset = 0.275f,
                health = 100
            });

            // OmniWrecker
            aiDatas.Add(new AIData
            {
                name = "OmniWrecker",
                headPlateTransformChildPath = "Physics/InnerBrain",
                headPlateHeightOffset = 0.6f,
                health = 100
            });

            // Ford_EarlyExit
            aiDatas.Add(new AIData
            {
                name = "Ford_EarlyExit",
                headPlateTransformChildPath = "Physics/Root_M/Spine_M/Chest_M/Head_M",
                headPlateHeightOffset = 0.25f,
                health = 100
            });

            // Ford_EarlyExit_headset
            aiDatas.Add(new AIData
            {
                name = "Ford_EarlyExit_headset",
                headPlateTransformChildPath = "Physics/Root_M/Spine_M/Chest_M/Head_M",
                headPlateHeightOffset = 0.275f,
                health = 100
            });

            // CrabletPlus
            aiDatas.Add(new AIData
            {
                name = "CrabletPlus",
                headPlateTransformChildPath = "PhysicsRig/Head",
                headPlateHeightOffset = 0.7f,
                health = 100
            });

            // OmniProjector
            aiDatas.Add(new AIData
            {
                name = "OmniProjector",
                headPlateTransformChildPath = "Physics/Root/Head",
                headPlateHeightOffset = 0.3f,
                health = 100
            });

            // NullRat
            aiDatas.Add(new AIData
            {
                name = "NullRat",
                headPlateTransformChildPath = "Physics/Head",
                headPlateHeightOffset = 0.2f,
                health = 100
            });

            // NullBodyCorrupted
            aiDatas.Add(new AIData
            {
                name = "NullBodyCorrupted",
                headPlateTransformChildPath = "Physics/Root_M/Spine_M/Chest_M/Head_M",
                headPlateHeightOffset = 0.225f,
                health = 100
            });

            Utilities.Utilities.SaveXMLData(aiDatas);
            Utilities.Utilities.LoadDefaultAIXML();
            LoadAIData();
        }
        */
    }
}
