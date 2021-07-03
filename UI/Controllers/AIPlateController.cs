using System;
using UnityEngine;
using StressLevelZero.AI;
using AIModifier.AI;
using ModThatIsNotMod;
using MelonLoader;

namespace AIModifier.UI
{
    class AIPlateController : MonoBehaviour
    {
        public AIPlateController(IntPtr ptr) : base(ptr) { }

        protected AIBrain aiBrain;
        protected AIData aiData;
        protected Transform playerHead;

        protected virtual void Awake()
        {
            aiBrain = transform.GetComponent<AIBrain>();
            aiData = AIDataManager.aiDataDictionary[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];
            playerHead = Player.GetPlayerHead().transform;
        }

        protected virtual void Update()
        {

        }
    }
}
