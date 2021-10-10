using UnityEngine;
using UnhollowerRuntimeLib;
using AIModifier.UI;
using AIModifier.AI;
using System.IO;
using StressLevelZero.Interaction;

namespace AIModifier.Utilities
{
    public static class Utilities
    {     
        public static string aiModifierDirectory { get; private set; }
        public static float triggerThreshold = 0.8f;

        public static void RegisterClasses()
        {
            ClassInjector.RegisterTypeInIl2Cpp<MenuPointerController>();
            ClassInjector.RegisterTypeInIl2Cpp<ButtonController>();
            ClassInjector.RegisterTypeInIl2Cpp<SmoothPlayerFollow>();
            ClassInjector.RegisterTypeInIl2Cpp<Pointer>();
            ClassInjector.RegisterTypeInIl2Cpp<AISelector>();
            ClassInjector.RegisterTypeInIl2Cpp<PointSelector>();
            ClassInjector.RegisterTypeInIl2Cpp<AIPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<AIHealthPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<AISelectedPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<LookAtPlayer>();
            ClassInjector.RegisterTypeInIl2Cpp<AIDataComponent>();
        }

        public static void InitialiseDirectories()
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory().ToString() + @"\UserData\AIModifier\Layouts");
            aiModifierDirectory = Directory.GetCurrentDirectory().ToString() + @"\UserData\AIModifier\";
        }

        public static void InitialiseBoneMenu()
        {
            ModThatIsNotMod.BoneMenu.MenuCategory rootCategory = ModThatIsNotMod.BoneMenu.MenuManager.CreateCategory("AIModifier", Color.grey);
            rootCategory.CreateFunctionElement("Open AIMenu", Color.grey, delegate { AIMenuManager.OpenAIMenu(); });

        }

        public static void SetupCollisionLayers()
        {
            // Setup layer 30 so it only collides with layer 31
            for (int j = 0; j < 32; j++)
            {
                if(j == 31)
                {
                    Physics.IgnoreLayerCollision(30, j, false);
                }
                else
                {
                    Physics.IgnoreLayerCollision(31, j, true);
                    Physics.IgnoreLayerCollision(30, j, true);
                }
            }
        }

        public static bool rightTriggerDown;
        private static bool leftTriggerDown;

        public static bool GetTriggerDown(Hand hand)
        {
            if(hand.handedness == StressLevelZero.Handedness.RIGHT)
            {
                return GetRightTriggerDown();
            }
            else
            {
                return GetLeftTriggerDown();
            }
        }

        private static bool GetLeftTriggerDown()
        {
            if (AssetManager.leftHand != null && !leftTriggerDown && AssetManager.leftHand.controller.GetPrimaryInteractionButtonAxis() > triggerThreshold)
            {
                leftTriggerDown = true;
                return true;
            }
            else if (AssetManager.leftHand != null && AssetManager.leftHand.controller.GetPrimaryInteractionButtonAxis() < triggerThreshold)
            {
                leftTriggerDown = false;
            }

            return false;
        }
        private static bool GetRightTriggerDown()
        {
            if (AssetManager.rightHand != null && !rightTriggerDown && AssetManager.rightHand.controller.GetPrimaryInteractionButtonAxis() > triggerThreshold)
            {
                rightTriggerDown = true;
                return true;
            }
            else if(AssetManager.rightHand != null && AssetManager.rightHand.controller.GetPrimaryInteractionButtonAxis() < triggerThreshold)
            {
                
                rightTriggerDown = false;
            }

            return false;
        }

    }
}
