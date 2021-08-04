using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MelonLoader;
using System;

namespace AIModifier.UI
{
    public class ButtonList : MenuElement
    {

        public TextDisplay statusText { get; private set; }
        private TextDisplay pageText;
        private List<Button> buttons;
        private List<string> elements;

        private int openPage;
        private int totalPages;

        public ButtonList(MenuPage menuPage, GameObject gameObject, TextProperties textProperties, Action<string> buttonAction) : base(menuPage, gameObject)
        {
            statusText = new TextDisplay(menuPage, gameObject.transform.FindChild("StatusText").gameObject, "", new TextProperties(6, Color.white, true));
            pageText = new TextDisplay(menuPage, gameObject.transform.FindChild("PageText").gameObject, "", new TextProperties(6, Color.white, true));

            new Button(menuPage, gameObject.transform.FindChild("Next").gameObject, ">", textProperties, Button.ButtonHighlightType.Color, LoadNextPage);
            new Button(menuPage, gameObject.transform.FindChild("Previous").gameObject, "<", textProperties, Button.ButtonHighlightType.Color, LoadPreviousPage);

            buttons = new List<Button>();
            elements = new List<string>();

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).name == "Button")
                {
                    buttons.Add(new Button(menuPage, gameObject.transform.GetChild(i).gameObject, "", new TextProperties(7f, Color.white, true), Button.ButtonHighlightType.Color, null, null, null, buttonAction));
                }
            }

            openPage = 1;
        }

        public override object GetValue()
        {
            throw new System.NotImplementedException();
        }

        // Set value with a list of button names
        public override void SetValue(object value)
        {
            elements.Clear();

            string[] arr = value as string[];
            if (arr != null)
            {
                foreach(string s in arr)
                {
                    elements.Add(s);
                }
            }

            totalPages = (int)Math.Ceiling((double)elements.Count / buttons.Count);
            if(totalPages == 0)
            {
                totalPages = 1;
            }

            RefreshButtons();
        }

        private void RefreshButtons()
        {
            foreach (Button button in buttons)
            {
                button.SetValue("");
            }

            for (int i = 0; i < elements.Count; i++)
            {
                // If the element is on the current page
                if(buttons.Count * (openPage - 1) <= i && i <= (buttons.Count * openPage) - 1)
                {
                    buttons[i - (openPage - 1) * buttons.Count].SetValue(elements[i]);
                }
            }
            pageText.SetValue("Page " + openPage + "/" + totalPages);
        }

        private void LoadNextPage()
        {
            if(openPage != totalPages)
            {
                ++openPage;
                RefreshButtons();
            }
        }

        private void LoadPreviousPage()
        {
            if(openPage != 1)
            {
                --openPage;
                RefreshButtons();
            }
        }
    }
}
