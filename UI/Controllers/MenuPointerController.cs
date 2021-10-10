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
        private RaycastHit pointerHit;

        private GameObject laser;
        private GameObject pointerTip;
        private GameObject pointerCenter;
        private GameObject pointerOrigin;

        void Awake()
        {
            hand = transform.parent.parent.GetComponent<Hand>();
            laser = new GameObject("Laser");
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
            laser.gameObject.SetActive(false);
        }

        void FixedUpdate()
        {
            if (AIMenuManager.aiMenu != null && AIMenuManager.aiMenu.isOpen && Vector3.Distance(AIMenuManager.aiMenu.gameObject.transform.position, hand.transform.position) < 5f)
            {
                pointingAtUI = PerformHandRaycast();
            }
            else
            {
                pointingAtUI = false;
            }

            if (MenuPointerManager.activePointerHand == pointerHand && pointingAtUI)
            {
                ActivatePointer();
                pointerTip.transform.position = pointerHit.point;
                pointerCenter.transform.localPosition = pointerTip.transform.localPosition * 0.65f;
            }
            else  if (pointingAtUI) // If we are pointing at UI but we are not the active pointer
            {
                DeactivatePointer();
                
                // If trigger down then set this pointer as active
                if(Utilities.Utilities.GetTriggerDown(hand))
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
            if(!laser.gameObject.active)
            {
                laser.gameObject.SetActive(true);
            }
        }

        private void DeactivatePointer()
        {
            if(laser.gameObject.active)
            {
                laser.gameObject.SetActive(false);
            }
        }

        private bool PerformHandRaycast()
        {
            return Physics.Raycast(laser.transform.position, laser.transform.forward, out pointerHit, 100f, 1 << 31, QueryTriggerInteraction.Collide);
        }

        private void InstantiatePointer()
        {
            Material transparentMaterial = new Material(Shader.Find("Standard"));
            transparentMaterial.SetOverrideTag("RenderType", "Transparent");
            transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparentMaterial.SetInt("_ZWrite", 0);
            transparentMaterial.DisableKeyword("_ALPHATEST_ON");
            transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
            transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparentMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            transparentMaterial.color = new Color(0, 0.5f, 0, 0.8f);

            pointerTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerTip.name = "PointerTip";
            pointerTip.transform.SetParent(laser.transform);
            pointerTip.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //pointerTip.GetComponent<MeshRenderer>().enabled = false;
            Rigidbody rigidbody = pointerTip.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            pointerTip.layer = 30;

            pointerCenter = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerCenter.name = "PointerCenter";
            pointerCenter.transform.SetParent(laser.transform);
            pointerCenter.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            pointerCenter.GetComponent<SphereCollider>().enabled = false;
            pointerCenter.GetComponent<Renderer>().material = transparentMaterial;

            pointerOrigin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerOrigin.name = "PointerOrigin";
            pointerOrigin.transform.SetParent(laser.transform);
            pointerOrigin.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            pointerOrigin.transform.localPosition = Vector3.zero;
            pointerOrigin.GetComponent<SphereCollider>().enabled = false;
            pointerOrigin.GetComponent<Renderer>().material = transparentMaterial;
        }
    }
}
