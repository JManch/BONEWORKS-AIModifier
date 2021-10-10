using MelonLoader;
using AIModifier.Utilities;

namespace AIModifier
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            Utilities.Utilities.InitialiseDirectories();
            Hooks.CreateHooks();
            Utilities.Utilities.RegisterClasses();
            AssetManager.LoadAssetBundles();
            XMLDataManager.InitialiseAIDataXML();
            Utilities.Utilities.SetupCollisionLayers();
            Utilities.Utilities.InitialiseBoneMenu();
            UserPreferences.Initialise();
        }

        public override void OnUpdate()
        {
            if(AssetManager.leftHand == null || AssetManager.rightHand == null)
            {
                AssetManager.GetHands();
            }
        }

    }
}
