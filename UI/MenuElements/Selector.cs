using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIModifier.UI
{
    public class Selector : MenuElement
    {
        public Dictionary<string, bool> selectorOptions { get; private set; }

        private Action<string> onValueChanged;
        private SelectorUI selectorUI;

        // Passed selector UI prefab must be built
        public Selector(MenuPage menuPage, GameObject gameObject, SelectorUI selectorUI, string selectorText, TextProperties textProperties, string[] options, Action<string> onValueChanged = null) : base(menuPage, gameObject)
        {
            new TextDisplay(menuPage, gameObject.transform.GetChild(0).gameObject, selectorText, textProperties);
            Button selectorButton = new Button(menuPage, gameObject.transform.FindChild("Select").gameObject, "Edit", textProperties, Button.ButtonHighlightType.Color, OpenSelector);
            selectorOptions = new Dictionary<string, bool>();
            foreach (string s in options)
            {
                selectorOptions.Add(s, false);
            }

            selectorUI.selector = this;
            this.selectorUI = selectorUI;
            this.onValueChanged = onValueChanged;
        }

        private void OpenSelector()
        {
            if (!selectorUI.isOpen)
            {
                selectorUI.OpenMenu();
                foreach (string key in selectorOptions.Keys.ToList()) 
                {
                    selectorUI.UpdateSelectedState(key);
                }
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
            foreach (string key in selectorOptions.Keys.ToList())
            {
                selectorOptions[key] = false;
            }

            string[] arr = value as string[];
            if (arr != null)
            {
                foreach(string s in arr)
                {
                    if(s != "0")
                    {
                        selectorOptions[s] = true;
                        selectorUI.UpdateSelectedState(s);
                    }
                }    
            }
        }

        public override object GetValue()
        {
            List<string> value = new List<string>();
            
            foreach(string key in selectorOptions.Keys.ToList())
            {
                if(selectorOptions[key])
                {
                    value.Add(key);
                }
            }

            return value.ToArray<string>();
        }

        public void OnEnterPressed()
        {
            string[] value = GetValue() as string[];

            string result = "";
            foreach(string s in value)
            {
                result = result + s + ", ";
            }

            if(value.Length > 0)
            {
                result = result.Remove(result.Length - 2, 2);
            }

            onValueChanged(result);
        }

        public override void OnPageClose()
        {
            selectorUI.CloseMenu();
        }
    }
}
