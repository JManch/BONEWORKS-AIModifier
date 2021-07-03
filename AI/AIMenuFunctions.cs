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

        public static void UpdateAgroSpeed(string agroSpeed)
        {
            if (float.TryParse(agroSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].agroSpeed = result;
            }
        }
        public static void UpdateRoamSpeed(string roamSpeed)
        {
            if (float.TryParse(roamSpeed, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].roamSpeed = result;
            }
        }
        public static void UpdateRoamByDefault(bool roamByDefault)
        {
            AIDataManager.aiDataDictionary[selectedAI].roamByDefault = roamByDefault;
        }

        #endregion

        #region Pathing Settings Page
        
        public static void UpdateRoamWanders(bool roamWanders)
        {
            AIDataManager.aiDataDictionary[selectedAI].roamWanders = roamWanders;
        }
        public static void UpdateBreakAgroHomeDistance(string breakAgroHomeDistance)
        {
            if (float.TryParse(breakAgroHomeDistance, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].breakAgroHomeDistance = result;
            }
        }
        public static void UpdateBreakAgroTargetDistance(string breakAgroTargetDistance)
        {
            if (float.TryParse(breakAgroTargetDistance, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].breakAgroHomeDistance = result;
            }
        }
        public static void UpdateInvestigateRange(string investigateRange)
        {
            if (float.TryParse(investigateRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].investigateRange = result;
            }
        }
        public static void UpdateInvestigationCooldown(string investigationCooldown)
        {
            if (float.TryParse(investigationCooldown, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].investigationCooldown = result;
            }
        }
        public static void UpdateRestingRange(string restingRange)
        {
            if (float.TryParse(restingRange, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].restingRange = result;
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

        #endregion

        #region Other Settings Page

        public static void UpdateEmissionColor(string emissionColor)
        {
            AIDataManager.aiDataDictionary[selectedAI].emissionColor = emissionColor;
        }

        public static void UpdateAITickFrequency(string aiTickFrequency)
        {
            if (float.TryParse(aiTickFrequency, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].aiTickFrequency = result;
            }
        }

        public static void UpdateHearingSensitivity(string hearingSensitivity)
        {
            if (float.TryParse(hearingSensitivity, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].hearingSensitivity = result;
            }
        }
        public static void UpdateVisionFOV(string visionFOV)
        {
            if (float.TryParse(visionFOV, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].visionFOV = result;
            }
        }
        public static void UpdatePitchMultiplier(string pitchMultiplier)
        {
            if (float.TryParse(pitchMultiplier, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].pitchMultiplier = result;
            }
        }

        public static void UpdateHitScaleFactor(string hitScaleFactor)
        {
            if (float.TryParse(hitScaleFactor, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                AIDataManager.aiDataDictionary[selectedAI].hitScaleFactor = result;
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
