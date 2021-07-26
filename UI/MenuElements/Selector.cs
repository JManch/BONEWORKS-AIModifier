using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIModifier.UI
{
    public class Selector : MenuElement
    {
        public Dictionary<string, bool> selectorOptions { get; private set; }

        private SelectorUI selectorUI;

        // Passed selector UI prefab must be built
        public Selector(GameObject gameObject, SelectorUI selectorUI, string selectorText, TextProperties textProperties, List<string> options) : base(gameObject)
        {
            new TextDisplay(gameObject.transform.GetChild(0).gameObject, selectorText, textProperties);
            Button selectorButton = new Button(gameObject.transform.FindChild("Select").gameObject, options[0], textProperties, Button.ButtonHighlightType.Color, OpenSelector);

            selectorOptions = new Dictionary<string, bool>();
            foreach (string s in options)
            {
                selectorOptions.Add(s, false);
            }

            selectorUI.selector = this;
            this.selectorUI = selectorUI;

            // REWRITE
            SetValue(options[0]);
        }

        private void OpenSelector()
        {
            if (!selectorUI.isOpen)
            {
                selectorUI.OpenMenu();
            }
        }

        public void CloseSelector()
        {
            if (selectorUI.isOpen)
            {
                selectorUI.CloseMenu();
            }
        }

        // Pass an array of strings to set the value
        public override void SetValue(object value)
        {
            // Reset dictionary
            foreach (string key in selectorOptions.Keys)
            {
                selectorOptions[key] = false;
            }

            string[] arr = value as string[];
            if (arr != null)
            {
                foreach(string s in arr)
                {
                    selectorOptions[s] = true;
                    selectorUI.UpdateSelectedState(s);
                }    
            }
        }

        public override object GetValue()
        {
            List<string> value = new List<string>();
            
            foreach(string key in selectorOptions.Keys)
            {
                if(selectorOptions[key])
                {
                    value.Add(key);
                }
            }

            return value.ToArray<string>();
        }
    }
}
