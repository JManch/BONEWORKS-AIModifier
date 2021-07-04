using System;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;

namespace AIModifier.UI
{
    class Button : MenuElement
    {        
        private ButtonController buttonController;

        public enum ButtonHighlightType 
        {
            Color,
            Underline
        }

        public Action onButtonDown { get; set; }
        public Action<string> onButtonDownParameter { get; set; }
        public Action<string> onButtonParameter { get; set; }
        public Action onButtonUp { get; set; }
        public Action onButton { get; set; }

        private TextDisplay textDisplay;
        private Image image;
        private Color defaultColor;
        private ButtonHighlightType buttonHighlightType;

        public Button(GameObject gameObject, string buttonText, TextProperties textProperties, ButtonHighlightType buttonHighlightType, Action onButtonDown = null, Action onButton = null, Action onButtonUp = null, Action<string> onButtonDownParameter = null, Action<string> onButtonParameter = null) : base(gameObject)
        {
            textDisplay = new TextDisplay(gameObject.transform.GetChild(0).gameObject, buttonText, textProperties);
            buttonController = gameObject.AddComponent<ButtonController>();
            buttonController.button = this;
            image = gameObject.GetComponent<Image>();
            defaultColor = image.color;
            this.onButtonDown = onButtonDown;
            this.onButton = onButton;
            this.onButtonUp = onButtonUp;
            this.onButtonDownParameter = onButtonDownParameter;
            this.onButtonParameter = onButtonParameter;
            this.buttonHighlightType = buttonHighlightType;
        }

        public override object GetValue()
        {
            return textDisplay.GetValue();
        }

        public override void SetValue(object value)
        {
            textDisplay.SetValue(value);
        }

        public override void OnPageOpen()
        {
            DisableOnButton();
        }

        public void DisableOnButton()
        {
            buttonController.disableOnButton = true;
        }

        public void OnButtonHighlighted() 
        {
            if(buttonHighlightType == ButtonHighlightType.Color)
            {
                image.color = AIMenuManager.uiHighlightColor;
            }
            else
            {
                textDisplay.text.fontStyle = TMPro.FontStyles.Underline;
            }
        }

        public void OnButtonUnhighlighted()
        {
            if (buttonHighlightType == ButtonHighlightType.Color)
            {
                image.color = defaultColor;
            }
            else
            {
                textDisplay.text.fontStyle = TMPro.FontStyles.Normal;
            }
        }

        public void OnButtonDown()
        {
            if (onButtonDown != null)
            {
                onButtonDown();
            }

            if(onButtonDownParameter != null)
            {
                onButtonDownParameter(GetValue().ToString());
            }
        }

        public void OnButtonUp()
        {
            if (onButtonUp != null)
            {
                onButtonUp();
            }
        }

        private readonly float activateFrequency = 0.5f;
        private float nextActivateTime;

        public void OnButton()
        {
            if (onButton != null)
            {
                if(Time.fixedTime > nextActivateTime)
                {
                    nextActivateTime = Time.fixedTime + activateFrequency;
                    onButton();
                }
            }

            if(onButtonParameter != null)
            {
                if(nextActivateTime == 0)
                {
                    nextActivateTime = Time.fixedTime;
                }

                if (Time.fixedTime > nextActivateTime)
                {
                    nextActivateTime = Time.fixedTime + activateFrequency;
                    onButtonParameter(GetValue().ToString());
                }
            }
        }
    }
}
