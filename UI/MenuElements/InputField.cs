using UnityEngine;
using System;
using System.Globalization;

namespace AIModifier.UI
{
    public class InputField : MenuElement
    {
        private TextDisplay textBox;

        private string value;
        private Action<string> onValueChanged;
        private bool useMinMax;
        private float maxValue;
        private float minValue;
        private InputType inputType;

        public enum InputType
        {
            Keyboard,
            Numpad
        }

        public InputField(MenuPage menuPage, GameObject gameObject, string inputFieldText, string defaultText, InputType inputType, TextProperties textProperties, float minValue = default, float maxValue = default, Action<string> onValueChanged = null) : base(menuPage, gameObject)
        {
            new TextDisplay(menuPage, gameObject.transform.GetChild(0).gameObject, inputFieldText, textProperties);
            textBox = new TextDisplay(menuPage, gameObject.transform.FindChild("InputBox").gameObject, defaultText, textProperties);
            new Button(menuPage, gameObject.transform.FindChild("Select").gameObject, "", textProperties, Button.ButtonHighlightType.Color, OnSelect);
            this.maxValue = maxValue;
            this.minValue = minValue;
            if(maxValue == default && minValue == default)
            {
                useMinMax = false;
            }
            else
            {
                useMinMax = true;
            }
            this.onValueChanged = onValueChanged;
            this.inputType = inputType;
            value = defaultText;
        }

        public override object GetValue()
        {
            return value;
        }

        public override void SetValue(object value)
        {
            this.value = value.ToString();

            if (useMinMax && float.TryParse(this.value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
            {
                if(result > maxValue)
                {
                    this.value = maxValue.ToString();
                }
                else if(result < minValue)
                {
                    this.value = minValue.ToString();
                }
            }

            SetDisplayValue(this.value);
        }

        public string GetDisplayValue()
        {
            return textBox.GetValue().ToString();
        }

        public void SetDisplayValue(string value)
        {
            textBox.SetValue(value);
        }

        public override void OnPageClose()
        {
            textBox.SetValue(value);
            if(Keyboard.numpad != null && Keyboard.numpad.isOpen)
            {
                Keyboard.numpad.CloseMenu();
            }
        }

        // Only update the "actual" value once the user presses Enter 
        public void OnEnterPressed()
        {
            if(GetDisplayValue() != "")
            {
                SetValue(GetDisplayValue());
                if (onValueChanged != null)
                {
                    onValueChanged(value);
                }
            }
            else
            {
                SetDisplayValue(GetValue().ToString());
            }
            
        }

        public void OnDeactivated()
        {
            textBox.SetValue(value);
        }

        private void OnSelect()
        {
            // Active the keyboard
            if(inputType == InputType.Keyboard)
            {
                Keyboard.OpenKeyboard();
                Keyboard.keyboard.SetActiveInputField(this);
            }
            else
            {
                Keyboard.OpenNumpad();
                Keyboard.numpad.SetActiveInputField(this);
            }
            SetDisplayValue("");
        }
    }
}
