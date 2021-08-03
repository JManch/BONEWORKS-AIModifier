using TMPro;
using UnityEngine;

namespace AIModifier.UI
{
    public class TextDisplay : MenuElement
    {
        private string value;
        public TextMeshProUGUI text { get; private set; }

        public TextDisplay(MenuPage menuPage, GameObject gameObject, string text, TextProperties textProperties) : base(menuPage, gameObject)
        {
            Vector2 sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
            this.text = gameObject.AddComponent<TextMeshProUGUI>();
            this.text.text = text;
            this.text.fontSize = textProperties.fontSize;
            this.text.color = textProperties.color;
            this.text.characterSpacing = textProperties.characterSpacing;
            this.text.enableAutoSizing = textProperties.autoSize;
            if(textProperties.autoSize)
            {
                this.text.fontSizeMin = 1;
                this.text.fontSizeMax = textProperties.fontSize;
            }
            this.text.alignment = textProperties.textAlignment;
            gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            this.value = text;
        }

        public override object GetValue()
        {
            return value;
        }

        public override void SetValue(object value)
        {
            this.value = value.ToString();
            text.text = value.ToString();
        }

        public void SetColor(Color color)
        {
            text.color = color;
        }
    }
}
