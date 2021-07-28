using UnityEngine;
using System;
using Boneworks;
using StressLevelZero.Interaction;
using ModThatIsNotMod;
using StressLevelZero.AI;
using AIModifier.AI;

namespace AIModifier.UI
{
    public class AISelectorController : MonoBehaviour
    {
        public AISelectorController(IntPtr ptr) : base(ptr) { }

        private GameObject laser;
        private GameObject pointerTip;
        private Hand hand;
        private RaycastHit pointerHit;
        private AIBrain aiBrain;
        

        void Awake()
        {
            hand = transform.parent.parent.GetComponent<Hand>();
            laser = new GameObject("SelectorLaser");
        }

        void Start()
        {
            transform.rotation = transform.parent.rotation;

            laser.transform.SetParent(transform);
            laser.transform.rotation = transform.rotation;
            laser.transform.localPosition = Vector3.zero;
            if (hand.gameObject.name == "Hand (left)")
            {
                transform.localPosition = new Vector3(-0.1f, 0.05f, 0.1f);
                laser.transform.localEulerAngles = new Vector3(350, 320, 0);
            }
            else
            {
                transform.localPosition = new Vector3(0.1f, 0.05f, 0.1f);
                laser.transform.localEulerAngles = new Vector3(350, 40, 0);
            }
            InstantiatePointer();
        }

        void FixedUpdate()
        {
            PerformHandRaycast();
            pointerTip.transform.position = pointerHit.point;

            if (hand.controller.GetPrimaryInteractionButtonDown())
            {
                if(pointerHit.transform != null)
                {
                    aiBrain = pointerHit.transform.root.gameObject.GetComponent<AIBrain>();
                    if (aiBrain != null)
                    {
                        AI.AIManager.ToggleSelectedAI(aiBrain);
                    }
                }
            }
        }

        private bool PerformHandRaycast()
        {
            return Physics.Raycast(laser.transform.position, laser.transform.forward, out pointerHit, 100f);
        }

        private void InstantiatePointer()
        {
            pointerTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerTip.name = "PointerTip";
            pointerTip.transform.SetParent(laser.transform);
            pointerTip.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            pointerTip.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
