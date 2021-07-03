using UnityEngine;
using ModThatIsNotMod;
using MelonLoader;
using UnityEngine.UI;
using System.Collections.Generic;
using AIModifier.AI;

namespace AIModifier.UI
{
    public static class AIMenuManager
    {
        public static Menu aiMenu;
        public static Color uiHighlightColor = new Color(0.3962264f, 0.3962264f, 0.3962264f, 0.7490196f);

        public static void OpenAIMenu()
        {
            if(aiMenu == null || aiMenu.gameObject == null)
            {
                SpawnAIMenu();
            }

            MenuPointerManager.EnableMenuPointer();
            aiMenu.OpenMenu();
        }

        public static void CloseAIMenu()
        {
            MenuPointerManager.DisableMenuPointer();
            aiMenu.CloseMenu();
        }

        private static void SpawnAIMenu()
        {
            BuildAIMenu(GameObject.Instantiate(Utilities.Utilities.aiMenuPrefab, Player.rightHand.transform.position + 4 * Player.GetRigManager().transform.forward, Quaternion.identity));
        }

        public static void BuildAIMenu(GameObject menuPrefab)
        {
            List<bool> boolList = new List<bool>();
            boolList.Add(true);
            boolList.Add(false);

            List<string> colorList = new List<string>();
            colorList.Add("Red");
            colorList.Add("Blue");
            colorList.Add("Cyan");
            colorList.Add("Yellow");
            colorList.Add("Purple");
            colorList.Add("White");
            colorList.Add("Black");
            colorList.Add("Green");
            colorList.Add("Orange");

            TextProperties titleTextProperties = new TextProperties(12, Color.white, false, 15);
            TextProperties buttonTextProperties = new TextProperties(10, Color.white);
            TextProperties elementTextProperties = new TextProperties(8, Color.white, true);

            #region Page Definitions

            MenuPage rootPage = new MenuPage(menuPrefab.transform.FindChild("RootPage").gameObject);
            MenuPage configureAIPage = new MenuPage(menuPrefab.transform.FindChild("ConfigureAIPage").gameObject);
            MenuPage additionalSettingsPage = new MenuPage(menuPrefab.transform.FindChild("AdditionalSettingsPage").gameObject);
            MenuPage gunSettingsPage = new MenuPage(menuPrefab.transform.FindChild("GunSettingsPage").gameObject);
            MenuPage throwSettingsPage = new MenuPage(menuPrefab.transform.FindChild("ThrowSettingsPage").gameObject);
            MenuPage movementSettingsPage = new MenuPage(menuPrefab.transform.FindChild("MovementSettingsPage").gameObject);
            MenuPage pathingSettingsPage = new MenuPage(menuPrefab.transform.FindChild("PathingSettingsPage").gameObject);
            MenuPage crabletSettingsPage = new MenuPage(menuPrefab.transform.FindChild("CrabletSettingsPage").gameObject);
            MenuPage otherSettingsPage = new MenuPage(menuPrefab.transform.FindChild("OtherSettingsPage").gameObject);

            #endregion

            #region Configure Root Page

            Transform rootPageTransform = menuPrefab.transform.FindChild("RootPage");
            rootPage.AddElement(new TextDisplay(rootPageTransform.FindChild("Title").gameObject, "AI MODIFIER", titleTextProperties));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("ConfigureAIButton").gameObject, "Configure AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("ControlAIButton").gameObject, "Control AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAIPage"); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("SettingsButton").gameObject, "Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("Settings"); }));

            #endregion

            MelonLogger.Msg("Built root page");

            #region Configure AI Page

            Transform configureAIPageTransform = menuPrefab.transform.FindChild("ConfigureAIPage");
            configureAIPage.AddElement(new TextDisplay(configureAIPageTransform.FindChild("Title").gameObject, "CONFIGURE AI", titleTextProperties));
            configureAIPage.AddElement(new GenericSelector<string>(configureAIPageTransform.FindChild("SelectedAIElement").gameObject, "Selected AI:", elementTextProperties, new List<string>(AIDataManager.aiDataDictionary.Keys), delegate (string s) { AIMenuFunctions.OnSelectedAIChanged(s); }));
            configureAIPage.AddElement(new InputField(configureAIPageTransform.FindChild("HealthElement").gameObject, "Health:", AIDataManager.aiDataDictionary["NullBody"].health.ToString(), elementTextProperties, int.MinValue, int.MaxValue, delegate(string health) { AIMenuFunctions.UpdateAIHealth(health); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("AdditionalSettingsButton").gameObject, "Additional Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("SaveSettingsButton").gameObject, "Save Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate {  }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));

            #endregion

            #region Configure Additional Settings Page

            TextProperties additionalSettingsButtonTextProperties = new TextProperties(9, Color.white);
            Transform additionalSettingsPageTransform = menuPrefab.transform.FindChild("AdditionalSettingsPage");
            additionalSettingsPage.AddElement(new TextDisplay(additionalSettingsPageTransform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("GunSettingsButton").gameObject, "Gun Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("GunSettingsPage"); }));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("ThrowSettingsButton").gameObject, "Throw Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ThrowSettingsPage"); }));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("MovementSettingsButton").gameObject, "Movement Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("MovementSettingsPage"); }));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("PathingSettingsButton").gameObject, "Pathing Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("PathingSettingsPage"); }));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("CrabletSettingsButton").gameObject, "Crablet Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CrabletSettingsPage"); }));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("OtherSettingsButton").gameObject, "Other Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OtherSettingsPage"); }));
            additionalSettingsPage.AddElement(new Button(additionalSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion

            MelonLogger.Msg("Built additional settings page");

            #region Configure Gun Settings Page

            Transform gunSettingsPageTransform = menuPrefab.transform.FindChild("GunSettingsPage");
            gunSettingsPage.AddElement(new TextDisplay(gunSettingsPageTransform.FindChild("Title").gameObject, "GUN SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("AccuracyElement").gameObject, "Accuracy:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAIAccuracy(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("GunRangeElement").gameObject, "Gun Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunRange(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("GunCooldownElement").gameObject, "Gun Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunCooldown(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("ReloadTimeElement").gameObject, "Reload Time:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateReloadTime(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("BurstSizeElement").gameObject, "Burst Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBurstSize(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("ClipSizeElement").gameObject, "Clip Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateClipSize(s); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));
            #endregion

            MelonLogger.Msg("Built gun settings page");

            #region Configure Throw Settings Page

            Transform throwSettingsPageTransform = menuPrefab.transform.FindChild("ThrowSettingsPage");
            throwSettingsPage.AddElement(new TextDisplay(throwSettingsPageTransform.FindChild("Title").gameObject, "THROW SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            MelonLogger.Msg("yo1");
            
            throwSettingsPage.AddElement(new GenericSelector<bool>(throwSettingsPageTransform.FindChild("EnableThrowAttackElement").gameObject, "Enable Throw Attack:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateEnableThrowAttack(b); }));
            MelonLogger.Msg("yo2");
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowCooldownElement").gameObject, "Throw Cooldown:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowCooldown(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowMaxRangeElement").gameObject, "Throw Max Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMaxRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowMinRangeElement").gameObject, "Throw Min Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMinRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowVelocityElement").gameObject, "Throw Velocity:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowVelocity(s); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));

            #endregion

            MelonLogger.Msg("Built throw settings page");

            #region Movement Settings Page

            Transform movementSettingsPageTransform = menuPrefab.transform.FindChild("MovementSettingsPage");
            movementSettingsPage.AddElement(new TextDisplay(movementSettingsPageTransform.FindChild("Title").gameObject, "MOVEMENT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("AgroSpeedElement").gameObject, "Agro Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAgroSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("RoamSpeedElement").gameObject, "Roam Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamSpeed(s); }));
            movementSettingsPage.AddElement(new GenericSelector<bool>(movementSettingsPageTransform.FindChild("DefaultToRoamingElement").gameObject, "Roam By Default:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateRoamByDefault(b); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));

            #endregion

            #region Pathing Settings Page

            Transform pathingSettingsPageTransform = menuPrefab.transform.FindChild("PathingSettingsPage");
            pathingSettingsPage.AddElement(new TextDisplay(pathingSettingsPageTransform.FindChild("Title").gameObject, "PATHING SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            pathingSettingsPage.AddElement(new GenericSelector<bool>(pathingSettingsPageTransform.FindChild("RoamWandersElement").gameObject, "Roam Wanders:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateRoamWanders(b); }));
            pathingSettingsPage.AddElement(new InputField(pathingSettingsPageTransform.FindChild("BreakAgroHomeDistanceElement").gameObject, "Break Agro Home Distance:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBreakAgroHomeDistance(s); }));
            pathingSettingsPage.AddElement(new InputField(pathingSettingsPageTransform.FindChild("BreakAgroTargetDistanceElement").gameObject, "Break Agro Target Distance:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBreakAgroTargetDistance(s); }));
            pathingSettingsPage.AddElement(new InputField(pathingSettingsPageTransform.FindChild("InvestigateRangeElement").gameObject, "Investigate Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateInvestigateRange(s); }));
            pathingSettingsPage.AddElement(new InputField(pathingSettingsPageTransform.FindChild("InvestigationCooldownElement").gameObject, "Investigation Cooldown:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateInvestigationCooldown(s); }));
            pathingSettingsPage.AddElement(new InputField(pathingSettingsPageTransform.FindChild("RestingRangeElement").gameObject, "Resting Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRestingRange(s); }));
            pathingSettingsPage.AddElement(new Button(pathingSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));

            #endregion

            MelonLogger.Msg("Built pathing settings page");

            #region Crablet Settings Page

            Transform crabletSettingsPageTranform = menuPrefab.transform.FindChild("CrabletSettingsPage");
            crabletSettingsPage.AddElement(new TextDisplay(crabletSettingsPageTranform.FindChild("Title").gameObject, "CRABLET SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPageTranform.FindChild("BaseColorElement").gameObject, "Base Color:", elementTextProperties, colorList, delegate (string s) { AIMenuFunctions.UpdateBaseColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPageTranform.FindChild("AgroColorElement").gameObject, "Agro Color:", elementTextProperties, colorList, delegate (string s) { AIMenuFunctions.UpdateAgroColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<bool>(crabletSettingsPageTranform.FindChild("JumpAttackEnabledElement").gameObject, "Jump Attack Enabled:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateJumpAttackEnabled(b); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPageTranform.FindChild("JumpChargeElement").gameObject, "Jump Charge:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCharge(s); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPageTranform.FindChild("JumpCooldownElement").gameObject, "Jump Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCooldown(s); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));

            #endregion

            MelonLogger.Msg("Built crablet settings page");

            #region Other Settings Page

            Transform otherSettingsPageTranform = menuPrefab.transform.FindChild("OtherSettingsPage");
            otherSettingsPage.AddElement(new TextDisplay(otherSettingsPageTranform.FindChild("Title").gameObject, "OTHER SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            otherSettingsPage.AddElement(new GenericSelector<string>(otherSettingsPageTranform.FindChild("EmissionColorElement").gameObject, "Emmission Color:", elementTextProperties, colorList, delegate (string s) { AIMenuFunctions.UpdateEmissionColor(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("AITickFrequencyElement").gameObject, "AI Tick Freq:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAITickFrequency(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("HearingSensitivityElement").gameObject, "Hearing Sensitivity:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateHearingSensitivity(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("PitchMultiplierElement").gameObject, "Pitch Multiplier:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdatePitchMultiplier(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("HitScaleFactorElement").gameObject, "Hit Scale Factor:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateHitScaleFactor(s); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage"); }));

            #endregion

            MelonLogger.Msg("Built other settings page");

            // Add the pages to the menu
            aiMenu = new Menu(menuPrefab, rootPage);
            aiMenu.AddPage(configureAIPage);
            aiMenu.AddPage(additionalSettingsPage);
            aiMenu.AddPage(gunSettingsPage);
            aiMenu.AddPage(throwSettingsPage);
            aiMenu.AddPage(movementSettingsPage);
            aiMenu.AddPage(pathingSettingsPage);
            aiMenu.AddPage(crabletSettingsPage);
            aiMenu.AddPage(otherSettingsPage);
        }
    }
}
