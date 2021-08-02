using System;
using StressLevelZero.AI;
using UnityEngine;

namespace AIModifier.AI
{
    class AIDataComponent : MonoBehaviour
    {
        public AIDataComponent(IntPtr ptr) : base(ptr) { }

        public AIData defaultAIData { get; private set; }
        public AIData aiData { get; set; }

        private bool generatedDefaultData = false;

        public void GenerateDefaultAIData()
        {
            if(!generatedDefaultData)
            {
                defaultAIData = AIDataManager.GenerateAIData(gameObject.GetComponent<AIBrain>());
                defaultAIData.defaultMentalState = "Default";
                defaultAIData.defaultEngagedMode = "Default";
                defaultAIData.defaultOmniEngagedMode = "Default";
                defaultAIData.agroColor = "Default";
                defaultAIData.baseColor = "Default";

                generatedDefaultData = true;
            }
        }
    }
}
