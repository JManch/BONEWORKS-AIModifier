using MelonLoader;
using StressLevelZero.Zones;
using ModThatIsNotMod;
using UnityEngine;
using AIModifier.Utilities;
using AIModifier.AI;
using AIModifier.UI;

namespace AIModifier
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            Utilities.Utilities.LoadBoneworksDirectory();
            Utilities.Utilities.CreateHooks();
            Utilities.Utilities.RegisterClasses();
            Utilities.Utilities.LoadAssetBundles();
            Utilities.Utilities.InitialiseAIDataXML();
            Utilities.Utilities.SetupCollisionLayers();
        }

        public override void OnUpdate()
        {

            // Getting variables
            if(Utilities.Utilities.leftHand == null || Utilities.Utilities.rightHand == null)
            {
                Utilities.Utilities.GetHands();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                AIMenuManager.OpenAIMenu();
            }

            if(Input.GetKeyDown(KeyCode.C))
            {
                Utilities.Utilities.GenerateDefaultAIData();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                //AISelectorManager.EnableAISelector();


                Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                mainCamera.cullingMask ^= 1 << 30;
                mainCamera.cullingMask ^= 1 << 31;

                /*
                if (Utilities.Utilities.aiMenuPrefab == null)
                {
                    MelonLogger.Msg("aiMenuPrefab is null");
                }
                else
                {
                    MelonLogger.Msg("aiMenuPrefab is not null");
                }

                if (Utilities.Utilities.healthPlatePrefab == null)
                {
                    MelonLogger.Msg("healthPlatePrefab is null");
                }
                else
                {
                    MelonLogger.Msg("healthPlatePrefab is not null");
                }
                */
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                AIMenuFunctions.FreezeSelectedAI();
            }
        }
    }
}
