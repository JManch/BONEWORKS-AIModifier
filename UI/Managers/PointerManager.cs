using UnityEngine;
using MelonLoader;

namespace AIModifier.UI
{
    public class PointerManager<T> where T : Pointer
    {
        public Pointer rightPointer;
        public Pointer leftPointer;

        public bool pointerEnabled { get; private set; }

        public PointerManager()
        {
            BuildPointers();
        }

        private void BuildPointers()
        {
            if (rightPointer == null && Utilities.AssetManager.rightHand != null)
            {
                GameObject aiSelector = new GameObject("AISelector");
                aiSelector.transform.SetParent(Utilities.AssetManager.rightHand.transform.FindChild("PalmCenter"));
                rightPointer = aiSelector.AddComponent<T>();
                aiSelector.SetActive(false);
            }

            if (leftPointer == null && Utilities.AssetManager.leftHand != null)
            {
                GameObject aiSelector = new GameObject("AISelector");
                aiSelector.transform.SetParent(Utilities.AssetManager.leftHand.transform.FindChild("PalmCenter"));
                leftPointer = aiSelector.AddComponent<T>();
                aiSelector.SetActive(false);
            }
        }

        #region Pointer management

        // Add support for building the pointer if it does not exist
        public void EnablePointer()
        {
            if (MenuPointerManager.activePointerHand == MenuPointerManager.PointerHand.Right && rightPointer != null)
            {
                rightPointer.gameObject.SetActive(true);
            }
            else if(leftPointer != null)
            {
                leftPointer.gameObject.SetActive(true);
            }
            else
            {
                // This means at least one was not built so rebuild
                BuildPointers();
                if(rightPointer != null)
                {
                    rightPointer.gameObject.SetActive(true);
                }
                else if(leftPointer != null)
                {
                    leftPointer.gameObject.SetActive(true);
                }
                else
                {
                    // Pointer creation failed because no controller exists
                    MelonLogger.Error("Failed to create pointer because no controllers exist");
                    return;
                }
            }

            pointerEnabled = true;
        }

        public void DisablePointer()
        {
            if (rightPointer != null && rightPointer.gameObject != null)
            {
                rightPointer.gameObject.SetActive(false);
            }

            if (leftPointer != null && leftPointer.gameObject != null)
            {
                leftPointer.gameObject.SetActive(false);
            }

            pointerEnabled = false;
        }
        #endregion  
    }
}
