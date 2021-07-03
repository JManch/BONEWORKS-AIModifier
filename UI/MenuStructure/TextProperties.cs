using UnityEngine;
using TMPro;

namespace AIModifier.UI
{
    public class TextProperties
    {
        public float fontSize { get; private set; }
        public bool autoSize { get; private set; }
        public Color color { get; private set; }
        public int characterSpacing { get; private set; }
        public TextAlignmentOptions textAlignment { get; private set; }

        public TextProperties(float fontSize, Color color, bool autoSize = false, int characterSpacing = 0, TextAlignmentOptions textAlignment = TextAlignmentOptions.Center)
        {
            this.fontSize = fontSize;
            this.color = color;
            this.autoSize = autoSize;
            this.characterSpacing = characterSpacing;
            this.textAlignment = textAlignment;
        }

    }
}
