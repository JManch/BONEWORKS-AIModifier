using UnityEngine;
using AIModifier.Utilities;

namespace AIModifier.UI
{
    public static class MenuPointerManager
    {
        private static MenuPointerController rightPointerController;
        private static GameObject rightPointer;
        private static MenuPointerController leftPointerController;
        private static GameObject leftPointer;

        public static PointerHand activePointerHand = PointerHand.Right; // Default to right hand
        public static MenuPointerController activePointer;

        public enum PointerHand
        {
            Right,
            Left
        }

        public static void EnableMenuPointer()
        {
            if (rightPointer == null || leftPointer == null)
            {
                InitialisePointers();
            }

            if(activePointerHand == PointerHand.Right)
            {
                activePointer = rightPointerController;
            }
            else
            {
                activePointer = leftPointerController;
            }

            rightPointer.SetActive(true);
            leftPointer.SetActive(true);
        }

        public static void DisableMenuPointer()
        {
            rightPointer.SetActive(false);
            leftPointer.SetActive(false);
        }

        public static void SwitchActivePointer(PointerHand pointerHand)
        {
            if(pointerHand == PointerHand.Right)
            {
                activePointerHand = PointerHand.Right;
                activePointer = rightPointerController;
            }
            else
            {
                activePointerHand = PointerHand.Left;
                activePointer = leftPointerController;
            }
        }

        private static void InitialisePointers()
        {
            if (rightPointer == null && AssetManager.rightHand != null)
            {
                rightPointer = new GameObject("MenuPointer");
                rightPointer.transform.SetParent(AssetManager.rightHand.transform.FindChild("PalmCenter"));
                rightPointerController = rightPointer.AddComponent<MenuPointerController>();
                rightPointerController.pointerHand = PointerHand.Right;
                rightPointer.SetActive(false);
            }

            if (leftPointer == null && AssetManager.leftHand != null)
            {
                leftPointer = new GameObject("MenuPointer");
                leftPointer.transform.SetParent(AssetManager.leftHand.transform.FindChild("PalmCenter"));
                leftPointerController = leftPointer.AddComponent<MenuPointerController>();
                leftPointerController.pointerHand = PointerHand.Left;
                leftPointer.SetActive(false);
            }
        }
    }
}
