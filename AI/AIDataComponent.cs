using System;
using StressLevelZero.AI;
using UnityEngine;

namespace AIModifier.AI
{
    class AIDataComponent : MonoBehaviour
    {
        public AIDataComponent(IntPtr ptr) : base(ptr) { }

        public AIData defaultAIData { get; private set; }

        void Awake()
        {
            defaultAIData = AIDataManager.GenerateAIData(gameObject.GetComponent<AIBrain>());
        }
    }
}
