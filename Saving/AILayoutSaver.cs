using StressLevelZero.AI;
using UnityEngine;
using System.Collections.Generic;
using AIModifier.AI;
using UnityEngine.SceneManagement;
using ModThatIsNotMod;
using MelonLoader;
using StressLevelZero.Pool;
using System.Linq;

namespace AIModifier.Saving
{
    public static class AILayoutSaver
    {
        public static void SaveAILayout()
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

            Utilities.XMLDataManager.SaveXMLData(layoutData, @"\Mods\Layout.xml");
        }

        public static void LoadAILayout(LayoutData layoutData)
        {


            if (SceneManager.GetActiveScene().name == layoutData.sceneName)
            {
                foreach (AILayoutData aiLayoutData in layoutData.ai)
                {
                    GameObject spawnedAI = GlobalPool.Spawn(aiLayoutData.poolName, aiLayoutData.position, aiLayoutData.rotation);
                    AI.AIManager.ConfigureNewAI(spawnedAI.GetComponent<AIBrain>(), aiLayoutData.aiData);
                }
            }
        }
    }
}
