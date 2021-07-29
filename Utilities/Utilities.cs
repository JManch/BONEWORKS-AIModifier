using UnityEngine;
using StressLevelZero.Zones;
using UnhollowerRuntimeLib;
using AIModifier.UI;
using AIModifier.AI;
using System.IO;

namespace AIModifier.Utilities
{
    public static class Utilities
    {     
        public static string boneworksDirectory;

        public static void RegisterClasses()
        {
            ClassInjector.RegisterTypeInIl2Cpp<MenuPointerController>();
            ClassInjector.RegisterTypeInIl2Cpp<ButtonController>();
            ClassInjector.RegisterTypeInIl2Cpp<SmoothPlayerFollow>();
            ClassInjector.RegisterTypeInIl2Cpp<Pointer>();
            ClassInjector.RegisterTypeInIl2Cpp<AISelector>();
            ClassInjector.RegisterTypeInIl2Cpp<AIPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<AIHealthPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<AISelectedPlateController>();
            ClassInjector.RegisterTypeInIl2Cpp<AIDataComponent>();
        }

        public static void LoadBoneworksDirectory()
        {
            boneworksDirectory = Directory.GetCurrentDirectory().ToString();
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
    }
}
