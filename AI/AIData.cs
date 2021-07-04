namespace AIModifier.AI
{
    public class AIData
    {
        public string name { get; set; }
        public string headPlateTransformChildPath { get; set; }
        public float headPlateHeightOffset { get; set; }


        #region Health Settings

        public float health { get; set; }
        public float leftLegHealth { get; set; }
        public float rightLegHealth { get; set; }
        public float leftArmHealth { get; set; }
        public float rightArmHealth { get; set; }

        #endregion

        #region Gun Settings
        public float accuracy { get; set; }
        public float gunRange { get; set; }
        public float gunCooldown { get; set; }
        public float reloadTime { get; set; }
        public float burstSize { get; set; }
        public float clipSize { get; set; }
        #endregion

        #region Throw Settings

        public bool enableThrowAttack { get; set; }
        public float throwCooldown { get; set; }
        public float throwMaxRange { get; set; }
        public float throwMinRange { get; set; }
        public float throwVelocity { get; set; }

        #endregion

        #region Movement Settings

        public float agroedSpeed { get; set; }
        public float roamSpeed { get; set; }
        public float engagedSpeed { get; set; }
        public float roamRange { get; set; }
        public bool roamWanders { get; set; }

        #endregion

        #region Behaviour Settings

        public string defaultMentalState { get; set; }
        public string defaultEngagedMode { get; set; }
        public float mirrorSkill { get; set; }

        #endregion

        #region Crablet Settings

        public string baseColor { get; set;}
        public string agroColor { get; set; }
        public bool jumpAttackEnabled { get; set; }
        public float jumpCharge { get; set; }
        public float jumpCooldown { get; set; }
        public float jumpForce { get; set; }

        #endregion

        #region Combat Settings

        public string agroOnNPCType { get; set; }
        public string combatProficiency { get; set; }
        public float meleeRange { get; set; }

        #endregion

        #region Visual Settings

        public string emissionColor { get; set; }
        public float faceExpressionCooldownTime { get; set; }

        #endregion

        #region OmniWheel Settings

        public float chargeAttackSpeed { get; set; }
        public float chargeCooldown { get; set; }
        public float chargePrepSpeed { get; set; }
        public float chargeWindupDistance { get; set; }
        public string defaultOmniEngagedMode { get; set; }

        #endregion


        #region Other Settings

        public float hearingSensitivity { get; set; }
        public float visionRadius { get; set; }
        public float pitchMultiplier { get; set; }

        #endregion
    }
}
