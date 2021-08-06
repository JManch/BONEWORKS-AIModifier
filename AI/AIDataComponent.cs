using System;
using StressLevelZero.AI;
using UnityEngine;
using MelonLoader;

namespace AIModifier.AI
{
    class AIDataComponent : MonoBehaviour
    {
        public AIDataComponent(IntPtr ptr) : base(ptr) { }

        public AIData defaultAIData { get; private set; }
        public AIData aiData { get; private set; }

        private bool generatedDefaultData = false;

        public void GenerateDefaultAIData()
        {
            if(!generatedDefaultData)
            {
                defaultAIData = AIDataManager.GenerateAIData(gameObject.GetComponent<AIBrain>());

                // Because the AI's BaseConfigs do not modify health, force health to reset to 100
                defaultAIData.health = 100;
                defaultAIData.leftLegHealth = 100;
                defaultAIData.rightLegHealth = 100;
                defaultAIData.leftArmHealth = 100;
                defaultAIData.rightArmHealth = 100;

                defaultAIData.defaultMentalState = "Default";
                defaultAIData.defaultEngagedMode = "Default";
                defaultAIData.defaultOmniEngagedMode = "Default";
                defaultAIData.agroColor = "Default";
                defaultAIData.baseColor = "Default";

                generatedDefaultData = true;
            }
        }

        public void UpdateAIData(AIData newAIData)
        {
            aiData = newAIData;
        }
    }
}
