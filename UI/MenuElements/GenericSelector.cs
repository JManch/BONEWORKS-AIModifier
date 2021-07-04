using UnityEngine;
using System.Collections.Generic;
using System;
using MelonLoader;

namespace AIModifier.UI
{
    class GenericSelector<T> : MenuElement
    {
        private List<T> elements;
        private TextDisplay itemDisplay;
        private int pointer = 0;
        private Action<T> action;

        private Button nextButton;
        private Button prevButton;

        public GenericSelector(GameObject gameObject, string elementSelectorText, TextProperties textProperties, List<T> elements, Action<T> onSelectorAction = null) : base(gameObject)
        {
            // Configure text components
            new TextDisplay(gameObject.transform.GetChild(0).gameObject, elementSelectorText, textProperties);
            itemDisplay = new TextDisplay(gameObject.transform.FindChild("ElementDisplay").gameObject, "", textProperties);
            nextButton = new Button(gameObject.transform.FindChild("Next").gameObject, ">", textProperties, Button.ButtonHighlightType.Color, NextItem);
            prevButton = new Button(gameObject.transform.FindChild("Previous").gameObject, "<", textProperties, Button.ButtonHighlightType.Color, PrevItem);
            this.elements = elements;
            if (this.elements.Count > 0)
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
            for(int i = 0; i < elements.Count; ++i)
            {
                if(elements[i].Equals((T)value))
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
            if(pointer != elements.Count - 1)
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
                pointer = elements.Count - 1;
                itemDisplay.SetValue(elements[pointer]);
            }

            if (action != null)
            {
                action(elements[pointer]);
            }
        }
    }
}
