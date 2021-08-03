using StressLevelZero.AI;
using UnityEngine;
using System.Collections.Generic;
using AIModifier.AI;
using UnityEngine.SceneManagement;
using System.IO;
using ModThatIsNotMod;
using MelonLoader;
using StressLevelZero.Pool;
using System.Linq;

namespace AIModifier.Saving
{
    public static class AILayoutSaver
    {
        private static Dictionary<string, LayoutData> layoutCache = new Dictionary<string, LayoutData>();

        public static void SaveAILayout(string layoutName)
        {
            AIBrain[] aiBrains = GameObject.FindObjectsOfType<AIBrain>();
            List<AILayoutData> aiLayoutDatas = new List<AILayoutData>();

            foreach (AIBrain aiBrain in aiBrains)
            {
                // Only save AI that are alive
                if(!aiBrain.isDead)
                {
                    AILayoutData aiLayoutData = new AILayoutData
                    {
                        poolName = aiBrain.poolee.pool.name.Remove(0, 7),
                        position = aiBrain.gameObject.transform.position,
                        rotation = aiBrain.gameObject.transform.rotation,
                        aiData = aiBrain.gameObject.GetComponent<AIDataComponent>().aiData,
                        selected = AI.AIManager.SelectedAIContains(aiBrain.name),
                        selectedTarget = AI.AIManager.SelectedTargetAIContrains(aiBrain.name)
                    };

                    aiLayoutDatas.Add(aiLayoutData);
                }
            }

            LayoutData layoutData = new LayoutData
            {
                sceneName = SceneManager.GetActiveScene().name,
                ai = aiLayoutDatas.ToArray()
            };

            // Now parse into a XML and save

            Utilities.XMLDataManager.SaveXMLData(layoutData, @"\Layouts\" + layoutName + @".xml");
        }

        public static void LoadAILayout(string layout)
        {
            if(layout == "")
            {
                return;
            }

            if(layoutCache.TryGetValue(layout, out LayoutData layoutData))
            {
                foreach (AILayoutData aiLayoutData in layoutData.ai)
                {
                    GameObject spawnedAI = GlobalPool.Spawn(aiLayoutData.poolName, aiLayoutData.position, aiLayoutData.rotation);
                    AI.AIManager.ConfigureNewAI(spawnedAI.GetComponent<AIBrain>(), aiLayoutData.aiData);
                }
            }
            else
            {
                MelonLogger.Error("Could not find file for AI layout: " + layout);
            }
        }

        public static string[] GetAILayouts(string scene)
        {
            List<string> sceneLayouts = new List<string>();

            foreach (KeyValuePair<string, LayoutData> keyValuePair in layoutCache)
            {
                if (keyValuePair.Value.sceneName == scene)
                {
                    sceneLayouts.Add(keyValuePair.Key);
                }
            }            

            return sceneLayouts.ToArray();
        }

        public static void CacheAILayouts()
        {
            layoutCache.Clear();

            string[] layouts = Directory.GetFiles(Utilities.Utilities.aiModifierDirectory + @"\Layouts\");

            foreach (string layout in layouts)
            {
                LayoutData layoutData = Utilities.XMLDataManager.LoadXMLData<LayoutData>(@"\Layouts\" + Path.GetFileName(layout));
                if(layoutData != null)
                {
                    layoutCache.Add(Path.GetFileNameWithoutExtension(layout), layoutData);
                }
            }

        }
    }
}
