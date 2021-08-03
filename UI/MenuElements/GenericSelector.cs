using UnityEngine;
using System.Collections.Generic;
using System;
using MelonLoader;

namespace AIModifier.UI
{
    class GenericSelector<T> : MenuElement
    {
        private T[] elements;
        private TextDisplay itemDisplay;
        private int pointer = 0;
        private Action<T> action;

        public GenericSelector(MenuPage menuPage, GameObject gameObject, string elementSelectorText, TextProperties textProperties, T[] elements, Action<T> onSelectorAction = null) : base(menuPage, gameObject)
        {
            // Configure text components
            new TextDisplay(menuPage, gameObject.transform.GetChild(0).gameObject, elementSelectorText, textProperties);
            itemDisplay = new TextDisplay(menuPage, gameObject.transform.FindChild("ElementDisplay").gameObject, "", textProperties);
            new Button(menuPage, gameObject.transform.FindChild("Next").gameObject, ">", textProperties, Button.ButtonHighlightType.Color, NextItem);
            new Button(menuPage, gameObject.transform.FindChild("Previous").gameObject, "<", textProperties, Button.ButtonHighlightType.Color, PrevItem);
            this.elements = elements;
            if (this.elements.Length > 0)
            {
                itemDisplay.SetValue(elements[0]);
            }

            if(onSelectorAction != null)
            {
                action += onSelectorAction;
            }
        }

        public override object GetValue()
        {
            return elements[pointer];
        }

        public override void SetValue(object value)
        {
            for(int i = 0; i < elements.Length; ++i)
            {
                if(elements[i].Equals(value))
                {
                    this.pointer = i;
                }
            }
            itemDisplay.SetValue(elements[pointer]);
        }

        public override void OnPageOpen()
        {
            if(action != null)
            {
                action(elements[pointer]);
            }
        }

        private void NextItem()
        {
            if(pointer != elements.Length - 1)
            {
                ++pointer;
                itemDisplay.SetValue(elements[pointer]);
            }
            else
            {
                pointer = 0;
                itemDisplay.SetValue(elements[pointer]);
            }

            if(action != null)
            {
                action(elements[pointer]);
            }
        }

        private void PrevItem()
        {
            if(pointer != 0)
            {
                --pointer;
                itemDisplay.SetValue(elements[pointer]);
            }
            else
            {
                pointer = elements.Length - 1;
                itemDisplay.SetValue(elements[pointer]);
            }

            if (action != null)
            {
                action(elements[pointer]);
            }
        }
    }
}
