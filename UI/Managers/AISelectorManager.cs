using UnityEngine;
using StressLevelZero.AI;
using AIModifier.AI;

namespace AIModifier.UI
{
    public static class AISelectorManager
    {
        private static AISelectorController aiSelectorControllerRight;
        private static AISelectorController aiSelectorControllerLeft;

        public static bool selectorEnabled { get; private set; }

        public static void EnableAISelector()
        {
            if (aiSelectorControllerLeft == null || aiSelectorControllerLeft == null)
            {
                InitialiseSelector();
            }

            if (MenuPointerManager.activePointerHand == MenuPointerManager.PointerHand.Right)
            {
                aiSelectorControllerRight.gameObject.SetActive(true);
            }
            else
            {
                aiSelectorControllerLeft.gameObject.SetActive(true);
            }

            selectorEnabled = true;
        }

        public static void DisableAISelector()
        {
            if(aiSelectorControllerRight != null && aiSelectorControllerRight.gameObject != null)
            {
                aiSelectorControllerRight.gameObject.SetActive(false);
            }

            if(aiSelectorControllerLeft != null && aiSelectorControllerLeft.gameObject != null)
            {
                aiSelectorControllerLeft.gameObject.SetActive(false);
            }
            selectorEnabled = false;
        }

        private static void InitialiseSelector()
        {
            if (Utilities.AssetManager.rightHand != null)
            {
                GameObject aiSelector = new GameObject("AIPointer");
                aiSelector.transform.SetParent(Utilities.AssetManager.rightHand.transform.FindChild("PalmCenter"));
                aiSelectorControllerRight = aiSelector.AddComponent<AISelectorController>();
                aiSelector.SetActive(false);
            }
            
            if (Utilities.AssetManager.leftHand != null)
            {
                GameObject aiSelector = new GameObject("AIPointer");
                aiSelector.transform.SetParent(Utilities.AssetManager.leftHand.transform.FindChild("PalmCenter"));
                aiSelectorControllerLeft = aiSelector.AddComponent<AISelectorController>();
                aiSelector.SetActive(false);
            }
        }
    }
}
