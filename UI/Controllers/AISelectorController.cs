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

        private SLZ_LaserPointer laserPointer;
        private Hand hand;
        private RaycastHit pointerHit;

        private AIBrain aiBrain;

        void Awake()
        {
            hand = transform.parent.parent.GetComponent<Hand>();
            GetSLZLaser();
        }

        void Start()
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = transform.parent.rotation;
            laserPointer.gameObject.SetActive(true);
        }

        void Update()
        {
            if (hand.controller.GetPrimaryInteractionButtonDown())
            {
                PerformHandRaycast();

                if(pointerHit.transform != null)
                {
                    aiBrain = pointerHit.transform.root.gameObject.GetComponent<AIBrain>();
                    if (aiBrain != null)
                    {
                        if (!AIModifier.AI.AIManager.selectedAI.ContainsKey(aiBrain.name))
                        {
                            AISelectorManager.OnAISelected(aiBrain);
                        }
                        else
                        {
                            AISelectorManager.OnAIDeselected(aiBrain);
                        }
                    }
                }
            }
        }

        private bool PerformHandRaycast()
        {
            return Physics.Raycast(laserPointer.transform.position, laserPointer.transform.forward, out pointerHit, 100f);
        }

        private void GetSLZLaser()
        {
            GameObject tempGun = CustomItems.SpawnFromPool("Rifle M16 Laser Foregrip", Vector3.zero, Quaternion.identity);
            GameObject laser = tempGun.transform.FindChild("attachment_Lazer/offset/LaserPointer").gameObject;
            laser.transform.SetParent(transform);
            if (hand.gameObject.name == "Hand (left)")
            {
                laser.transform.localPosition = new Vector3(-0.04f, 0, -0.04f);
                laser.transform.eulerAngles = new Vector3(356f, 322f, 349f);
            }
            else
            {
                laser.transform.localPosition = new Vector3(0.04f, 0, -0.04f);
                laser.transform.eulerAngles = new Vector3(356f, 38f, 349f);
            }

            Destroy(tempGun);
            laserPointer = laser.GetComponent<SLZ_LaserPointer>();
        }
    }
}
