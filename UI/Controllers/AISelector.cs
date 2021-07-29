using UnityEngine;
using System;
using StressLevelZero.Interaction;
using StressLevelZero.AI;

namespace AIModifier.UI
{
    public class AISelector : Pointer
    {
        public AISelector(IntPtr ptr) : base(ptr) { }

        private AIBrain aiBrain;

        protected override void OnPointerClick(RaycastHit pointerHit)
        {
            if (pointerHit.transform != null)
            {
                aiBrain = pointerHit.transform.root.gameObject.GetComponent<AIBrain>();
                if (aiBrain != null)
                {
                    if(AIMenuManager.aiSelectorType == AISelectedPlateController.SelectedType.Standard)
                    {
                        AI.AIManager.ToggleSelectedAI(aiBrain);
                    }
                    else
                    {
                        AI.AIManager.ToggleSelectedTargetAI(aiBrain);
                    }
                }
            }
        }
    }
}
