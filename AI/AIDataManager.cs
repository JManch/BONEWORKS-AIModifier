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

        public static void LoadDefaultAIData()
        {
            Utilities.Utilities.LoadDefaultAIXML();
            LoadAIData();
        }
    }
}
