using System;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using StressLevelZero.Interaction;

namespace AIModifier.UI
{
    class ButtonController : MonoBehaviour
    {
        public ButtonController(IntPtr ptr) : base(ptr) { }

        public Button button { get; set; }

        private bool buttonHighlighted;
        private Vector3 halfExtents;

        void Awake()
        {
            halfExtents = new Vector3(GetComponent<RectTransform>().sizeDelta.x / 2, GetComponent<RectTransform>().sizeDelta.y / 2, 0.1f);
        }

        void FixedUpdate()
        {
            if (Physics.CheckBox(transform.position, halfExtents, transform.rotation, 1 << 30))
            {
                if(!buttonHighlighted)
                {
                    // Trigger enter here
                    buttonHighlighted = true;
                    button.OnButtonHighlighted();
                }

                if (Utilities.Utilities.GetTriggerDown(MenuPointerManager.activePointer.hand))
                {
                    button.OnButtonDown();
                }
            }
            else
            {
                if(buttonHighlighted)
                {
                    // Trigger exit Here
                    button.OnButtonUnhighlighted();
                    buttonHighlighted = false;
                }
            }
        }
    }
}
