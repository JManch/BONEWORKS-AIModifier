 using UnityEngine;
using System;
using StressLevelZero.Interaction;
using StressLevelZero.Rig;
using ModThatIsNotMod;
using MelonLoader;
using Boneworks;

namespace AIModifier.UI
{
    public class MenuPointerController : MonoBehaviour
    {
        public MenuPointerController(IntPtr ptr) : base(ptr) { }

        public MenuPointerManager.PointerHand pointerHand { get; set; }
        public Hand hand { get; private set; }

        private bool pointingAtUI;
        private bool pointerActive;
        private GameObject pointerTip;
        private RaycastHit pointerHit;
        private GameObject laserDirection;
        
        void Awake()
        {
            hand = transform.parent.parent.GetComponent<Hand>();
            InstantiatePointerTip();
            laserDirection = new GameObject("LaserDirection");
        }

        void Start()
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = transform.parent.rotation;
            pointerTip.gameObject.SetActive(false);

            laserDirection.transform.SetParent(transform);
            laserDirection.transform.rotation = transform.rotation;
            laserDirection.transform.localPosition = Vector3.zero;
            if (hand.gameObject.name == "Hand (left)")
            {
                laserDirection.transform.localEulerAngles = new Vector3(350, 320, 0);
            }
            else
            {
                laserDirection.transform.localEulerAngles = new Vector3(350, 40, 0);
            }
        }

        void FixedUpdate()
        {
            // Need some kind of limitation here to prevent the raycast always running. Only run when within x units of the UI?
            pointingAtUI = PerformHandRaycast();

            if (MenuPointerManager.activePointerHand == pointerHand && pointingAtUI)
            {
                ActivatePointer();
                pointerTip.transform.position = pointerHit.point;
            }
            else  if (pointingAtUI) // If we are pointing at UI but we are not the active pointer
            {
                DeactivatePointer();
                
                // If trigger down then set this pointer as active
                if(hand.controller.GetPrimaryInteractionButtonDown())
                {
                    MenuPointerManager.SwitchActivePointer(pointerHand);
                }
            }
            else if (pointerTip.gameObject.active)
            {
                DeactivatePointer();
            }
        }

        private void ActivatePointer()
        {
            if(!pointerActive)
            {
                pointerTip.gameObject.SetActive(true);
                pointerActive = true;
            }
        }

        private void DeactivatePointer()
        {
            if(pointerActive)
            {
                pointerTip.gameObject.SetActive(false);
                pointerActive = false;
            }
        }

        private bool PerformHandRaycast()
        {
            return Physics.Raycast(laserDirection.transform.position, laserDirection.transform.forward, out pointerHit, 100f, 1 << 5, QueryTriggerInteraction.Collide);
        }

        private void InstantiatePointerTip()
        {
            pointerTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerTip.name = "PointerTip";
            pointerTip.transform.SetParent(transform);
            pointerTip.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            pointerTip.GetComponent<Renderer>().material.color = Color.green;
            Rigidbody rigidbody = pointerTip.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            pointerTip.layer = 30;
        }

        /*
        private void GetSLZLaser()
        {
            GameObject tempGun = CustomItems.SpawnFromPool("Rifle M16 Laser Foregrip", Vector3.zero, Quaternion.identity);
            GameObject laser = tempGun.transform.FindChild("attachment_Lazer/offset/LaserPointer").gameObject;
            laser.transform.SetParent(transform);
            if(hand.gameObject.name == "Hand (left)")
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
        */
    }
}
