using UnityEngine;
using ModThatIsNotMod;
using AIModifier.AI;
using System.Linq;
using UnityEngine.SceneManagement;

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
        public static PointerManager<PointSelector> pointSelectorPointer;

        public static void OpenAIMenu()
        {
            if(aiMenu == null || aiMenu.gameObject == null)
            {
                SpawnAIMenu();
            }

            aiMenu.gameObject.transform.position = Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]/Head").position + 4 * Vector3.ProjectOnPlane(Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]/Head").forward, Vector3.up);
            MenuPointerManager.EnableMenuPointer();
            aiMenu.SwitchPage("RootPage");
            aiMenu.OpenMenu();
        }

        private static void CloseAIMenu()
        {
            MenuPointerManager.DisableMenuPointer();
            aiMenu.CloseMenu();
        }

        private static void SpawnAIMenu()
        {
            BuildAIMenu(GameObject.Instantiate(Utilities.AssetManager.aiMenuPrefab, Vector3.zero, Quaternion.identity));
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
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("FordHair").gameObject, "FordHair", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("FordShortHair").gameObject, "FordShortHair", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("EarlyExit").gameObject, "EarlyExit", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("NullBody").gameObject, "NullBody", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Fordlet").gameObject, "Fordlet", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Crablet").gameObject, "Crablet", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("OmniProjector").gameObject, "OmniProjector", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("OmniWrecker").gameObject, "OmniWrecker", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("OmniTurret").gameObject, "OmniTurret", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Turret").gameObject, "Turret", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("NullRat").gameObject, "NullRat", textProperties, Button.ButtonHighlightType.Underline, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Enter").gameObject, "Enter", new TextProperties(3, Color.white), Button.ButtonHighlightType.Color, null, delegate (string s) { aiSelectorUI.OnKeyPressed(s); }));
                aiSelectorUI.AddPage(rootPage);
                aiSelectorUI.CloseMenu();
            }
        }

        private static void BuildAIPointers()
        {
            aiSelectorPointer = new PointerManager<AISelector>();
            pointSelectorPointer = new PointerManager<PointSelector>();
        }

        public static void BuildAIMenu(GameObject menuPrefab)
        {
            //SmoothPlayerFollow smoothPlayerFollow = menuPrefab.AddComponent<SmoothPlayerFollow>();
            //smoothPlayerFollow.distance = 2f;
            menuPrefab.AddComponent<LookAtPlayer>();

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
            string[] scenes = { "scene_mainMenu", "scene_breakroom", "scene_museum", "scene_streets", "scene_runoff", "scene_sewerStation", "scene_warehouse", "scene_subwayStation", "scene_tower", "scene_towerBoss", "scene_dungeon", "scene_arena", "scene_throneRoom", "sandbox_museumBasement", "sandbox_blankBox", "scene_Tuscany", "scene_redactedChamber", "sandbox_handgunBox", "scene_hoverJunkers" };

            TextProperties closeButtonTextProperties = new TextProperties(5, Color.red, false, 15);
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
            MenuPage walkToPointPage = new MenuPage(menuPrefab.transform.FindChild("WalkToPointPage").gameObject);
            MenuPage aiLayoutPage = new MenuPage(menuPrefab.transform.FindChild("AILayoutPage").gameObject);
            MenuPage loadLayoutPage = new MenuPage(menuPrefab.transform.FindChild("LoadLayoutPage").gameObject);
            MenuPage saveLayoutPage = new MenuPage(menuPrefab.transform.FindChild("SaveLayoutPage").gameObject);
            MenuPage settingsPage = new MenuPage(menuPrefab.transform.FindChild("SettingsPage").gameObject);

            #endregion

            #region Configure Root Page

            Transform rootPageTransform = menuPrefab.transform.FindChild("RootPage");
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            rootPage.AddElement(new TextDisplay(rootPage, rootPageTransform.FindChild("Title").gameObject, "AI MODIFIER", titleTextProperties));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("ConfigureAIButton").gameObject, "Configure AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("ControlAIButton").gameObject, "Control AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAIPage"); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("AILayoutButton").gameObject, "AI Layouts", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AILayoutPage"); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("SettingsButton").gameObject, "Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("SettingsPage"); }));

            #endregion

            #region Configure AI Page

            Transform configureAIPageTransform = menuPrefab.transform.FindChild("ConfigureAIPage");
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            configureAIPage.AddElement(new TextDisplay(configureAIPage, configureAIPageTransform.FindChild("Title").gameObject, "CONFIGURE AI", titleTextProperties));
            configureAIPage.AddElement(new GenericSelector<string>(configureAIPage, configureAIPageTransform.FindChild("SelectedAIElement").gameObject, "Selected AI:", elementTextProperties, AIDataManager.aiData.Keys.ToArray(), delegate (string s) { AIMenuFunctions.OnSelectedAIChanged(s); }));
            configureAIPage.AddElement(new InputField(configureAIPage, configureAIPageTransform.FindChild("HealthElement").gameObject, "Health:", AIDataManager.aiData["NullBody"].health.ToString(), InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate(string health) { AIMenuFunctions.UpdateAIHealth(health); }));
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("AdditionalSettingsButton").gameObject, "Additional Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.LoadAIDataIntoUI(); aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("SaveSettingsButton").gameObject, "Permanently Save", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIDataManager.WriteAIDataToDisk(AIMenuFunctions.selectedAI); }));
            configureAIPage.AddElement(new Button(configureAIPage, configureAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));

            #endregion

            #region Control AI Page

            Transform controlAIPageTransform = menuPrefab.transform.FindChild("ControlAIPage");
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            controlAIPage.AddElement(new TextDisplay(controlAIPage, controlAIPageTransform.FindChild("Title").gameObject, "CONTROL AI", titleTextProperties));
            controlAIPage.AddElement(new GenericSelector<string>(controlAIPage, controlAIPageTransform.FindChild("ToggleSelectorElement").gameObject, "Toggle AI Selector", elementTextProperties, activeStates, delegate (string s){ AIMenuFunctions.ToggleControlAISelector(s); }));
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("ClearSelectedButton").gameObject, "Clear Selected", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AI.AIManager.ClearSelectedAI(); }));
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("ControlAIButton").gameObject, "Control 0 Selected AI", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAISettingsPage"); }));
            controlAIPage.AddElement(new Button(controlAIPage, controlAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));
            controlAIPage.onPageClose += delegate { aiSelectorPointer.DisablePointer(); };

            #endregion

            #region Additional Settings Page 1

            TextProperties additionalSettingsButtonTextProperties = new TextProperties(9, Color.white);
            Transform additionalSettingsPage1Transform = menuPrefab.transform.FindChild("AdditionalSettingsPage1");
            additionalSettingsPage1.AddElement(new Button(additionalSettingsPage1, additionalSettingsPage1Transform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
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
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            additionalSettingsPage2.AddElement(new TextDisplay(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("CombatSettingsButton").gameObject, "Combat Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CombatSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("OmniWheelSettingsButton").gameObject, "OmniWheel Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OmniWheelSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("OtherSettingsButton").gameObject, "Other Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OtherSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("PreviousPageButton").gameObject, "<", new TextProperties(20, Color.white), Button.ButtonHighlightType.Color, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2, additionalSettingsPage2Transform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion

            #region Health Settings Page

            Transform healthSettingsPageTransform = menuPrefab.transform.FindChild("HealthSettingsPage");
            healthSettingsPage.AddElement(new Button(healthSettingsPage, healthSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            healthSettingsPage.AddElement(new TextDisplay(healthSettingsPage, healthSettingsPageTransform.FindChild("Title").gameObject, "HEALTH SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("LeftLegHealthElement").gameObject, "Left Leg Health:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("RightLegHealthElement").gameObject, "Right Leg Health:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("LeftArmHealthElement").gameObject, "Left Arm Health:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftArmHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPage, healthSettingsPageTransform.FindChild("RightArmHealthElement").gameObject, "Right Arm Health:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightArmHealth(s); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPage, healthSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreHealthSettings(); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPage, healthSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Gun Settings Page

            Transform gunSettingsPageTransform = menuPrefab.transform.FindChild("GunSettingsPage");
            gunSettingsPage.AddElement(new Button(gunSettingsPage, gunSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            gunSettingsPage.AddElement(new TextDisplay(gunSettingsPage, gunSettingsPageTransform.FindChild("Title").gameObject, "GUN SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("AccuracyElement").gameObject, "Accuracy:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAIAccuracy(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("GunRangeElement").gameObject, "Gun Range:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunRange(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("ReloadTimeElement").gameObject, "Reload Time:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateReloadTime(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("BurstSizeElement").gameObject, "Burst Size:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBurstSize(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPage, gunSettingsPageTransform.FindChild("ClipSizeElement").gameObject, "Clip Size:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateClipSize(s); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPage, gunSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreGunSettings(); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPage, gunSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Throw Settings Page

            Transform throwSettingsPageTransform = menuPrefab.transform.FindChild("ThrowSettingsPage");
            throwSettingsPage.AddElement(new Button(throwSettingsPage, throwSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            throwSettingsPage.AddElement(new TextDisplay(throwSettingsPage, throwSettingsPageTransform.FindChild("Title").gameObject, "THROW SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            throwSettingsPage.AddElement(new GenericSelector<bool>(throwSettingsPage, throwSettingsPageTransform.FindChild("EnableThrowAttackElement").gameObject, "Enable Throw Attack:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateEnableThrowAttack(b); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPage, throwSettingsPageTransform.FindChild("ThrowCooldownElement").gameObject, "Throw Cooldown:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowCooldown(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPage, throwSettingsPageTransform.FindChild("ThrowMaxRangeElement").gameObject, "Throw Max Range:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMaxRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPage, throwSettingsPageTransform.FindChild("ThrowMinRangeElement").gameObject, "Throw Min Range:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMinRange(s); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPage, throwSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreThrowSettings(); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPage, throwSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Movement Settings Page

            Transform movementSettingsPageTransform = menuPrefab.transform.FindChild("MovementSettingsPage");
            movementSettingsPage.AddElement(new Button(movementSettingsPage, movementSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            movementSettingsPage.AddElement(new TextDisplay(movementSettingsPage, movementSettingsPageTransform.FindChild("Title").gameObject, "MOVEMENT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("AgroSpeedElement").gameObject, "Agro Speed:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAgroSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("EngagedSpeedElement").gameObject, "Engaged Speed:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateEngagedSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("RoamSpeedElement").gameObject, "Roam Speed:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPage, movementSettingsPageTransform.FindChild("RoamRangeElement").gameObject, "Roam Range:", "", InputField.InputType.Numpad, elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamRange(s); }));
            movementSettingsPage.AddElement(new GenericSelector<bool>(movementSettingsPage, movementSettingsPageTransform.FindChild("RoamWandersElement").gameObject, "Roam Wanders:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateRoamWanders(b); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPage, movementSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreMovementSettings(); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPage, movementSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Behaviour Settings Page

            Transform behaviourSettingsPageTransform = menuPrefab.transform.FindChild("BehaviourSettingsPage");
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            behaviourSettingsPage.AddElement(new TextDisplay(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("Title").gameObject, "BEHAVIOUR SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("DefaultMentalStateElement").gameObject, "Default Mental State:", elementTextProperties, mentalStates, delegate (string s) { AIMenuFunctions.UpdateDefaultMentalState(s); }));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, engagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultEngagedMode(s); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreBehaviourSettings(); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPage, behaviourSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Crablet Settings Page

            Transform crabletSettingsPageTranform = menuPrefab.transform.FindChild("CrabletSettingsPage");
            crabletSettingsPage.AddElement(new Button(crabletSettingsPage, crabletSettingsPageTranform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            crabletSettingsPage.AddElement(new TextDisplay(crabletSettingsPage, crabletSettingsPageTranform.FindChild("Title").gameObject, "CRABLET SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPage, crabletSettingsPageTranform.FindChild("BaseColorElement").gameObject, "Base Color:", elementTextProperties, colorArr, delegate (string s) { AIMenuFunctions.UpdateBaseColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPage, crabletSettingsPageTranform.FindChild("AgroColorElement").gameObject, "Agro Color:", elementTextProperties, colorArr, delegate (string s) { AIMenuFunctions.UpdateAgroColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<bool>(crabletSettingsPage, crabletSettingsPageTranform.FindChild("JumpAttackEnabledElement").gameObject, "Jump Attack Enabled:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.UpdateJumpAttackEnabled(b); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPage, crabletSettingsPageTranform.FindChild("JumpCooldownElement").gameObject, "Jump Cooldown:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCooldown(s); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPage, crabletSettingsPageTranform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreCrabletSettings(); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPage, crabletSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Combat Settings Page

            Transform combatSettingsPageTransform = menuPrefab.transform.FindChild("CombatSettingsPage");
            combatSettingsPage.AddElement(new Button(combatSettingsPage, combatSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            combatSettingsPage.AddElement(new TextDisplay(combatSettingsPage, combatSettingsPageTransform.FindChild("Title").gameObject, "COMBAT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            BuildAISelector();
            combatSettingsPage.AddElement(new Selector(combatSettingsPage, combatSettingsPageTransform.FindChild("AgroOnNPCTypeElement").gameObject, aiSelectorUI, "Agro On NPC Type:", elementTextProperties, NPCTypes, delegate (string s) { AIMenuFunctions.UpdateAgroOnNPCType(s);  }));
            combatSettingsPage.AddElement(new InputField(combatSettingsPage, combatSettingsPageTransform.FindChild("MeleeRangeElement").gameObject, "Melee Range:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateMeleeRange(s); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPage, combatSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreCombatSettings(); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPage, combatSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion

            #region OmniWheel Settings Page

            Transform omniWheelSettingsPageTransform = menuPrefab.transform.FindChild("OmniWheelSettingsPage");
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            omniWheelSettingsPage.AddElement(new TextDisplay(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("Title").gameObject, "OMNIWHEEL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargeAttackSpeedElement").gameObject, "Charge Attack Speed:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeAttackSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargeCooldownElement").gameObject, "Charge Cooldown:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeCooldown(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargePrepSpeedElement").gameObject, "Charge Prep Speed:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargePrepSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("ChargeWindupDistanceElement").gameObject, "Charge Wind-up Distance:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeWindupDistance(s); }));
            omniWheelSettingsPage.AddElement(new GenericSelector<string>(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, omniEngagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultOmniEngagedMode(s); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreOmniWheelSettings(); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPage, omniWheelSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion

            #region Other Settings Page

            Transform otherSettingsPageTranform = menuPrefab.transform.FindChild("OtherSettingsPage");
            otherSettingsPage.AddElement(new Button(otherSettingsPage, otherSettingsPageTranform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            otherSettingsPage.AddElement(new TextDisplay(otherSettingsPage, otherSettingsPageTranform.FindChild("Title").gameObject, "OTHER SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            otherSettingsPage.AddElement(new InputField(otherSettingsPage, otherSettingsPageTranform.FindChild("HearingSensitivityElement").gameObject, "Hearing Sensitivity:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateHearingSensitivity(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPage, otherSettingsPageTranform.FindChild("VisionRadiusElement").gameObject, "Vision Radius:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateVisionRadius(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPage, otherSettingsPageTranform.FindChild("PitchMultiplierElement").gameObject, "Pitch Multiplier:", "", InputField.InputType.Numpad, elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdatePitchMultiplier(s); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPage, otherSettingsPageTranform.FindChild("RestoreButton").gameObject, "", titleTextProperties, Button.ButtonHighlightType.Color, delegate { AIMenuFunctions.RestoreOtherSettings(); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPage, otherSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion

            #region Control AI Settings Page
            Transform controlAISettingsPageTransform = menuPrefab.transform.FindChild("ControlAISettingsPage");
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            controlAISettingsPage.AddElement(new TextDisplay(controlAISettingsPage, controlAISettingsPageTransform.FindChild("Title").gameObject, "CONTROL AI SETTINGS", new TextProperties(10, Color.white, false, 15)));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("AgroTargetsButton").gameObject, "Set Agro Targets", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AgroTargetsPage"); }));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("WalkToPointButton").gameObject, "Walk To Point", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("WalkToPointPage"); }));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("AgroPlayerButton").gameObject, "Start Following Player", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.SelectedAgroPlayer(); }));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("ResetHitEffectsButton").gameObject, "Reset Hit Effects", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.ResetSelectedHitEffects(); }));
            controlAISettingsPage.AddElement(new Button(controlAISettingsPage, controlAISettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAIPage"); }));

            #endregion

            #region Agro Targets Page

            Transform agroTargetsPageTransform = menuPrefab.transform.FindChild("AgroTargetsPage");
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            agroTargetsPage.AddElement(new TextDisplay(agroTargetsPage, agroTargetsPageTransform.FindChild("Title").gameObject, "AGRO TARGETS", titleTextProperties));
            agroTargetsPage.AddElement(new GenericSelector<string>(agroTargetsPage, agroTargetsPageTransform.FindChild("ToggleSelectorElement").gameObject, "Toggle Target Selector", new TextProperties(7, Color.white), activeStates, delegate (string s) { AIMenuFunctions.ToggleAgroTargetsSelector(s); }));
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("ClearSelectedButton").gameObject, "Clear Selected Targets", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AI.AIManager.ClearSelectedTargetAI(); }));
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("StartAgroButton").gameObject, "Start Agro", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.AgroTargets(); }));
            agroTargetsPage.AddElement(new Button(agroTargetsPage, agroTargetsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAISettingsPage"); }));
            agroTargetsPage.onPageClose += delegate { aiSelectorPointer.DisablePointer(); };

            #endregion

            #region Walk To Point Page

            Transform walkToPointPageTransform = menuPrefab.transform.FindChild("WalkToPointPage");
            walkToPointPage.AddElement(new Button(walkToPointPage, walkToPointPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            walkToPointPage.AddElement(new TextDisplay(walkToPointPage, walkToPointPageTransform.FindChild("Title").gameObject, "WALK TO POINT", new TextProperties(10, Color.white, false, 15)));
            walkToPointPage.AddElement(new Button(walkToPointPage, walkToPointPageTransform.FindChild("SelectPointButton").gameObject, "Select Point", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { pointSelectorPointer.EnablePointer(); }));
            walkToPointPage.AddElement(new Button(walkToPointPage, walkToPointPageTransform.FindChild("WalkToPointButton").gameObject, "Walk To Point", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.WalkToSelectedPoint(); }));
            walkToPointPage.AddElement(new Button(walkToPointPage, walkToPointPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ControlAISettingsPage"); }));
            walkToPointPage.onPageOpen += delegate { AIMenuFunctions.ShowSelectedPointVisual(); };
            walkToPointPage.onPageClose += delegate { pointSelectorPointer.DisablePointer(); AIMenuFunctions.HideSelectedPointVisual(); };

            #endregion

            #region AI Layout Page

            Transform aiLayoutPageTransform = menuPrefab.transform.FindChild("AILayoutPage");
            aiLayoutPage.AddElement(new Button(aiLayoutPage, aiLayoutPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            aiLayoutPage.AddElement(new TextDisplay(aiLayoutPage, aiLayoutPageTransform.FindChild("Title").gameObject, "AI LAYOUTS", titleTextProperties));
            aiLayoutPage.AddElement(new Button(aiLayoutPage, aiLayoutPageTransform.FindChild("LoadButton").gameObject, "Load Layout", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("LoadLayoutPage"); }));
            aiLayoutPage.AddElement(new Button(aiLayoutPage, aiLayoutPageTransform.FindChild("SaveButton").gameObject, "Save Layout", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("SaveLayoutPage"); }));
            aiLayoutPage.AddElement(new Button(aiLayoutPage, aiLayoutPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));

            #endregion

            #region Load Layout Page

            Transform loadLayoutPageTransform = menuPrefab.transform.FindChild("LoadLayoutPage");
            loadLayoutPage.AddElement(new Button(loadLayoutPage, loadLayoutPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            loadLayoutPage.AddElement(new TextDisplay(loadLayoutPage, loadLayoutPageTransform.FindChild("Title").gameObject, "LOAD LAYOUTS", titleTextProperties));
            loadLayoutPage.AddElement(new GenericSelector<string>(loadLayoutPage, loadLayoutPageTransform.FindChild("SceneSelectorElement").gameObject, "Scene:", new TextProperties(7, Color.white, true), scenes, delegate (string s) { AIMenuFunctions.LoadSceneAILayouts(s); }));
            loadLayoutPage.AddElement(new ButtonList(loadLayoutPage, loadLayoutPageTransform.FindChild("LayoutsListElement").gameObject, new TextProperties(7, Color.white, true), delegate (string s) { Saving.AILayoutSaver.LoadAILayout(s); }));
            loadLayoutPage.AddElement(new Button(loadLayoutPage, loadLayoutPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AILayoutPage"); }));
            loadLayoutPage.onPageOpen += delegate { Saving.AILayoutSaver.CacheAILayouts(); loadLayoutPage.GetElement("SceneSelectorElement").SetValue(SceneManager.GetActiveScene().name); };

            #endregion

            #region Save Layout Page

            Transform saveLayoutPageTransform = menuPrefab.transform.FindChild("SaveLayoutPage");
            saveLayoutPage.AddElement(new Button(saveLayoutPage, saveLayoutPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            saveLayoutPage.AddElement(new TextDisplay(saveLayoutPage, saveLayoutPageTransform.FindChild("Title").gameObject, "SAVE LAYOUT", titleTextProperties));
            saveLayoutPage.AddElement(new InputField(saveLayoutPage, saveLayoutPageTransform.FindChild("SaveNameElement").gameObject, "Set Name Below", "", InputField.InputType.Keyboard, elementTextProperties, 0, 0, delegate (string s) { Saving.AILayoutSaver.UpdateSaveName(s); }));
            saveLayoutPage.AddElement(new TextDisplay(saveLayoutPage, saveLayoutPageTransform.FindChild("StatusText").gameObject, "", elementTextProperties));
            saveLayoutPage.AddElement(new Button(saveLayoutPage, saveLayoutPageTransform.FindChild("SaveButton").gameObject, "Save", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { Saving.AILayoutSaver.SaveAILayout(Saving.AILayoutSaver.saveName); }));
            saveLayoutPage.AddElement(new Button(saveLayoutPage, saveLayoutPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AILayoutPage"); }));

            #endregion

            #region Settings Page

            Transform settingsPageTransform = menuPrefab.transform.FindChild("SettingsPage");
            settingsPage.AddElement(new Button(settingsPage, settingsPageTransform.FindChild("CloseMenuButton").gameObject, "CLOSE", closeButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { CloseAIMenu(); }));
            settingsPage.AddElement(new TextDisplay(settingsPage, settingsPageTransform.FindChild("Title").gameObject, "SETTINGS", titleTextProperties));
            settingsPage.AddElement(new GenericSelector<bool>(settingsPage, settingsPageTransform.FindChild("HealthBarsEnabledElement").gameObject, "Health Bars:", elementTextProperties, boolArr, delegate (bool b) { AIMenuFunctions.ToggleHealthBarSetting(b); }));
            settingsPage.AddElement(new Button(settingsPage, settingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));
            settingsPage.onPageOpen += delegate { AIMenuFunctions.LoadPreferencesIntoUI(); };

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
            aiMenu.AddPage(walkToPointPage);
            aiMenu.AddPage(aiLayoutPage);
            aiMenu.AddPage(loadLayoutPage);
            aiMenu.AddPage(saveLayoutPage);
            aiMenu.AddPage(settingsPage);
        }
    }
}
