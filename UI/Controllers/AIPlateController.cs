using System;
using UnityEngine;
using StressLevelZero.AI;
using AIModifier.AI;
using ModThatIsNotMod;
using MelonLoader;

namespace AIModifier.UI
{
    public class AIPlateController : MonoBehaviour
    {
        public AIPlateController(IntPtr ptr) : base(ptr) { }

        protected AIBrain aiBrain;
        protected AIData aiData;
        protected Transform playerHead;

        protected virtual void Awake()
        {
            aiBrain = transform.GetComponent<AIBrain>();
            aiData = AIDataManager.aiData[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];
            playerHead = Player.GetPlayerHead().transform;
        }

        public virtual void OnSpawn()
        {
            aiBrain = transform.GetComponent<AIBrain>();
            aiData = AIDataManager.aiData[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];
        }

        protected virtual void Update() {}
    }
}
