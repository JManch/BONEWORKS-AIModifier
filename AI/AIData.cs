namespace AIModifier.AI
{
    public class AIData
    {
        public string name { get; set; }
        public string headPlateTransformChildPath { get; set; }
        public float headPlateHeightOffset { get; set; }
        public float health { get; set; }

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
        public float agroSpeed { get; set; }
        public float roamSpeed { get; set; }
        public bool roamByDefault { get; set; }
        #endregion

        #region Pathing Settings

        public bool roamWanders { get; set; }
        public float breakAgroHomeDistance { get; set; }
        public float breakAgroTargetDistance { get; set; }
        public float investigateRange { get; set; }
        public float investigationCooldown { get; set; }
        public float restingRange { get; set; }

        #endregion

        #region Crablet Settings

        public string baseColor { get; set;}
        public string agroColor { get; set; }
        public bool jumpAttackEnabled { get; set; }
        public float jumpCharge { get; set; }
        public float jumpCooldown { get; set; }

        #endregion

        #region Other Settings

        public string emissionColor { get; set; }
        public float aiTickFrequency { get; set; }
        public float hearingSensitivity { get; set; }
        public float visionFOV { get; set; }
        public float pitchMultiplier { get; set; }
        public float hitScaleFactor { get; set; }

        #endregion
    }
}
