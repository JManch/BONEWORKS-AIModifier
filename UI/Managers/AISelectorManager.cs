using UnityEngine;
using StressLevelZero.AI;
using AIModifier.AI;

namespace AIModifier.UI
{
    public static class AISelectorManager
    {
        private static GameObject aiSelector;
        private static AISelectorController aiSelectorController;

        public static void EnableAISelector()
        {
            if(aiSelector == null)
            {
                InitialiseSelector();
            }

            aiSelector.SetActive(true);
        }

        public static void DisableAISelector()
        {
            aiSelector.SetActive(false);
        }

        public static void InitialiseSelector()
        {
            if(aiSelector == null)
            {
                if (MenuPointerManager.activePointerHand == MenuPointerManager.PointerHand.Right && Utilities.Utilities.rightHand != null)
                {
                    aiSelector = new GameObject("AIPointer");
                    aiSelector.transform.SetParent(Utilities.Utilities.rightHand.transform.FindChild("PalmCenter"));
                    aiSelectorController = aiSelector.AddComponent<AISelectorController>();
                }
                else if (Utilities.Utilities.leftHand != null)
                {
                    aiSelector = new GameObject("AIPointer");
                    aiSelector.transform.SetParent(Utilities.Utilities.leftHand.transform.FindChild("PalmCenter"));
                    aiSelectorController = aiSelector.AddComponent<AISelectorController>();
                }
            }
        }

        public static void OnAISelected(AIBrain aiBrain)
        {
            AIModifier.AI.AIManager.selectedAI.Add(aiBrain.name, aiBrain);
            aiBrain.GetComponent<AIHeadPlateController>().EnableSelectedIcon();
        }

        public static void OnAIDeselected(AIBrain aiBrain)
        {
            AIModifier.AI.AIManager.selectedAI.Remove(aiBrain.name);
            aiBrain.GetComponent<AIHeadPlateController>().DisableSelectedIcon();
        }
    }
}
