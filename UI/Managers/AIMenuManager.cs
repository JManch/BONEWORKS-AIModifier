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
            Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            mainCamera.cullingMask ^= 1 << 30;
            mainCamera.cullingMask ^= 1 << 31;

            List<bool> boolList = new List<bool>();
            boolList.Add(true);
            boolList.Add(false);

            List<string> colorList = new List<string>();
            colorList.Add("Default");
            colorList.Add("Red");
            colorList.Add("Blue");
            colorList.Add("Cyan");
            colorList.Add("Yellow");
            colorList.Add("Purple");
            colorList.Add("White");
            colorList.Add("Black");
            colorList.Add("Green");
            colorList.Add("Orange");

            List<string> mentalStates = new List<string>();
            mentalStates.Add("Default");
            mentalStates.Add("Rest");
            mentalStates.Add("Roam");

            List<string> engagedModes = new List<string>();
            engagedModes.Add("Default");
            engagedModes.Add("Stay");
            engagedModes.Add("Follow");
            engagedModes.Add("Mirror");
            engagedModes.Add("Hide");

            List<string> omniEngagedModes = new List<string>();
            omniEngagedModes.Add("Default");
            omniEngagedModes.Add("Stay");
            omniEngagedModes.Add("Follow");
            omniEngagedModes.Add("Hide");

            List<string> NPCTypes = new List<string>();
            NPCTypes.Add("Crablet");
            NPCTypes.Add("Ford Early Exit");
            NPCTypes.Add("Ford");
            NPCTypes.Add("Ford Head");
            NPCTypes.Add("Ford VR Junkie");
            NPCTypes.Add("Null Body");
            NPCTypes.Add("Null Rat");
            NPCTypes.Add("Omni Projector");
            NPCTypes.Add("Omni Turret");
            NPCTypes.Add("Omni Wrecker");
            NPCTypes.Add("Turret");

            List<string> combatProficiency = new List<string>();
            combatProficiency.Add("FistFight");
            combatProficiency.Add("WildPunch");

            TextProperties titleTextProperties = new TextProperties(12, Color.white, false, 15);
            TextProperties buttonTextProperties = new TextProperties(10, Color.white);
            TextProperties elementTextProperties = new TextProperties(8, Color.white, true);

            #region Page Definitions

            MenuPage rootPage = new MenuPage(menuPrefab.transform.FindChild("RootPage").gameObject);
            MenuPage configureAIPage = new MenuPage(menuPrefab.transform.FindChild("ConfigureAIPage").gameObject);
            MenuPage additionalSettingsPage1 = new MenuPage(menuPrefab.transform.FindChild("AdditionalSettingsPage1").gameObject);
            MenuPage additionalSettingsPage2 = new MenuPage(menuPrefab.transform.FindChild("AdditionalSettingsPage2").gameObject);
            MenuPage healthSettingsPage = new MenuPage(menuPrefab.transform.FindChild("HealthSettingsPage").gameObject);
            MenuPage gunSettingsPage = new MenuPage(menuPrefab.transform.FindChild("GunSettingsPage").gameObject);
            MenuPage throwSettingsPage = new MenuPage(menuPrefab.transform.FindChild("ThrowSettingsPage").gameObject);
            MenuPage movementSettingsPage = new MenuPage(menuPrefab.transform.FindChild("MovementSettingsPage").gameObject);
            MenuPage behaviourSettingsPage = new MenuPage(menuPrefab.transform.FindChild("BehaviourSettingsPage").gameObject);
            MenuPage crabletSettingsPage = new MenuPage(menuPrefab.transform.FindChild("CrabletSettingsPage").gameObject);
            MenuPage combatSettingsPage = new MenuPage(menuPrefab.transform.FindChild("CombatSettingsPage").gameObject);
            MenuPage visualSettingsPage = new MenuPage(menuPrefab.transform.FindChild("VisualSettingsPage").gameObject);
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
            configureAIPage.AddElement(new GenericSelector<string>(configureAIPageTransform.FindChild("SelectedAIElement").gameObject, "Selected AI:", elementTextProperties, new List<string>(AIDataManager.aiDataDictionary.Keys), delegate (string s) { AIMenuFunctions.OnSelectedAIChanged(s); }));
            configureAIPage.AddElement(new InputField(configureAIPageTransform.FindChild("HealthElement").gameObject, "Health:", AIDataManager.aiDataDictionary["NullBody"].health.ToString(), elementTextProperties, int.MinValue, int.MaxValue, delegate(string health) { AIMenuFunctions.UpdateAIHealth(health); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("AdditionalSettingsButton").gameObject, "Additional Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("SaveSettingsButton").gameObject, "Save Settings", buttonTextProperties, Button.ButtonHighlightType.Underline, delegate {  }));
            configureAIPage.AddElement(new Button(configureAIPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("RootPage"); }));

            #endregion

            #region Configure Additional Settings Page 1

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

            #region Configure Additional Settings Page 2

            Transform additionalSettingsPage2Transform = menuPrefab.transform.FindChild("AdditionalSettingsPage2");
            additionalSettingsPage2.AddElement(new TextDisplay(additionalSettingsPage2Transform.FindChild("Title").gameObject, "ADDITIONAL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("CombatSettingsButton").gameObject, "Combat Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("CombatSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("VisualSettingsButton").gameObject, "Visual Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("VisualSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("OmniWheelSettingsButton").gameObject, "OmniWheel Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OmniWheelSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("OtherSettingsButton").gameObject, "Other Settings", additionalSettingsButtonTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("OtherSettingsPage"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("PreviousPageButton").gameObject, "<", new TextProperties(20, Color.white), Button.ButtonHighlightType.Color, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));
            additionalSettingsPage2.AddElement(new Button(additionalSettingsPage2Transform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("ConfigureAIPage"); }));

            #endregion

            #region Configure Health Settings Page

            Transform healthSettingsPageTransform = menuPrefab.transform.FindChild("HealthSettingsPage");
            healthSettingsPage.AddElement(new TextDisplay(healthSettingsPageTransform.FindChild("Title").gameObject, "HEALTH SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("LeftLegHealthElement").gameObject, "Left Leg Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("RightLegHealthElement").gameObject, "Right Leg Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightLegHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("LeftArmHealthElement").gameObject, "Left Arm Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateLeftArmHealth(s); }));
            healthSettingsPage.AddElement(new InputField(healthSettingsPageTransform.FindChild("RightArmHealthElement").gameObject, "Right Arm Health:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRightArmHealth(s); }));
            healthSettingsPage.AddElement(new Button(healthSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Configure Gun Settings Page

            Transform gunSettingsPageTransform = menuPrefab.transform.FindChild("GunSettingsPage");
            gunSettingsPage.AddElement(new TextDisplay(gunSettingsPageTransform.FindChild("Title").gameObject, "GUN SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("AccuracyElement").gameObject, "Accuracy:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAIAccuracy(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("GunRangeElement").gameObject, "Gun Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunRange(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("GunCooldownElement").gameObject, "Gun Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateGunCooldown(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("ReloadTimeElement").gameObject, "Reload Time:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateReloadTime(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("BurstSizeElement").gameObject, "Burst Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateBurstSize(s); }));
            gunSettingsPage.AddElement(new InputField(gunSettingsPageTransform.FindChild("ClipSizeElement").gameObject, "Clip Size:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateClipSize(s); }));
            gunSettingsPage.AddElement(new Button(gunSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Configure Throw Settings Page

            Transform throwSettingsPageTransform = menuPrefab.transform.FindChild("ThrowSettingsPage");
            throwSettingsPage.AddElement(new TextDisplay(throwSettingsPageTransform.FindChild("Title").gameObject, "THROW SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            throwSettingsPage.AddElement(new GenericSelector<bool>(throwSettingsPageTransform.FindChild("EnableThrowAttackElement").gameObject, "Enable Throw Attack:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateEnableThrowAttack(b); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowCooldownElement").gameObject, "Throw Cooldown:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowCooldown(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowMaxRangeElement").gameObject, "Throw Max Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMaxRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowMinRangeElement").gameObject, "Throw Min Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowMinRange(s); }));
            throwSettingsPage.AddElement(new InputField(throwSettingsPageTransform.FindChild("ThrowVelocityElement").gameObject, "Throw Velocity:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateThrowVelocity(s); }));
            throwSettingsPage.AddElement(new Button(throwSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            #region Configure Movement Settings Page

            Transform movementSettingsPageTransform = menuPrefab.transform.FindChild("MovementSettingsPage");
            movementSettingsPage.AddElement(new TextDisplay(movementSettingsPageTransform.FindChild("Title").gameObject, "MOVEMENT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("AgroSpeedElement").gameObject, "Agro Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateAgroSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("EngagedSpeedElement").gameObject, "Engaged Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateEngagedSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("RoamSpeedElement").gameObject, "Roam Speed:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamSpeed(s); }));
            movementSettingsPage.AddElement(new InputField(movementSettingsPageTransform.FindChild("RoamRangeElement").gameObject, "Roam Range:", "", elementTextProperties, 0, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateRoamRange(s); }));
            movementSettingsPage.AddElement(new GenericSelector<bool>(movementSettingsPageTransform.FindChild("RoamWandersElement").gameObject, "Roam Wanders:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateRoamWanders(b); }));
            movementSettingsPage.AddElement(new Button(movementSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { AIMenuFunctions.LoadAdditionalSettings(); aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            MelonLogger.Msg("Built Movement Settings Page");

            #region Configure Behaviour Settings Page

            Transform behaviourSettingsPageTransform = menuPrefab.transform.FindChild("BehaviourSettingsPage");
            behaviourSettingsPage.AddElement(new TextDisplay(behaviourSettingsPageTransform.FindChild("Title").gameObject, "BEHAVIOUR SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPageTransform.FindChild("DefaultMentalStateElement").gameObject, "Default Mental State:", elementTextProperties, mentalStates, delegate (string s) { AIMenuFunctions.UpdateDefaultMentalState(s); }));
            behaviourSettingsPage.AddElement(new GenericSelector<string>(behaviourSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, engagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultEngagedMode(s); }));
            behaviourSettingsPage.AddElement(new InputField(behaviourSettingsPageTransform.FindChild("MirrorSkillElement").gameObject, "Mirror Skill:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateMirrorSkill(s); }));
            behaviourSettingsPage.AddElement(new Button(behaviourSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            MelonLogger.Msg("Built Behaviour Settings Page");

            #region Configure Crablet Settings Page

            Transform crabletSettingsPageTranform = menuPrefab.transform.FindChild("CrabletSettingsPage");
            crabletSettingsPage.AddElement(new TextDisplay(crabletSettingsPageTranform.FindChild("Title").gameObject, "CRABLET SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPageTranform.FindChild("BaseColorElement").gameObject, "Base Color:", elementTextProperties, colorList, delegate (string s) { AIMenuFunctions.UpdateBaseColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<string>(crabletSettingsPageTranform.FindChild("AgroColorElement").gameObject, "Agro Color:", elementTextProperties, colorList, delegate (string s) { AIMenuFunctions.UpdateAgroColor(s); }));
            crabletSettingsPage.AddElement(new GenericSelector<bool>(crabletSettingsPageTranform.FindChild("JumpAttackEnabledElement").gameObject, "Jump Attack Enabled:", elementTextProperties, boolList, delegate (bool b) { AIMenuFunctions.UpdateJumpAttackEnabled(b); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPageTranform.FindChild("JumpChargeElement").gameObject, "Jump Charge:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCharge(s); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPageTranform.FindChild("JumpCooldownElement").gameObject, "Jump Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpCooldown(s); }));
            crabletSettingsPage.AddElement(new InputField(crabletSettingsPageTranform.FindChild("JumpForceElement").gameObject, "Jump Force:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateJumpForce(s); }));
            crabletSettingsPage.AddElement(new Button(crabletSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage1"); }));

            #endregion

            MelonLogger.Msg("Built Crablet Settings Page");

            #region Configure Combat Settings Page

            Transform combatSettingsPageTransform = menuPrefab.transform.FindChild("CombatSettingsPage");
            combatSettingsPage.AddElement(new TextDisplay(combatSettingsPageTransform.FindChild("Title").gameObject, "COMBAT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            combatSettingsPage.AddElement(new GenericSelector<string>(combatSettingsPageTransform.FindChild("AgroOnNPCTypeElement").gameObject, "Agro On NPC Type:", elementTextProperties, NPCTypes, delegate (string s) { AIMenuFunctions.UpdateAgroOnNPCType(s); }));
            combatSettingsPage.AddElement(new GenericSelector<string>(combatSettingsPageTransform.FindChild("CombatProficiencyElement").gameObject, "Combat Proficiency:", elementTextProperties, combatProficiency, delegate (string s) { AIMenuFunctions.UpdateCombatProficiency(s); }));
            combatSettingsPage.AddElement(new InputField(combatSettingsPageTransform.FindChild("MeleeRangeElement").gameObject, "Melee Range:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateMeleeRange(s); }));
            combatSettingsPage.AddElement(new Button(combatSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));
            
            #endregion

            MelonLogger.Msg("Built Combat Settings Page");

            #region Configure Visual Settings Page

            Transform visualSettingsPageTransform = menuPrefab.transform.FindChild("VisualSettingsPage");
            visualSettingsPage.AddElement(new TextDisplay(visualSettingsPageTransform.FindChild("Title").gameObject, "COMBAT SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            visualSettingsPage.AddElement(new GenericSelector<string>(visualSettingsPageTransform.FindChild("EmissionColorElement").gameObject, "Emission Color:", elementTextProperties, colorList, delegate (string s) { AIMenuFunctions.UpdateEmissionColor(s); }));
            visualSettingsPage.AddElement(new InputField(visualSettingsPageTransform.FindChild("FaceExpressionCooldownElement").gameObject, "Face Expression Cooldown Time:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateFaceExpressionCooldownTime(s); }));
            visualSettingsPage.AddElement(new Button(visualSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion

            MelonLogger.Msg("Built Visual Settings Page");

            #region Configure OmniWheel Settings Page

            Transform omniWheelSettingsPageTransform = menuPrefab.transform.FindChild("OmniWheelSettingsPage");
            omniWheelSettingsPage.AddElement(new TextDisplay(omniWheelSettingsPageTransform.FindChild("Title").gameObject, "OMNIWHEEL SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargeAttackSpeedElement").gameObject, "Charge Attack Speed:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeAttackSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargeCooldownElement").gameObject, "Charge Cooldown:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeCooldown(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargePrepSpeedElement").gameObject, "Charge Prep Speed:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargePrepSpeed(s); }));
            omniWheelSettingsPage.AddElement(new InputField(omniWheelSettingsPageTransform.FindChild("ChargeWindupDistanceElement").gameObject, "Charge Wind-up Distance:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateChargeWindupDistance(s); }));
            omniWheelSettingsPage.AddElement(new GenericSelector<string>(omniWheelSettingsPageTransform.FindChild("DefaultEngagedModeElement").gameObject, "Default Engaged Mode:", elementTextProperties, omniEngagedModes, delegate (string s) { AIMenuFunctions.UpdateDefaultOmniEngagedMode(s); }));
            omniWheelSettingsPage.AddElement(new Button(omniWheelSettingsPageTransform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion

            MelonLogger.Msg("Built OmniWheel Settings Page");

            #region Other Settings Page

            Transform otherSettingsPageTranform = menuPrefab.transform.FindChild("OtherSettingsPage");
            otherSettingsPage.AddElement(new TextDisplay(otherSettingsPageTranform.FindChild("Title").gameObject, "OTHER SETTINGS", new TextProperties(10.5f, Color.white, false, 15)));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("HearingSensitivityElement").gameObject, "Hearing Sensitivity:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateHearingSensitivity(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("VisionRadiusElement").gameObject, "Vision Radius:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdateVisionRadius(s); }));
            otherSettingsPage.AddElement(new InputField(otherSettingsPageTranform.FindChild("PitchMultiplierElement").gameObject, "Pitch Multiplier:", "", elementTextProperties, int.MinValue, int.MaxValue, delegate (string s) { AIMenuFunctions.UpdatePitchMultiplier(s); }));
            otherSettingsPage.AddElement(new Button(otherSettingsPageTranform.FindChild("BackButton").gameObject, "BACK", titleTextProperties, Button.ButtonHighlightType.Underline, delegate { aiMenu.SwitchPage("AdditionalSettingsPage2"); }));

            #endregion

            MelonLogger.Msg("Built Other Settings Page");

            // Add the pages to the menu
            aiMenu = new Menu(menuPrefab, rootPage);
            aiMenu.AddPage(configureAIPage);
            aiMenu.AddPage(additionalSettingsPage1);
            aiMenu.AddPage(additionalSettingsPage2);
            aiMenu.AddPage(healthSettingsPage);
            aiMenu.AddPage(gunSettingsPage);
            aiMenu.AddPage(throwSettingsPage);
            aiMenu.AddPage(movementSettingsPage);
            aiMenu.AddPage(behaviourSettingsPage);
            aiMenu.AddPage(crabletSettingsPage);
            aiMenu.AddPage(combatSettingsPage);
            aiMenu.AddPage(visualSettingsPage);
            aiMenu.AddPage(omniWheelSettingsPage);
            aiMenu.AddPage(otherSettingsPage);
        }
    }
}
