using System;
using UnityEngine;
using UnityEngine.UI;

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
        public Color defaultColor { get; private set; }

        private Image image;
        private TextDisplay textDisplay; 
        private ButtonHighlightType buttonHighlightType;

        public Button(MenuPage menuPage, GameObject gameObject, string buttonText, TextProperties textProperties, ButtonHighlightType buttonHighlightType, Action onButtonDown = null, Action<string> onButtonDownParameter = null) : base(menuPage, gameObject)
        {
            textDisplay = new TextDisplay(menuPage, gameObject.transform.GetChild(0).gameObject, buttonText, textProperties);
            buttonController = gameObject.AddComponent<ButtonController>();
            buttonController.button = this;
            image = gameObject.GetComponent<Image>();
            defaultColor = image.color;
            this.onButtonDown = onButtonDown;
            this.onButtonDownParameter = onButtonDownParameter;
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

        public override void OnPageClose()
        {
            OnButtonUnhighlighted();
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
            ClickEffects();

            if (onButtonDown != null)
            {
                onButtonDown();
            }

            if(onButtonDownParameter != null)
            {
                onButtonDownParameter(GetValue().ToString());
            }
        }

        public void SetColor(Color color)
        {
            image.color = color;
        }

        private void ClickEffects()
        {
            menuPage.menu.audioSource.Play();
            MenuPointerManager.activePointer.hand.controller.HapticAction(0, 0.008f, 150, 1f);
            //MenuPointerManager.activePointer.hand.controller.HapticAction(0, 0.125f, 50f, 0.2f);
        }
    }
}
