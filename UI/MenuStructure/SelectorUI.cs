using UnityEngine;
using System;
using System.Collections.Generic;

namespace AIModifier.UI
{
    public class SelectorUI : Menu
    {
        public Selector selector { get; set; }
        private Color selectedColor;

        public SelectorUI(GameObject gameObject) : base(gameObject)
        {
            gameObject.AddComponent<SmoothPlayerFollow>();
            LookAtPlayer lookAtPlayer = gameObject.AddComponent<LookAtPlayer>();
            lookAtPlayer.fixedXAxis = false;

            this.selector = selector;
            selectedColor = Color.green;
        }

        public void UpdateSelectedState(string element)
        {
            if (selector.selectorOptions[element])
            {
                ((Button)(activePage.GetElement(element))).SetColor(selectedColor);
            }
            else
            {
                ((Button)(activePage.GetElement(element))).SetColor(((Button)(activePage.GetElement(element))).defaultColor);
            }
        }

        public void OnKeyPressed(string selectorKey)
        {
            if(selectorKey == "Enter")
            {
                selector.OnEnterPressed();
                selector.CloseSelector();
            }
            else
            {
                selector.selectorOptions[selectorKey] = !selector.selectorOptions[selectorKey];

                // Highlight the button accordingly 
                if (selector.selectorOptions[selectorKey])
                {
                    ((Button)(activePage.GetElement(selectorKey))).SetColor(selectedColor);
                }
                else
                {
                    ((Button)(activePage.GetElement(selectorKey))).SetColor(((Button)(activePage.GetElement(selectorKey))).defaultColor);
                }
            }
        }
    }
}
