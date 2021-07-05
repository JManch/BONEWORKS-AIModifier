using AIModifier.UI;
using System.Globalization;
using System.Collections.Generic;
using StressLevelZero.AI;
using MelonLoader;

namespace AIModifier.AI
{
    public static class AIMenuFunctions
    {
        private static string selectedAI;

        public static void LoadAdditionalSettings()
        {
            MenuPage healthSettingsPage = AIMenuManager.aiMenu.GetPage("HealthSettingsPage");
            healthSettingsPage.GetElement("LeftLegHealthElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].leftLegHealth);
            healthSettingsPage.GetElement("RightLegHealthElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].rightLegHealth);
            healthSettingsPage.GetElement("LeftArmHealthElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].leftArmHealth);
            healthSettingsPage.GetElement("RightArmHealthElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].rightArmHealth);

            MenuPage gunSettingsPage = AIMenuManager.aiMenu.GetPage("GunSettingsPage");
            gunSettingsPage.GetElement("AccuracyElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].accuracy);
            gunSettingsPage.GetElement("GunRangeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].gunRange);
            gunSettingsPage.GetElement("GunCooldownElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].gunCooldown);
            gunSettingsPage.GetElement("ReloadTimeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].reloadTime);
            gunSettingsPage.GetElement("BurstSizeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].burstSize);
            gunSettingsPage.GetElement("ClipSizeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].clipSize);

            MenuPage throwSettingsPage = AIMenuManager.aiMenu.GetPage("ThrowSettingsPage");
            throwSettingsPage.GetElement("EnableThrowAttackElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].enableThrowAttack);
            throwSettingsPage.GetElement("ThrowCooldownElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].throwCooldown);
            throwSettingsPage.GetElement("ThrowMaxRangeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].throwMaxRange);
            throwSettingsPage.GetElement("ThrowMinRangeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].throwMinRange);
            throwSettingsPage.GetElement("ThrowVelocityElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].throwVelocity);

            MenuPage movementSettingsPage = AIMenuManager.aiMenu.GetPage("MovementSettingsPage");
            movementSettingsPage.GetElement("AgroSpeedElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].agroedSpeed);
            movementSettingsPage.GetElement("EngagedSpeedElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].engagedSpeed);
            movementSettingsPage.GetElement("RoamSpeedElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].roamSpeed);
            movementSettingsPage.GetElement("RoamRangeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].roamRange);
            movementSettingsPage.GetElement("RoamWandersElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].roamWanders);

            MenuPage behaviourSettingsPage = AIMenuManager.aiMenu.GetPage("BehaviourSettingsPage");
            behaviourSettingsPage.GetElement("DefaultMentalStateElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].defaultMentalState);
            behaviourSettingsPage.GetElement("DefaultEngagedModeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].defaultEngagedMode);
            behaviourSettingsPage.GetElement("MirrorSkillElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].mirrorSkill);

            MenuPage crabletSettingsPage = AIMenuManager.aiMenu.GetPage("CrabletSettingsPage");
            crabletSettingsPage.GetElement("BaseColorElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].baseColor);
            crabletSettingsPage.GetElement("AgroColorElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].agroColor);
            crabletSettingsPage.GetElement("JumpAttackEnabledElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].jumpAttackEnabled);
            crabletSettingsPage.GetElement("JumpChargeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].jumpCharge);
            crabletSettingsPage.GetElement("JumpCooldownElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].jumpCooldown);
            crabletSettingsPage.GetElement("JumpForceElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].jumpForce);

            MenuPage combatSettingsPage = AIMenuManager.aiMenu.GetPage("CombatSettingsPage");
            combatSettingsPage.GetElement("AgroOnNPCTypeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].agroOnNPCType);
            combatSettingsPage.GetElement("CombatProficiencyElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].combatProficiency);
            combatSettingsPage.GetElement("MeleeRangeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].meleeRange);

            MenuPage visualSettingsPage = AIMenuManager.aiMenu.GetPage("VisualSettingsPage");
            visualSettingsPage.GetElement("EmissionColorElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].emissionColor);
            visualSettingsPage.GetElement("FaceExpressionCooldownElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].faceExpressionCooldownTime);

            MenuPage omniWheelSettingsPage = AIMenuManager.aiMenu.GetPage("OmniWheelSettingsPage");
            omniWheelSettingsPage.GetElement("ChargeAttackSpeedElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].chargeAttackSpeed);
            omniWheelSettingsPage.GetElement("ChargeCooldownElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].chargeCooldown);
            omniWheelSettingsPage.GetElement("ChargePrepSpeedElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].chargePrepSpeed);
            omniWheelSettingsPage.GetElement("ChargeWindupDistanceElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].chargeWindupDistance);
            omniWheelSettingsPage.GetElement("DefaultEngagedModeElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].defaultOmniEngagedMode);

            MenuPage otherSettingsPage = AIMenuManager.aiMenu.GetPage("OtherSettingsPage");
            otherSettingsPage.GetElement("HearingSensitivityElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].hearingSensitivity);
            otherSettingsPage.GetElement("VisionRadiusElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].visionRadius);
            otherSettingsPage.GetElement("PitchMultiplierElement").SetValue(AIDataManager.aiDataDictionary[selectedAI].pitchMultiplier);
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
                AIDataManager.aiDataDictionary[aiType].health = result;
            }
        }

        public static void UpdateAIHealthUI(string aiType)
        {
            float health = AIDataManager.aiDataDictionary[aiType].health;
            AIMenuManager.aiMenu.GetPage("ConfigureAIPage").GetElement("HealthElement").SetValue(health);
        }

        #endregion

        #region Health Settings Page

        public static void UpdateLeftLegHealth(string leftLegHealth)
        {
            if (float.TryParse(leftLegHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].leftLegHealth = result;
            }
        }
        public static void UpdateRightLegHealth(string rightLegHealth)
        {
            if (float.TryParse(rightLegHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].rightLegHealth = result;
            }
        }
        public static void UpdateLeftArmHealth(string leftArmHealth)
        {
            if (float.TryParse(leftArmHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].leftArmHealth = result;
            }
        }
        public static void UpdateRightArmHealth(string rightArmHealth)
        {
            if (float.TryParse(rightArmHealth, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].rightArmHealth = result;
            }
        }

        #endregion

        #region Gun Settings Page
        public static void UpdateAIAccuracy(string accuracy)
        {
            if (float.TryParse(accuracy, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].accuracy = result;
            }
        }
        public static void UpdateGunRange(string gunRange)
        {
            if (float.TryParse(gunRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].gunRange = result;
            }
        }
        public static void UpdateGunCooldown(string gunCooldown)
        {
            if (float.TryParse(gunCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].gunCooldown = result;
            }
        }
        public static void UpdateReloadTime(string reloadTime)
        {
            if (float.TryParse(reloadTime, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].reloadTime = result;
            }
        }
        public static void UpdateBurstSize(string burstSize)
        {
            if (float.TryParse(burstSize, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].burstSize = result;
            }
        }
        public static void UpdateClipSize(string clipSize)
        {
            if (float.TryParse(clipSize, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].clipSize = result;
            }
        }

        #endregion

        #region Throw Settings Page

        public static void UpdateEnableThrowAttack(bool enableThrowAttack)
        {
            AIDataManager.aiDataDictionary[selectedAI].enableThrowAttack = enableThrowAttack;
        }

        public static void UpdateThrowCooldown(string throwCooldown)
        {
            if (float.TryParse(throwCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].throwCooldown = result;
            }
        }

        public static void UpdateThrowMaxRange(string throwMaxRange)
        {
            if (float.TryParse(throwMaxRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].throwMaxRange = result;
            }
        }
        public static void UpdateThrowMinRange(string throwMinRange)
        {
            if (float.TryParse(throwMinRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].throwMinRange = result;
            }
        }
        public static void UpdateThrowVelocity(string throwVelocity)
        {
            if (float.TryParse(throwVelocity, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].throwVelocity = result;
            }
        }

        #endregion

        #region Movement Settings Page

        public static void UpdateAgroSpeed(string agroedSpeed)
        {
            if (float.TryParse(agroedSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].agroedSpeed = result;
            }
        }
        public static void UpdateRoamSpeed(string roamSpeed)
        {
            if (float.TryParse(roamSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].roamSpeed = result;
            }
        }
        public static void UpdateEngagedSpeed(string engagedSpeed)
        {
            if (float.TryParse(engagedSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].engagedSpeed = result;
            }
        }
        public static void UpdateRoamRange(string roamRange)
        {
            if (float.TryParse(roamRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].roamRange = result;
            }
        }
        public static void UpdateRoamWanders(bool roamWanders)
        {
            AIDataManager.aiDataDictionary[selectedAI].roamWanders = roamWanders;
        }


        #endregion

        #region Behaviour Settings Page

        public static void UpdateDefaultMentalState(string defaultMentalState)
        {
            AIDataManager.aiDataDictionary[selectedAI].defaultMentalState = defaultMentalState;
        }
        public static void UpdateDefaultEngagedMode(string defaultEngagedMode)
        {
            AIDataManager.aiDataDictionary[selectedAI].defaultEngagedMode = defaultEngagedMode;
        }
        public static void UpdateMirrorSkill(string mirrorSkill)
        {
            if (float.TryParse(mirrorSkill, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].mirrorSkill = result;
            }
        }

        #endregion

        #region Crablet Settings Page

        public static void UpdateBaseColor(string baseColor)
        {
            AIDataManager.aiDataDictionary[selectedAI].baseColor = baseColor;
        }
        public static void UpdateAgroColor(string agroColor)
        {
            AIDataManager.aiDataDictionary[selectedAI].agroColor = agroColor;
        }
        public static void UpdateJumpAttackEnabled(bool jumpAttackEnabled)
        {
            AIDataManager.aiDataDictionary[selectedAI].jumpAttackEnabled = jumpAttackEnabled;
        }
        public static void UpdateJumpCharge(string jumpCharge)
        {
            if (float.TryParse(jumpCharge, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].jumpCharge = result;
            }
        }
        public static void UpdateJumpCooldown(string jumpCooldown)
        {
            if (float.TryParse(jumpCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].jumpCooldown = result;
            }
        }
        public static void UpdateJumpForce(string jumpForce)
        {
            if (float.TryParse(jumpForce, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].jumpForce = result;
            }
        }

        #endregion

        #region Combat Settings Page

        public static void UpdateAgroOnNPCType(string agroOnNPCType)
        {
            AIDataManager.aiDataDictionary[selectedAI].agroOnNPCType = agroOnNPCType;
        }
        public static void UpdateCombatProficiency(string combatProficiency)
        {
            AIDataManager.aiDataDictionary[selectedAI].combatProficiency = combatProficiency;
        }
        public static void UpdateMeleeRange(string meleeRange)
        {
            if (float.TryParse(meleeRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].meleeRange = result;
            }
        }

        #endregion

        #region Visual Settings Page

        public static void UpdateEmissionColor(string emissionColor)
        {
            AIDataManager.aiDataDictionary[selectedAI].emissionColor = emissionColor;
        }
        public static void UpdateFaceExpressionCooldownTime(string faceExpressionCooldownTime)
        {
            if (float.TryParse(faceExpressionCooldownTime, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].faceExpressionCooldownTime = result;
            }
        }

        #endregion

        #region OmniWheel Settings Page

        public static void UpdateChargeAttackSpeed(string chargeAttackSpeed)
        {
            if (float.TryParse(chargeAttackSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].chargeAttackSpeed = result;
            }
        }
        public static void UpdateChargeCooldown(string chargeCooldown)
        {
            if (float.TryParse(chargeCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].chargeCooldown = result;
            }
        }
        public static void UpdateChargePrepSpeed(string chargePrepSpeed)
        {
            if (float.TryParse(chargePrepSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].chargePrepSpeed = result;
            }
        }
        public static void UpdateChargeWindupDistance(string chargeWindupDistance)
        {
            if (float.TryParse(chargeWindupDistance, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].chargeWindupDistance = result;
            }
        }
        public static void UpdateDefaultOmniEngagedMode(string defaultOmniEngagedMode)
        {
            AIDataManager.aiDataDictionary[selectedAI].defaultOmniEngagedMode = defaultOmniEngagedMode;
        }

        #endregion

        #region Other Settings Page

        public static void UpdateHearingSensitivity(string hearingSensitivity)
        {
            if (float.TryParse(hearingSensitivity, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].hearingSensitivity = result;
            }
        }
        public static void UpdateVisionRadius(string visionRadius)
        {
            if (float.TryParse(visionRadius, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].visionRadius = result;
            }
        }
        public static void UpdatePitchMultiplier(string pitchMultiplier)
        {
            if (float.TryParse(pitchMultiplier, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].pitchMultiplier = result;
            }
        }

        #endregion

        public static void FreezeSelectedAI()
        {
            foreach(AIBrain aiBrain in new List<AIBrain>(AIManager.selectedAI.Values))
            {
                if(aiBrain != null)
                {
                    aiBrain.behaviour.Freeze();
                }
            }
        }

        public static void UnfreezeSelectedAI()
        {

        }
    }
}
