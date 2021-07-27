using UnityEngine;

namespace AIModifier.UI
{
    public class Keyboard : Menu
    {
        public static Keyboard numpad { get; private set; }

        private InputField activeInputField;

        public Keyboard(GameObject gameObject, MenuPage page) : base(gameObject, page)
        {
            gameObject.AddComponent<SmoothPlayerFollow>();
        }

        #region Numpad

        public static void OpenNumpad()
        {
            if (numpad == null || numpad.gameObject == null)
            {
                BuildNumpad();
            }
            else
            {
                numpad.OpenMenu();
            }
        }

        #endregion

        public void OnKeyPressed(string key)
        {
            if(key == "<-")
            {
                if(activeInputField.GetDisplayValue().ToString().Length > 0)
                {
                    // Remove the last character
                    activeInputField.SetDisplayValue(activeInputField.GetDisplayValue().Remove(activeInputField.GetDisplayValue().Length - 1));
                }
            }
            else if(key == "Enter")
            {
                activeInputField.OnEnterPressed();
                activeInputField.OnDeactivated();
                activeInputField = null;
                CloseMenu();
            }
            else
            {
                activeInputField.SetDisplayValue(activeInputField.GetDisplayValue() + key);
            }
        }

        public void SetActiveInputField(InputField inputField)
        {
            if(activeInputField != null)
            {
                activeInputField.OnDeactivated();
            }
            activeInputField = inputField;
        }

        private static void BuildNumpad()
        {
            GameObject numpadPrefab = GameObject.Instantiate(Utilities.AssetManager.numpadPrefab, Utilities.AssetManager.playerPelvis.position + 0.8f * Utilities.AssetManager.playerPelvis.transform.forward, Quaternion.identity);

            // Define new menu and keyboard
            MenuPage rootPage = new MenuPage(numpadPrefab.transform.FindChild("RootPage").gameObject);
            numpad = new Keyboard(numpadPrefab, rootPage);

            // Configure number page
            Transform rootPageTransform = numpadPrefab.transform.FindChild("RootPage");

            TextProperties textProperties = new TextProperties(4, Color.white);

            // Digits
            rootPage.AddElement(new Button(rootPageTransform.FindChild("0").gameObject, "0", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("1").gameObject, "1", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("2").gameObject, "2", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("3").gameObject, "3", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("4").gameObject, "4", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("5").gameObject, "5", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("6").gameObject, "6", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("7").gameObject, "7", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("8").gameObject, "8", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("9").gameObject, "9", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild(".").gameObject, ".", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));

            // Buttons
            rootPage.AddElement(new Button(rootPageTransform.FindChild("Backspace").gameObject, "<-", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPageTransform.FindChild("Enter").gameObject, "Enter", new TextProperties(3, Color.white), Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
        }
    }
}
