using UnityEngine;
using ModThatIsNotMod;
using MelonLoader;
using UnityEngine.UI;
using System.Collections.Generic;
using AIModifier.AI;
using System.Linq;

namespace AIModifier.UI
{
    public static class AIMenuManager
    {
        public static Menu aiMenu;
        public static Color uiHighlightColor = new Color(0.3962264f, 0.3962264f, 0.3962264f, 0.7490196f);
        private static SelectorUI aiSelectorUI;

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
            BuildAIMenu(GameObject.Instantiate(Utilities.AssetManager.aiMenuPrefab, Player.rightHand.transform.position + 4 * Player.GetRigManager().transform.forward, Quaternion.identity));
        }

        private static void BuildAISelector()
        {
            if(aiSelectorUI == null || aiSelectorUI.gameObject == null)
            {
                // Build it...
                GameObject aiSelectorPrefab = GameObject.Instantiate(Utilities.AssetManager.aiSelectorPrefab, Utilities.AssetManager.playerPelvis.position + 0.8f * Utilities.AssetManager.playerPelvis.transform.forward, Quaternion.identity);
                MenuPage rootPage = new MenuPage(aiSelectorPrefab.transform.FindChild("RootPage").gameObject);
                aiSelectorUI = new SelectorUI(aiSelectorPrefab, rootPage);

                Transform rootPageTransform = aiSelectorPrefab.transform.FindChild("RootPage");
                TextProperties textProperties = new TextProperties(2, Color.white);

                rootPage.AddElement(new Button(rootPageTransform.FindChild("FordHair").gameObject, "FordHair", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("FordShortHair").gameObject, "FordShortHair", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("EarlyExit").gameObject, "EarlyExit", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("NullBody").gameObject, "NullBody", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("Fordlet").gameObject, "Fordlet", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("Crablet").gameObject, "Crablet", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("OmniProjector").gameObject, "OmniProjector", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("OmniWrecker").gameObject, "OmniWrecker", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("OmniTurret").gameObject, "OmniTurret", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("Turret").gameObject, "Turret", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPageTransform.FindChild("NullRat").gameObject, "NullRat", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));

                rootPage.AddElement(new Button(rootPageTransform.FindChild("Enter").gameObject, "Enter", new TextProperties(3, Color.white), Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                aiSelectorUI.CloseMenu();
            }
        }

        public static void BuildAIMenu(GameObject menuPrefab)
        {
            Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            mainCamera.cullingMask ^= 1 << 30;
            mainCamera.cullingMask ^= 1 << 31;

            bool[] boolArr = { true, false };
            string[] colorArr = { "Default", "Red", "Blue", "Cyan", "Yellow", "Purple", "White", "Black", "Green", "Orange" };
            string[] mentalStates = { "Default", "Rest", "Roam" };
            string[] engagedModes = { "Default", "Stay", "Follow", "Mirror", "Hide" };
            string[] omniEngagedModes = { "Default", "Stay", "Follow", "Hide" };
            string[] NPCTypes = { "FordHair", "FordShortHair", "EarlyExit", "NullBody", "Fordlet", "Crablet", "OmniProjector", "OmniWrecker", "OmniTurret", "Turret", "NullRat" };
            string[] activeStates = { "Inactive", "Active" };

            TextProperties titleTextProperties = new TextProperties(12, Color.white, false, 15);
            TextProperties buttonTextProperties = new TextProperties(10, Color.white);
            TextProperties elementTextProperties = new TextProperties(8, Color.white, true);
            
            #region Page Definitions

            MenuPage rootPage = new MenuPage(menuPrefab.transform.FindChild("RootPage").gameObject);
            MenuPage configureAIPage = new MenuPage(menuPrefab.transform.FindChild("ConfigureAIPage").gameObject);
            MenuPage controlAIPage = new MenuPage(menuPrefab.transform.FindChild("ControlAIPage").gameObject);
            MenuPage additionalSettingsPage1 = new MenuPage(menuPrefab.transform.FindChild("AdditionalSettingsPage1").gameObject);
            MenuPage additionalSettingsPage2 = new MenuPage(menuPrefab.transform.FindChild("AdditionalSettingsPage2").gameObject);
            MenuPage healthSettingsPage = new MenuPage(menuPrefab.transform.FindChild("HealthSettingsPage").gameObject);
            MenuPage gunSettingsPage = new MenuPage(menuPrefab.transform.FindChild("GunSettingsPage").gameObject);
            MenuPage throwSettingsPage = new MenuPage(menuPrefab.transform.FindChild("ThrowSettingsPage").gameObject);
            MenuPage movementSettingsPage = new MenuPage(menuPrefab.transform.FindChild("MovementSettingsPage").gameObject);
            MenuPage behaviourSettingsPage = new MenuPage(menuPrefab.transform.FindChild("BehaviourSettingsPage").gameObject);
            MenuPage crabletSettingsPage = new MenuPage(menuPrefab.transform.FindChild("CrabletSettingsPage").gameObject);
            MenuPage combatSettingsPage = new MenuPage(menuPrefab.transform.FindChild("CombatSettingsPage").gameObject);
            MenuPage omniWheelSettingsPage = new MenuPage(menuPrefab.transform.FindChild("OmniWheelSettingsPage").gameObject);
            MenuPage otherSettingsPage = new MenuPage(menuPrefab.transform.FindChild("OtherSettingsPage").gameObject);

            #endregion
            #region Configure Root Page

            Transform rootPageTransform = menuPrefab.transform.FindChild("RootPage");
            rootPage.AddElement(new TextDisplay(rootPageTransform.FindChild("Title").gameObject, "AI MODIFIER", titleTextProperties));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("ConfigureAIButton").gameObject, "Configure AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("ControlAIButton").gameObject, "Control AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAIPage"); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("SettingsButton").gameObject, "Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("Settings"); }));

            #endregion
            #region Configure AI Page

            Transform configureAIPageTransform = menuPrefab.transform.FindChild("ConfigureAIPage");
            configureAIPage.AddElement(new TextDisplay(configureAIPageTransform.FindChild("Title").gameObject, "CONFIGURE AI", titleTextProperties));
            configureAIPage.AddElement(new GenericSelector<string>(configureAIPageTransform.FindChild("SelectedAIElement").gameObject, "Selected AI:", elementTextProperties, AIDataManager.aiData.Keys.ToArray(), delegate (string s) { AIMenuFunctions.OnSelectedAIChanged(s); }));
            configureAIPage.AddElement(new InputField(configureAIPageTransform.FindChild("HealthElement").gameObject, "Health:", AIDataManager.aiData["NullBody"].health.ToString(), elementTextProperties, int.MinValue, int.MaxValue, delegate(string health) { AIMenuFunctions.UpdateAIHealth(health); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("AdditionalSettingsButton").gameObject, "Additional Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.LoadAIDataIntoUI(); aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("SaveSettingsButton").gameObject, "Permanently Save", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIDataManager.WriteAIDataToDisk(AIMenuFunctions.selectedAI); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));

            #endregion
            #region Control AI Page

            Transform controlAIPageTransform = menuPrefab.transform.FindChild("ControlAIPage");
            controlAIPage.AddElement(new TextDisplay(controlAIPageTransform.FindChild("Title").gameObject, "CONTROL AI", titleTextProperties));
            controlAIPage.AddElement(new GenericSelector<string>(controlAIPageTransform.FindChild("ToggleSelectorElement").gameObject, "Toggle AI Selector", elementTextProperties, activeStates, delegate (string s){ AIMenuFunctions.ToggleAISelector(s); }));
            controlAIPage.AddElement(new Button(controlAIPageTransform.FindChild("ClearSelectedButton").gameObject, "Clear Selected", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AI.AIManager.ClearSelectedAI(); }));
            controlAIPage.AddElement(new Button(controlAIPageTransform.FindChild("ControlAIButton").gameObject, "Control 0 Selected AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AISelectorManager.DisableAISelector(); aiMenu.SwitchPage("ControlAIPage"); }));
            controlAIPage.AddElement(new Button(controlAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { AISelectorManager.DisableAISelector(); aiMenu.SwitchPage("RootPage"); }));


            #endregion
            #region Additional Settings Page 1

            TextProperties additionalSettingsButtonTextProperties = new TextProperties(9, Color.white);
            Transform additionalSettingsPage1Transform = menuPrefab.transform.FindChild("AdditionalSettingsPage1");
            additionalSettingsPage1.AddElement(new TextDisplay(additionalSettingsPage1Transform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("HealthSettingsButton").gameObject, "Health Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("HealthSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("GunSettingsButton").gameObject, "Gun Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("GunSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("ThrowSettingsButton").gameObject, "Throw Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ThrowSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("MovementSettingsButton").gameObject, "Movement Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("MovementSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("BehaviourSettingsButton").gameObject, "Behaviour Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("BehaviourSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("CrabletSettingsButton").gameObject, "Crablet Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CrabletSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("NextPageButton").gameObject, ">", new TextProperties(20, Color.white), Button.ButtonHighlightType.Color, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1Transform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion
            #region Additional Settings Page 2

            Transform additionalSettingsPage2Transform = menuPrefab.transform.FindChild("AdditionalSettingsPage2");
            additionalSettingsPage2.AddElement(new TextDisplay(additionalSettingsPage2Transform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("CombatSettingsButton").gameObject, "Combat Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CombatSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("OmniWheelSettingsButton").gameObject, "OmniWheel Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OmniWheelSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("OtherSettingsButton").gameObject, "Other Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OtherSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("PreviousPageButton").gameObject, "<", new TextProperties(20, Color.white), Button.ButtonHighlightType.Color, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion
            #region Health Settings Page

            Transform healthSettingsPageTransform = menuPrefab.transform.FindChild("HealthSettingsPage");
            healthSettingsPage.AddElement(new TextDisplay(healthSettingsPageTransform.FindChild("Title").gameObject, "HEALTH SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("LeftLegHealthElement").gameObject, "Left Leg Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("RightLegHealthElement").gameObject, "Right Leg Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("LeftArmHealthElement").gameObject, "Left Arm Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftArmHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("RightArmHealthElement").gameObject, "Right Arm Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightArmHealth(s); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreHealthSettings(); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Gun Settings Page

            Transform gunSettingsPageTransform = menuPrefab.transform.FindChild("GunSettingsPage");
            gunSettingsPage.AddElement(new TextDisplay(gunSettingsPageTransform.FindChild("Title").gameObject, "GUN SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("AccuracyElement").gameObject, "Accuracy:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAIAccuracy(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("GunRangeElement").gameObject, "Gun Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunRange(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("ReloadTimeElement").gameObject, "Reload Time:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateReloadTime(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("BurstSizeElement").gameObject, "Burst Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBurstSize(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("ClipSizeElement").gameObject, "Clip Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateClipSize(s); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreGunSettings(); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Throw Settings Page

            Transform throwSettingsPageTransform = menuPrefab.transform.FindChild("ThrowSettingsPage");
            throwSettingsPage.AddElement(new TextDisplay(throwSettingsPageTransform.FindChild("Title").gameObject, "THROW SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            throwSettingsPage.AddElement(new GenericSelector<bool>(throwSettingsPageTransform.FindChild("EnableThrowAttackElement").gameObject, "Enable Throw Attack:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateEnableThrowAttack(b); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowCooldownElement").gameObject, "Throw Cooldown:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowCooldown(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowMaxRangeElement").gameObject, "Throw Max Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMaxRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowMinRangeElement").gameObject, "Throw Min Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMinRange(s); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreThrowSettings(); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Movement Settings Page

            Transform movementSettingsPageTransform = menuPrefab.transform.FindChild("MovementSettingsPage");
            movementSettingsPage.AddElement(new TextDisplay(movementSettingsPageTransform.FindChild("Title").gameObject, "MOVEMENT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("AgroSpeedElement").gameObject, "Agro Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAgroSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("EngagedSpeedElement").gameObject, "Engaged Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateEngagedSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("RoamSpeedElement").gameObject, "Roam Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("RoamRangeElement").gameObject, "Roam Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamRange(s); }));
            movementSettingsPage.AddElement(new GenericSelector<bool>(movementSettingsPageTransform.FindChild("RoamWandersElement").gameObject, "Roam Wanders:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateRoamWanders(b); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreMovementSettings(); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Behaviour Settings Page

            Transform behaviourSettingsPageTransform = menuPrefab.transform.FindChild("BehaviourSettingsPage");
            behaviourSettingsPage.AddElement(new TextDisplay(behaviourSettingsPageTransform.FindChild("Title").gameObject, "BEHAVIOUR SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPageTransform.FindChild("DefaultMentalStateElement").gameObject, "Default Mental State:", elementTextProperties, mentalStates, delegate (string s) { AIMenuFunctions.UpdateDefaultMentalState(s); }));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, engagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultEngagedMode(s); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreBehaviourSettings(); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Crablet Settings Page

            Transform crabletSettingsPageTranform = menuPrefab.transform.FindChild("CrabletSettingsPage");
            crabletSettingsPage.AddElement(new TextDisplay(crabletSettingsPageTranform.FindChild("Title").gameObject, "CRABLET SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPageTranform.FindChild("BaseColorElement").gameObject, "Base Color:", elementTextProperties, colorArr, delegate (string s) { AIMenuFunctions.UpdateBaseColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPageTranform.FindChild("AgroColorElement").gameObject, "Agro Color:", elementTextProperties, colorArr, delegate (string s) { AIMenuFunctions.UpdateAgroColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<bool>(crabletSettingsPageTranform.FindChild("JumpAttackEnabledElement").gameObject, "Jump Attack Enabled:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateJumpAttackEnabled(b); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPageTranform.FindChild("JumpCooldownElement").gameObject, "Jump Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCooldown(s); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPageTranform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreCrabletSettings(); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Combat Settings Page

            Transform combatSettingsPageTransform = menuPrefab.transform.FindChild("CombatSettingsPage");
            combatSettingsPage.AddElement(new TextDisplay(combatSettingsPageTransform.FindChild("Title").gameObject, "COMBAT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            BuildAISelector();
            combatSettingsPage.AddElement(new Selector(combatSettingsPageTransform.FindChild("AgroOnNPCTypeElement").gameObject, aiSelectorUI, "Agro On NPC Type:", elementTextProperties, NPCTypes, delegate (string s) { AIMenuFunctions.UpdateAgroOnNPCType(s);  }));
            combatSettingsPage.AddElement(new InputField(combatSettingsPageTransform.FindChild("MeleeRangeElement").gameObject, "Melee Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateMeleeRange(s); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreCombatSettings(); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion
            #region OmniWheel Settings Page

            Transform omniWheelSettingsPageTransform = menuPrefab.transform.FindChild("OmniWheelSettingsPage");
            omniWheelSettingsPage.AddElement(new TextDisplay(omniWheelSettingsPageTransform.FindChild("Title").gameObject, "OMNIWHEEL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargeAttackSpeedElement").gameObject, "Charge Attack Speed:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeAttackSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargeCooldownElement").gameObject, "Charge Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeCooldown(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargePrepSpeedElement").gameObject, "Charge Prep Speed:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargePrepSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargeWindupDistanceElement").gameObject, "Charge Wind-up Distance:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeWindupDistance(s); }));
            omniWheelSettingsPage.AddElement(new GenericSelector<string>(omniWheelSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, omniEngagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultOmniEngagedMode(s); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreOmniWheelSettings(); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion
            #region Other Settings Page

            Transform otherSettingsPageTranform = menuPrefab.transform.FindChild("OtherSettingsPage");
            otherSettingsPage.AddElement(new TextDisplay(otherSettingsPageTranform.FindChild("Title").gameObject, "OTHER SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("HearingSensitivityElement").gameObject, "Hearing Sensitivity:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateHearingSensitivity(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("VisionRadiusElement").gameObject, "Vision Radius:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateVisionRadius(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("PitchMultiplierElement").gameObject, "Pitch Multiplier:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdatePitchMultiplier(s); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPageTranform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreOtherSettings(); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion
            
            // Add the pages to the menu
            aiMenu = new Menu(menuPrefab, rootPage);
            aiMenu.AddPage(configureAIPage);
            aiMenu.AddPage(controlAIPage);
            aiMenu.AddPage(additionalSettingsPage1);
            aiMenu.AddPage(additionalSettingsPage2);
            aiMenu.AddPage(healthSettingsPage);
            aiMenu.AddPage(gunSettingsPage);
            aiMenu.AddPage(throwSettingsPage);
            aiMenu.AddPage(movementSettingsPage);
            aiMenu.AddPage(behaviourSettingsPage);
            aiMenu.AddPage(crabletSettingsPage);
            aiMenu.AddPage(combatSettingsPage);
            aiMenu.AddPage(omniWheelSettingsPage);
            aiMenu.AddPage(otherSettingsPage);
        }
    }
}
