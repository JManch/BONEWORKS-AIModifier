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
        public float reloadTime { get; set; }
        public float burstSize { get; set; }
        public float clipSize { get; set; }
        #endregion

        #region Throw Settings

        public bool enableThrowAttack { get; set; }
        public float throwCooldown { get; set; }
        public float throwMaxRange { get; set; }
        public float throwMinRange { get; set; }

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

        #endregion

        #region Crablet Settings

        public string baseColor { get; set;}
        public string agroColor { get; set; }
        public bool jumpAttackEnabled { get; set; }
        public float jumpCooldown { get; set; }

        #endregion

        #region Combat Settings

        public string agroOnNPCType { get; set; }
        public float meleeRange { get; set; }

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

        public AIData createClone()
        {
            AIData aiData = new AIData();
            aiData.name = name;
            aiData.headPlateTransformChildPath = headPlateTransformChildPath;
            aiData.headPlateHeightOffset = headPlateHeightOffset;
            aiData.health = health;
            aiData.leftLegHealth = leftLegHealth;
            aiData.rightLegHealth = rightLegHealth;
            aiData.leftArmHealth = leftArmHealth;
            aiData.rightArmHealth = rightArmHealth;
            aiData.accuracy = accuracy;
            aiData.gunRange = gunRange;
            aiData.reloadTime = reloadTime;
            aiData.burstSize = burstSize;
            aiData.clipSize = clipSize;
            aiData.enableThrowAttack = enableThrowAttack;
            aiData.throwCooldown = throwCooldown;
            aiData.throwMaxRange = throwMaxRange;
            aiData.throwMinRange = throwMinRange;
            aiData.agroedSpeed = agroedSpeed;
            aiData.roamSpeed = roamSpeed;
            aiData.engagedSpeed = engagedSpeed;
            aiData.roamRange = roamRange;
            aiData.roamWanders = roamWanders;
            aiData.defaultMentalState = defaultMentalState;
            aiData.defaultEngagedMode = defaultEngagedMode;
            aiData.baseColor = baseColor;
            aiData.agroColor = agroColor;
            aiData.jumpAttackEnabled = jumpAttackEnabled;
            aiData.jumpCooldown = jumpCooldown;
            aiData.agroOnNPCType = agroOnNPCType;
            aiData.meleeRange = meleeRange;
            aiData.baseColor = baseColor;
            aiData.chargeAttackSpeed = chargeAttackSpeed;
            aiData.chargeCooldown = chargeCooldown;
            aiData.chargePrepSpeed = chargePrepSpeed;
            aiData.chargeWindupDistance = chargeWindupDistance;
            aiData.defaultOmniEngagedMode = defaultOmniEngagedMode;
            aiData.hearingSensitivity = hearingSensitivity;
            aiData.visionRadius = visionRadius;
            aiData.pitchMultiplier = pitchMultiplier;

            return aiData;
        }
    }
}
