﻿using AIModifier.UI;
using System.Globalization;
using System;
using System.Collections.Generic;
using StressLevelZero.AI;
using PuppetMasta;
using UnityEngine;

namespace AIModifier.AI
{
    public static class AIMenuFunctions
    {
        public static string selectedAI { get; private set; }

        public static void LoadAIDataIntoUI()
        {
            MenuPage healthSettingsPage = AIMenuManager.aiMenu.GetPage("HealthSettingsPage");
            healthSettingsPage.GetElement("LeftLegHealthElement").SetValue(AIDataManager.aiData[selectedAI].leftLegHealth);
            healthSettingsPage.GetElement("RightLegHealthElement").SetValue(AIDataManager.aiData[selectedAI].rightLegHealth);
            healthSettingsPage.GetElement("LeftArmHealthElement").SetValue(AIDataManager.aiData[selectedAI].leftArmHealth);
            healthSettingsPage.GetElement("RightArmHealthElement").SetValue(AIDataManager.aiData[selectedAI].rightArmHealth);

            MenuPage gunSettingsPage = AIMenuManager.aiMenu.GetPage("GunSettingsPage");
            gunSettingsPage.GetElement("AccuracyElement").SetValue(AIDataManager.aiData[selectedAI].accuracy);
            gunSettingsPage.GetElement("GunRangeElement").SetValue(AIDataManager.aiData[selectedAI].gunRange);
            gunSettingsPage.GetElement("ReloadTimeElement").SetValue(AIDataManager.aiData[selectedAI].reloadTime);
            gunSettingsPage.GetElement("BurstSizeElement").SetValue(AIDataManager.aiData[selectedAI].burstSize);
            gunSettingsPage.GetElement("ClipSizeElement").SetValue(AIDataManager.aiData[selectedAI].clipSize);

            MenuPage throwSettingsPage = AIMenuManager.aiMenu.GetPage("ThrowSettingsPage");
            throwSettingsPage.GetElement("EnableThrowAttackElement").SetValue(AIDataManager.aiData[selectedAI].enableThrowAttack);
            throwSettingsPage.GetElement("ThrowCooldownElement").SetValue(AIDataManager.aiData[selectedAI].throwCooldown);
            throwSettingsPage.GetElement("ThrowMaxRangeElement").SetValue(AIDataManager.aiData[selectedAI].throwMaxRange);
            throwSettingsPage.GetElement("ThrowMinRangeElement").SetValue(AIDataManager.aiData[selectedAI].throwMinRange);

            MenuPage movementSettingsPage = AIMenuManager.aiMenu.GetPage("MovementSettingsPage");
            movementSettingsPage.GetElement("AgroSpeedElement").SetValue(AIDataManager.aiData[selectedAI].agroedSpeed);
            movementSettingsPage.GetElement("EngagedSpeedElement").SetValue(AIDataManager.aiData[selectedAI].engagedSpeed);
            movementSettingsPage.GetElement("RoamSpeedElement").SetValue(AIDataManager.aiData[selectedAI].roamSpeed);
            movementSettingsPage.GetElement("RoamRangeElement").SetValue(AIDataManager.aiData[selectedAI].roamRange);
            movementSettingsPage.GetElement("RoamWandersElement").SetValue(AIDataManager.aiData[selectedAI].roamWanders);

            MenuPage behaviourSettingsPage = AIMenuManager.aiMenu.GetPage("BehaviourSettingsPage");
            behaviourSettingsPage.GetElement("DefaultMentalStateElement").SetValue(AIDataManager.aiData[selectedAI].defaultMentalState);
            behaviourSettingsPage.GetElement("DefaultEngagedModeElement").SetValue(AIDataManager.aiData[selectedAI].defaultEngagedMode);

            MenuPage crabletSettingsPage = AIMenuManager.aiMenu.GetPage("CrabletSettingsPage");
            crabletSettingsPage.GetElement("BaseColorElement").SetValue(AIDataManager.aiData[selectedAI].baseColor);
            crabletSettingsPage.GetElement("AgroColorElement").SetValue(AIDataManager.aiData[selectedAI].agroColor);
            crabletSettingsPage.GetElement("JumpAttackEnabledElement").SetValue(AIDataManager.aiData[selectedAI].jumpAttackEnabled);
            crabletSettingsPage.GetElement("JumpCooldownElement").SetValue(AIDataManager.aiData[selectedAI].jumpCooldown);

            MenuPage combatSettingsPage = AIMenuManager.aiMenu.GetPage("CombatSettingsPage");
            combatSettingsPage.GetElement("AgroOnNPCTypeElement").SetValue(AIDataManager.aiData[selectedAI].agroOnNPCType.Replace(" ", "").Split(','));
            combatSettingsPage.GetElement("MeleeRangeElement").SetValue(AIDataManager.aiData[selectedAI].meleeRange);

            MenuPage omniWheelSettingsPage = AIMenuManager.aiMenu.GetPage("OmniWheelSettingsPage");
            omniWheelSettingsPage.GetElement("ChargeAttackSpeedElement").SetValue(AIDataManager.aiData[selectedAI].chargeAttackSpeed);
            omniWheelSettingsPage.GetElement("ChargeCooldownElement").SetValue(AIDataManager.aiData[selectedAI].chargeCooldown);
            omniWheelSettingsPage.GetElement("ChargePrepSpeedElement").SetValue(AIDataManager.aiData[selectedAI].chargePrepSpeed);
            omniWheelSettingsPage.GetElement("ChargeWindupDistanceElement").SetValue(AIDataManager.aiData[selectedAI].chargeWindupDistance);
            omniWheelSettingsPage.GetElement("DefaultEngagedModeElement").SetValue(AIDataManager.aiData[selectedAI].defaultOmniEngagedMode);

            MenuPage otherSettingsPage = AIMenuManager.aiMenu.GetPage("OtherSettingsPage");
            otherSettingsPage.GetElement("HearingSensitivityElement").SetValue(AIDataManager.aiData[selectedAI].hearingSensitivity);
            otherSettingsPage.GetElement("VisionRadiusElement").SetValue(AIDataManager.aiData[selectedAI].visionRadius);
            otherSettingsPage.GetElement("PitchMultiplierElement").SetValue(AIDataManager.aiData[selectedAI].pitchMultiplier);
        }

        #region Configure AI Page
        public static void OnSelectedAIChanged(string selectedAI)
        {
            AIMenuFunctions.selectedAI = selectedAI;
            UpdateAIHealthUI(selectedAI);
        }

        public static void UpdateAIHealth(string health)
        {
            string aiType = (string)AIMenuManager.aiMenu.GetPage("ConfigureAIPage").GetElement("SelectedAIElement").GetValue();

            if(float.TryParse(health, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[aiType].health = result;
            }
        }

        public static void UpdateAIHealthUI(string aiType)
        {
            float health = AIDataManager.aiData[aiType].health;
            AIMenuManager.aiMenu.GetPage("ConfigureAIPage").GetElement("HealthElement").SetValue(health);
        }

        #endregion

        #region Control AI Page

        public static void ToggleControlAISelector(string state)
        {
            AIMenuManager.aiSelectorType = AISelectedPlateController.SelectedType.Standard;

            if(state == "Active" && !AIMenuManager.aiSelectorPointer.pointerEnabled)
            {
                AIMenuManager.aiSelectorPointer.EnablePointer();
            }
            else if(state == "Inactive" && AIMenuManager.aiSelectorPointer.pointerEnabled)
            {
                AIMenuManager.aiSelectorPointer.DisablePointer();
            }
        }

        #endregion

        #region Agro Targets Page

        public static void ToggleAgroTargetsSelector(string state)
        {
            AIMenuManager.aiSelectorType = AISelectedPlateController.SelectedType.Target;

            if (state == "Active" && !AIMenuManager.aiSelectorPointer.pointerEnabled)
            {
                AIMenuManager.aiSelectorPointer.EnablePointer();
            }
            else if (state == "Inactive" && AIMenuManager.aiSelectorPointer.pointerEnabled)
            {
                AIMenuManager.aiSelectorPointer.DisablePointer();
            }
        }

        #endregion

        #region Health Settings Page

        public static void UpdateLeftLegHealth(string leftLegHealth)
        {
            if (float.TryParse(leftLegHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].leftLegHealth = result;
            }
        }
        public static void UpdateRightLegHealth(string rightLegHealth)
        {
            if (float.TryParse(rightLegHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].rightLegHealth = result;
            }
        }
        public static void UpdateLeftArmHealth(string leftArmHealth)
        {
            if (float.TryParse(leftArmHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].leftArmHealth = result;
            }
        }
        public static void UpdateRightArmHealth(string rightArmHealth)
        {
            if (float.TryParse(rightArmHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].rightArmHealth = result;
            }
        }
        public static void RestoreHealthSettings()
        {
            AIDataManager.aiData[selectedAI].leftLegHealth = AIDataManager.defaultAIConfigurations[selectedAI].leftLegHealth;
            AIDataManager.aiData[selectedAI].rightLegHealth = AIDataManager.defaultAIConfigurations[selectedAI].rightLegHealth;
            AIDataManager.aiData[selectedAI].leftArmHealth = AIDataManager.defaultAIConfigurations[selectedAI].leftArmHealth;
            AIDataManager.aiData[selectedAI].rightArmHealth = AIDataManager.defaultAIConfigurations[selectedAI].rightArmHealth;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Gun Settings Page
        public static void UpdateAIAccuracy(string accuracy)
        {
            if (float.TryParse(accuracy, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].accuracy = result;
            }
        }
        public static void UpdateGunRange(string gunRange)
        {
            if (float.TryParse(gunRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].gunRange = result;
            }
        }
        public static void UpdateReloadTime(string reloadTime)
        {
            if (float.TryParse(reloadTime, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].reloadTime = result;
            }
        }
        public static void UpdateBurstSize(string burstSize)
        {
            if (float.TryParse(burstSize, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].burstSize = result;
            }
        }
        public static void UpdateClipSize(string clipSize)
        {
            if (float.TryParse(clipSize, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].clipSize = result;
            }
        }
        public static void RestoreGunSettings()
        {
            AIDataManager.aiData[selectedAI].accuracy = AIDataManager.defaultAIConfigurations[selectedAI].accuracy;
            AIDataManager.aiData[selectedAI].gunRange = AIDataManager.defaultAIConfigurations[selectedAI].gunRange;
            AIDataManager.aiData[selectedAI].burstSize = AIDataManager.defaultAIConfigurations[selectedAI].burstSize;
            AIDataManager.aiData[selectedAI].clipSize = AIDataManager.defaultAIConfigurations[selectedAI].clipSize;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Throw Settings Page

        public static void UpdateEnableThrowAttack(bool enableThrowAttack)
        {
            AIDataManager.aiData[selectedAI].enableThrowAttack = enableThrowAttack;
        }

        public static void UpdateThrowCooldown(string throwCooldown)
        {
            if (float.TryParse(throwCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].throwCooldown = result;
            }
        }

        public static void UpdateThrowMaxRange(string throwMaxRange)
        {
            if (float.TryParse(throwMaxRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].throwMaxRange = result;
            }
        }
        public static void UpdateThrowMinRange(string throwMinRange)
        {
            if (float.TryParse(throwMinRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].throwMinRange = result;
            }
        }
        public static void RestoreThrowSettings()
        {
            AIDataManager.aiData[selectedAI].enableThrowAttack = AIDataManager.defaultAIConfigurations[selectedAI].enableThrowAttack;
            AIDataManager.aiData[selectedAI].throwCooldown = AIDataManager.defaultAIConfigurations[selectedAI].throwCooldown;
            AIDataManager.aiData[selectedAI].throwMaxRange = AIDataManager.defaultAIConfigurations[selectedAI].throwMaxRange;
            AIDataManager.aiData[selectedAI].throwMinRange = AIDataManager.defaultAIConfigurations[selectedAI].throwMinRange;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Movement Settings Page
        public static void UpdateAgroSpeed(string agroedSpeed)
        {
            if (float.TryParse(agroedSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].agroedSpeed = result;
            }
        }
        public static void UpdateRoamSpeed(string roamSpeed)
        {
            if (float.TryParse(roamSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].roamSpeed = result;
            }
        }
        public static void UpdateEngagedSpeed(string engagedSpeed)
        {
            if (float.TryParse(engagedSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].engagedSpeed = result;
            }
        }
        public static void UpdateRoamRange(string roamRange)
        {
            if (float.TryParse(roamRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].roamRange = result;
            }
        }
        public static void UpdateRoamWanders(bool roamWanders)
        {
            AIDataManager.aiData[selectedAI].roamWanders = roamWanders;
        }
        public static void RestoreMovementSettings()
        {
            AIDataManager.aiData[selectedAI].agroedSpeed = AIDataManager.defaultAIConfigurations[selectedAI].agroedSpeed;
            AIDataManager.aiData[selectedAI].roamSpeed = AIDataManager.defaultAIConfigurations[selectedAI].roamSpeed;
            AIDataManager.aiData[selectedAI].engagedSpeed = AIDataManager.defaultAIConfigurations[selectedAI].engagedSpeed;
            AIDataManager.aiData[selectedAI].roamRange = AIDataManager.defaultAIConfigurations[selectedAI].roamRange;
            AIDataManager.aiData[selectedAI].roamWanders = AIDataManager.defaultAIConfigurations[selectedAI].roamWanders;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Behaviour Settings Page

        public static void UpdateDefaultMentalState(string defaultMentalState)
        {
            AIDataManager.aiData[selectedAI].defaultMentalState = defaultMentalState;
        }
        public static void UpdateDefaultEngagedMode(string defaultEngagedMode)
        {
            AIDataManager.aiData[selectedAI].defaultEngagedMode = defaultEngagedMode;
        }
        public static void RestoreBehaviourSettings()
        {
            AIDataManager.aiData[selectedAI].defaultMentalState = AIDataManager.defaultAIConfigurations[selectedAI].defaultMentalState;
            AIDataManager.aiData[selectedAI].defaultEngagedMode = AIDataManager.defaultAIConfigurations[selectedAI].defaultEngagedMode;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Crablet Settings Page

        public static void UpdateBaseColor(string baseColor)
        {
            AIDataManager.aiData[selectedAI].baseColor = baseColor;
        }
        public static void UpdateAgroColor(string agroColor)
        {
            AIDataManager.aiData[selectedAI].agroColor = agroColor;
        }
        public static void UpdateJumpAttackEnabled(bool jumpAttackEnabled)
        {
            AIDataManager.aiData[selectedAI].jumpAttackEnabled = jumpAttackEnabled;
        }
        public static void UpdateJumpCooldown(string jumpCooldown)
        {
            if (float.TryParse(jumpCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].jumpCooldown = result;
            }
        }
        public static void RestoreCrabletSettings()
        {
            AIDataManager.aiData[selectedAI].baseColor = AIDataManager.defaultAIConfigurations[selectedAI].baseColor;
            AIDataManager.aiData[selectedAI].agroColor = AIDataManager.defaultAIConfigurations[selectedAI].agroColor;
            AIDataManager.aiData[selectedAI].jumpAttackEnabled = AIDataManager.defaultAIConfigurations[selectedAI].jumpAttackEnabled;
            AIDataManager.aiData[selectedAI].jumpCooldown = AIDataManager.defaultAIConfigurations[selectedAI].jumpCooldown;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Combat Settings Page

        public static void UpdateAgroOnNPCType(string agroOnNPCType)
        {
            AIDataManager.aiData[selectedAI].agroOnNPCType = agroOnNPCType;
        }
        public static void UpdateMeleeRange(string meleeRange)
        {
            if (float.TryParse(meleeRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].meleeRange = result;
            }
        }
        public static void RestoreCombatSettings()
        {
            AIDataManager.aiData[selectedAI].agroOnNPCType = AIDataManager.defaultAIConfigurations[selectedAI].agroOnNPCType;
            AIDataManager.aiData[selectedAI].meleeRange = AIDataManager.defaultAIConfigurations[selectedAI].meleeRange;
            LoadAIDataIntoUI();
        }

        #endregion

        #region OmniWheel Settings Page

        public static void UpdateChargeAttackSpeed(string chargeAttackSpeed)
        {
            if (float.TryParse(chargeAttackSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].chargeAttackSpeed = result;
            }
        }
        public static void UpdateChargeCooldown(string chargeCooldown)
        {
            if (float.TryParse(chargeCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].chargeCooldown = result;
            }
        }
        public static void UpdateChargePrepSpeed(string chargePrepSpeed)
        {
            if (float.TryParse(chargePrepSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].chargePrepSpeed = result;
            }
        }
        public static void UpdateChargeWindupDistance(string chargeWindupDistance)
        {
            if (float.TryParse(chargeWindupDistance, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].chargeWindupDistance = result;
            }
        }
        public static void UpdateDefaultOmniEngagedMode(string defaultOmniEngagedMode)
        {
            AIDataManager.aiData[selectedAI].defaultOmniEngagedMode = defaultOmniEngagedMode;
        }
        public static void RestoreOmniWheelSettings()
        {
            AIDataManager.aiData[selectedAI].chargeAttackSpeed = AIDataManager.defaultAIConfigurations[selectedAI].chargeAttackSpeed;
            AIDataManager.aiData[selectedAI].chargeCooldown = AIDataManager.defaultAIConfigurations[selectedAI].chargeCooldown;
            AIDataManager.aiData[selectedAI].chargePrepSpeed = AIDataManager.defaultAIConfigurations[selectedAI].chargePrepSpeed;
            AIDataManager.aiData[selectedAI].chargeWindupDistance = AIDataManager.defaultAIConfigurations[selectedAI].chargeWindupDistance;
            AIDataManager.aiData[selectedAI].defaultOmniEngagedMode = AIDataManager.defaultAIConfigurations[selectedAI].defaultOmniEngagedMode;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Other Settings Page

        public static void UpdateHearingSensitivity(string hearingSensitivity)
        {
            if (float.TryParse(hearingSensitivity, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].hearingSensitivity = result;
            }
        }
        public static void UpdateVisionRadius(string visionRadius)
        {
            if (float.TryParse(visionRadius, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].visionRadius = result;
            }
        }
        public static void UpdatePitchMultiplier(string pitchMultiplier)
        {
            if (float.TryParse(pitchMultiplier, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiData[selectedAI].pitchMultiplier = result;
            }
        }
        public static void RestoreOtherSettings()
        {
            AIDataManager.aiData[selectedAI].hearingSensitivity = AIDataManager.defaultAIConfigurations[selectedAI].hearingSensitivity;
            AIDataManager.aiData[selectedAI].visionRadius = AIDataManager.defaultAIConfigurations[selectedAI].visionRadius;
            AIDataManager.aiData[selectedAI].pitchMultiplier = AIDataManager.defaultAIConfigurations[selectedAI].pitchMultiplier;
            LoadAIDataIntoUI();
        }

        #endregion

        #region Control AI functions

        public static void SwitchMentalState(string mentalState)
        {
            foreach(AIBrain aiBrain in AIManager.GetSelectedAI())
            {
                if(aiBrain != null)
                {
                    aiBrain.behaviour.SwitchMentalState((BehaviourBaseNav.MentalState)Enum.Parse(typeof(BehaviourBaseNav.MentalState), mentalState));
                }
            }
        }

        public static void AgroTargets()
        {
            List<AIBrain> selectedAI = AIManager.GetSelectedAI();

            // Fill this list
            List<AIBrain> targetAI = AIManager.GetSelectedTargetAI();
            
            System.Random rnd = new System.Random();

            for (int i = 0; i < selectedAI.Count; i++)
            {
                // If an AI has been assigned to attack every other AI then randomnly assign the rest
                if(i > targetAI.Count - 1 && targetAI.Count != 0)
                {
                    selectedAI[i].behaviour.SetAgro(targetAI[rnd.Next(0, targetAI.Count)].gameObject.GetComponent<Arena_EnemyReference>().triggerRefProxy);
                }
                else if(targetAI.Count != 0)
                {
                    selectedAI[i].behaviour.SetAgro(targetAI[i].gameObject.GetComponent<Arena_EnemyReference>().triggerRefProxy);
                }
            }

            for (int i = 0; i < targetAI.Count; i++)
            {
                // If an AI has been assigned to attack every other AI then randomnly assign the rest
                if (i > selectedAI.Count - 1 && selectedAI.Count != 0)
                {
                    targetAI[i].behaviour.SetAgro(selectedAI[rnd.Next(0, selectedAI.Count)].gameObject.GetComponent<Arena_EnemyReference>().triggerRefProxy);
                }
                else if (selectedAI.Count != 0)
                {
                    targetAI[i].behaviour.SetAgro(selectedAI[i].gameObject.GetComponent<Arena_EnemyReference>().triggerRefProxy);
                }
            }
        }

        public static void WalkToPoint(Vector3 point)
        {
            foreach (AIBrain aiBrain in AIManager.GetSelectedAI())
            {
                if (aiBrain != null)
                {
                    aiBrain.behaviour.SetPath(point);
                }
            }
        }

        #endregion
    }
}
