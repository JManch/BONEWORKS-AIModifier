using UnityEngine;
using System;
using System.Collections.Generic;

namespace AIModifier.UI
{
    public class SelectorUI : Menu
    {
        public Selector selector { get; set; }
        private MenuPage rootPage;
        private Color selectedColor;

        public SelectorUI(GameObject gameObject, MenuPage page) : base(gameObject, page)
        {
            gameObject.AddComponent<SmoothPlayerFollow>();
            rootPage = page;
            this.selector = selector;
            selectedColor = Color.green;
        }

        public void UpdateSelectedState(string element)
        {
            if (selector.selectorOptions[element])
            {
                ((Button)(rootPage.GetElement(element))).SetColor(selectedColor);
            }
            else
            {
                ((Button)(rootPage.GetElement(element))).SetColor(((Button)(rootPage.GetElement(element))).defaultColor);
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
                    ((Button)(rootPage.GetElement(selectorKey))).SetColor(selectedColor);
                }
                else
                {
                    ((Button)(rootPage.GetElement(selectorKey))).SetColor(((Button)(rootPage.GetElement(selectorKey))).defaultColor);
                }
            }
        }
    }
}
