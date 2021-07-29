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
        public static AISelectedPlateController.SelectedType aiSelectorType;

        // Pointers
        public static PointerManager<AISelector> aiSelectorPointer;

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
                
                aiSelectorUI = new SelectorUI(aiSelectorPrefab);
                MenuPage rootPage = new MenuPage(aiSelectorPrefab.transform.FindChild("RootPage").gameObject);
                Transform rootPageTransform = aiSelectorPrefab.transform.FindChild("RootPage");
                TextProperties textProperties = new TextProperties(2, Color.white);
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("FordHair").gameObject, "FordHair", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("FordShortHair").gameObject, "FordShortHair", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("EarlyExit").gameObject, "EarlyExit", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("NullBody").gameObject, "NullBody", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Fordlet").gameObject, "Fordlet", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Crablet").gameObject, "Crablet", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("OmniProjector").gameObject, "OmniProjector", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("OmniWrecker").gameObject, "OmniWrecker", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("OmniTurret").gameObject, "OmniTurret", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Turret").gameObject, "Turret", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("NullRat").gameObject, "NullRat", textProperties, Button.ButtonHighlightType.Underline, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Enter").gameObject, "Enter", new TextProperties(3, Color.white), Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                aiSelectorUI.AddPage(rootPage);
                aiSelectorUI.CloseMenu();
            }
        }

        private static void BuildAIPointers()
        {
            aiSelectorPointer = new PointerManager<AISelector>();
        }

        public static void BuildAIMenu(GameObject menuPrefab)
        {
            BuildAIPointers();

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

            aiMenu = new Menu(menuPrefab);
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
            MenuPage controlAISettingsPage = new MenuPage(menuPrefab.transform.FindChild("ControlAISettingsPage").gameObject);
            MenuPage agroTargetsPage = new MenuPage(menuPrefab.transform.FindChild("AgroTargetsPage").gameObject);

            #endregion
            #region Configure Root Page

            Transform rootPageTransform = menuPrefab.transform.FindChild("RootPage");
            rootPage.AddElement(new TextDisplay(rootPage, rootPageTransform.FindChild("Title").gameObject, "AI MODIFIER", titleTextProperties));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("ConfigureAIButton").gameObject, "Configure AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("ControlAIButton").gameObject, "Control AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAIPage"); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("SettingsButton").gameObject, "Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("Settings"); }));

            #endregion
            #region Configure AI Page

            Transform configureAIPageTransform = menuPrefab.transform.FindChild("ConfigureAIPage");
            configureAIPage.AddElement(new TextDisplay(configureAIPage, configureAIPageTransform.FindChild("Title").gameObject, "CONFIGURE AI", titleTextProperties));
            configureAIPage.AddElement(new GenericSelector<string>(configureAIPage, configureAIPageTransform.FindChild("SelectedAIElement").gameObject, "Selected AI:", elementTextProperties, AIDataManager.aiData.Keys.ToArray(), delegate (string s) { AIMenuFunctions.OnSelectedAIChanged(s); }));
            configureAIPage.AddElement(new InputField(configureAIPage, configureAIPageTransform.FindChild("HealthElement").gameObject, "Health:", AIDataManager.aiData["NullBody"].health.ToString(), elementTextProperties, int.MinValue, int.MaxValue, delegate(string health) { AIMenuFunctions.UpdateAIHealth(health); }));
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("AdditionalSettingsButton").gameObject, "Additional Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.LoadAIDataIntoUI(); aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("SaveSettingsButton").gameObject, "Permanently Save", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIDataManager.WriteAIDataToDisk(AIMenuFunctions.selectedAI); }));
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));

            #endregion
            #region Control AI Page

            Transform controlAIPageTransform = menuPrefab.transform.FindChild("ControlAIPage");
            controlAIPage.AddElement(new TextDisplay(controlAIPage, controlAIPageTransform.FindChild("Title").gameObject, "CONTROL AI", titleTextProperties));
            controlAIPage.AddElement(new GenericSelector<string>(controlAIPage, controlAIPageTransform.FindChild("ToggleSelectorElement").gameObject, "Toggle AI Selector", elementTextProperties, activeStates, delegate (string s){ AIMenuFunctions.ToggleControlAISelector(s); }));
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("ClearSelectedButton").gameObject, "Clear Selected", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AI.AIManager.ClearSelectedAI(); }));
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("ControlAIButton").gameObject, "Control 0 Selected AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiSelectorPointer.EnablePointer(); aiMenu.SwitchPage("ControlAISettingsPage"); }));
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiSelectorPointer.DisablePointer(); aiMenu.SwitchPage("RootPage"); }));


            #endregion
            #region Additional Settings Page 1

            TextProperties additionalSettingsButtonTextProperties = new TextProperties(9, Color.white);
            Transform additionalSettingsPage1Transform = menuPrefab.transform.FindChild("AdditionalSettingsPage1");
            additionalSettingsPage1.AddElement(new TextDisplay(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("HealthSettingsButton").gameObject, "Health Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("HealthSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("GunSettingsButton").gameObject, "Gun Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("GunSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("ThrowSettingsButton").gameObject, "Throw Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ThrowSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("MovementSettingsButton").gameObject, "Movement Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("MovementSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("BehaviourSettingsButton").gameObject, "Behaviour Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("BehaviourSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("CrabletSettingsButton").gameObject, "Crablet Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CrabletSettingsPage"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("NextPageButton").gameObject, ">", new TextProperties(20, Color.white), Button.ButtonHighlightType.Color, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion
            #region Additional Settings Page 2

            Transform additionalSettingsPage2Transform = menuPrefab.transform.FindChild("AdditionalSettingsPage2");
            additionalSettingsPage2.AddElement(new TextDisplay(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("CombatSettingsButton").gameObject, "Combat Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CombatSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("OmniWheelSettingsButton").gameObject, "OmniWheel Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OmniWheelSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("OtherSettingsButton").gameObject, "Other Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OtherSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("PreviousPageButton").gameObject, "<", new TextProperties(20, Color.white), Button.ButtonHighlightType.Color, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion
            #region Health Settings Page

            Transform healthSettingsPageTransform = menuPrefab.transform.FindChild("HealthSettingsPage");
            healthSettingsPage.AddElement(new TextDisplay(healthSettingsPage, healthSettingsPageTransform.FindChild("Title").gameObject, "HEALTH SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("LeftLegHealthElement").gameObject, "Left Leg Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("RightLegHealthElement").gameObject, "Right Leg Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("LeftArmHealthElement").gameObject, "Left Arm Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftArmHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("RightArmHealthElement").gameObject, "Right Arm Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightArmHealth(s); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPage, healthSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreHealthSettings(); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPage, healthSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Gun Settings Page

            Transform gunSettingsPageTransform = menuPrefab.transform.FindChild("GunSettingsPage");
            gunSettingsPage.AddElement(new TextDisplay(gunSettingsPage, gunSettingsPageTransform.FindChild("Title").gameObject, "GUN SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("AccuracyElement").gameObject, "Accuracy:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAIAccuracy(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("GunRangeElement").gameObject, "Gun Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunRange(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("ReloadTimeElement").gameObject, "Reload Time:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateReloadTime(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("BurstSizeElement").gameObject, "Burst Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBurstSize(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("ClipSizeElement").gameObject, "Clip Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateClipSize(s); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPage, gunSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreGunSettings(); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPage, gunSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Throw Settings Page

            Transform throwSettingsPageTransform = menuPrefab.transform.FindChild("ThrowSettingsPage");
            throwSettingsPage.AddElement(new TextDisplay(throwSettingsPage, throwSettingsPageTransform.FindChild("Title").gameObject, "THROW SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            throwSettingsPage.AddElement(new GenericSelector<bool>(throwSettingsPage, throwSettingsPageTransform.FindChild("EnableThrowAttackElement").gameObject, "Enable Throw Attack:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateEnableThrowAttack(b); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPage, throwSettingsPageTransform.FindChild("ThrowCooldownElement").gameObject, "Throw Cooldown:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowCooldown(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPage, throwSettingsPageTransform.FindChild("ThrowMaxRangeElement").gameObject, "Throw Max Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMaxRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPage, throwSettingsPageTransform.FindChild("ThrowMinRangeElement").gameObject, "Throw Min Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMinRange(s); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPage, throwSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreThrowSettings(); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPage, throwSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Movement Settings Page

            Transform movementSettingsPageTransform = menuPrefab.transform.FindChild("MovementSettingsPage");
            movementSettingsPage.AddElement(new TextDisplay(movementSettingsPage, movementSettingsPageTransform.FindChild("Title").gameObject, "MOVEMENT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("AgroSpeedElement").gameObject, "Agro Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAgroSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("EngagedSpeedElement").gameObject, "Engaged Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateEngagedSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("RoamSpeedElement").gameObject, "Roam Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("RoamRangeElement").gameObject, "Roam Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamRange(s); }));
            movementSettingsPage.AddElement(new GenericSelector<bool>(movementSettingsPage, movementSettingsPageTransform.FindChild("RoamWandersElement").gameObject, "Roam Wanders:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateRoamWanders(b); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPage, movementSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreMovementSettings(); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPage, movementSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Behaviour Settings Page

            Transform behaviourSettingsPageTransform = menuPrefab.transform.FindChild("BehaviourSettingsPage");
            behaviourSettingsPage.AddElement(new TextDisplay(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("Title").gameObject, "BEHAVIOUR SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("DefaultMentalStateElement").gameObject, "Default Mental State:", elementTextProperties, mentalStates, delegate (string s) { AIMenuFunctions.UpdateDefaultMentalState(s); }));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, engagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultEngagedMode(s); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreBehaviourSettings(); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Crablet Settings Page

            Transform crabletSettingsPageTranform = menuPrefab.transform.FindChild("CrabletSettingsPage");
            crabletSettingsPage.AddElement(new TextDisplay(crabletSettingsPage, crabletSettingsPageTranform.FindChild("Title").gameObject, "CRABLET SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPage, crabletSettingsPageTranform.FindChild("BaseColorElement").gameObject, "Base Color:", elementTextProperties, colorArr, delegate (string s) { AIMenuFunctions.UpdateBaseColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPage, crabletSettingsPageTranform.FindChild("AgroColorElement").gameObject, "Agro Color:", elementTextProperties, colorArr, delegate (string s) { AIMenuFunctions.UpdateAgroColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<bool>(crabletSettingsPage, crabletSettingsPageTranform.FindChild("JumpAttackEnabledElement").gameObject, "Jump Attack Enabled:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateJumpAttackEnabled(b); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPage, crabletSettingsPageTranform.FindChild("JumpCooldownElement").gameObject, "Jump Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCooldown(s); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPage, crabletSettingsPageTranform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreCrabletSettings(); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPage, crabletSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion
            #region Combat Settings Page

            Transform combatSettingsPageTransform = menuPrefab.transform.FindChild("CombatSettingsPage");
            combatSettingsPage.AddElement(new TextDisplay(combatSettingsPage, combatSettingsPageTransform.FindChild("Title").gameObject, "COMBAT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            BuildAISelector();
            combatSettingsPage.AddElement(new Selector(combatSettingsPage, combatSettingsPageTransform.FindChild("AgroOnNPCTypeElement").gameObject, aiSelectorUI, "Agro On NPC Type:", elementTextProperties, NPCTypes, delegate (string s) { AIMenuFunctions.UpdateAgroOnNPCType(s);  }));
            combatSettingsPage.AddElement(new InputField(combatSettingsPage, combatSettingsPageTransform.FindChild("MeleeRangeElement").gameObject, "Melee Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateMeleeRange(s); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPage, combatSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreCombatSettings(); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPage, combatSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion
            #region OmniWheel Settings Page

            Transform omniWheelSettingsPageTransform = menuPrefab.transform.FindChild("OmniWheelSettingsPage");
            omniWheelSettingsPage.AddElement(new TextDisplay(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("Title").gameObject, "OMNIWHEEL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargeAttackSpeedElement").gameObject, "Charge Attack Speed:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeAttackSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargeCooldownElement").gameObject, "Charge Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeCooldown(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargePrepSpeedElement").gameObject, "Charge Prep Speed:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargePrepSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargeWindupDistanceElement").gameObject, "Charge Wind-up Distance:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeWindupDistance(s); }));
            omniWheelSettingsPage.AddElement(new GenericSelector<string>(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, omniEngagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultOmniEngagedMode(s); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreOmniWheelSettings(); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion
            #region Other Settings Page

            Transform otherSettingsPageTranform = menuPrefab.transform.FindChild("OtherSettingsPage");
            otherSettingsPage.AddElement(new TextDisplay(otherSettingsPage, otherSettingsPageTranform.FindChild("Title").gameObject, "OTHER SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            otherSettingsPage.AddElement(new InputField(otherSettingsPage, otherSettingsPageTranform.FindChild("HearingSensitivityElement").gameObject, "Hearing Sensitivity:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateHearingSensitivity(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPage, otherSettingsPageTranform.FindChild("VisionRadiusElement").gameObject, "Vision Radius:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateVisionRadius(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPage, otherSettingsPageTranform.FindChild("PitchMultiplierElement").gameObject, "Pitch Multiplier:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdatePitchMultiplier(s); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPage, otherSettingsPageTranform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreOtherSettings(); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPage, otherSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion
            #region Control AI Settings Page
            Transform controlAISettingsPageTransform = menuPrefab.transform.FindChild("ControlAISettingsPage");
            controlAISettingsPage.AddElement(new TextDisplay(controlAISettingsPage, controlAISettingsPageTransform.FindChild("Title").gameObject, "CONTROL AI SETTINGS", new TextProperties(10, Color.white, false, 15)));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("AgroTargetsButton").gameObject, "Set Agro Targets", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AgroTargetsPage"); }));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAIPage"); }));

            #endregion

            #region Agro Targets Page

            Transform agroTargetsPageTransform = menuPrefab.transform.FindChild("AgroTargetsPage");
            agroTargetsPage.AddElement(new TextDisplay(agroTargetsPage, agroTargetsPageTransform.FindChild("Title").gameObject, "AGRO TARGETS", titleTextProperties));
            agroTargetsPage.AddElement(new GenericSelector<string>(agroTargetsPage, agroTargetsPageTransform.FindChild("ToggleSelectorElement").gameObject, "Toggle Target Selector", new TextProperties(7, Color.white), activeStates, delegate (string s) { AIMenuFunctions.ToggleAgroTargetsSelector(s); }));
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("ClearSelectedButton").gameObject, "Clear Selected Targets", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AI.AIManager.ClearSelectedTargetAI(); }));
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("StartAgroButton").gameObject, "Start Agro", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.AgroTargets(); }));
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiSelectorPointer.DisablePointer(); aiMenu.SwitchPage("ControlAISettingsPage"); }));


            #endregion

            // Add the pages to the menu
            aiMenu.AddPage(rootPage);
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
            aiMenu.AddPage(controlAISettingsPage);
            aiMenu.AddPage(agroTargetsPage);
        }
    }
}
