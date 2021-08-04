using UnityEngine;

namespace AIModifier.UI
{
    public class Keyboard : Menu
    {
        public static Keyboard numpad { get; private set; }
        public static Keyboard keyboard { get; private set; }

        private InputField activeInputField;
        
        private bool capsLock;

        public Keyboard(GameObject gameObject) : base(gameObject)
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

        private static void BuildNumpad()
        {
            GameObject numpadPrefab = GameObject.Instantiate(Utilities.AssetManager.numpadPrefab, Utilities.AssetManager.playerPelvis.position + 0.8f * Utilities.AssetManager.playerPelvis.transform.forward, Quaternion.identity);

            // Define new menu and keyboard
            numpad = new Keyboard(numpadPrefab);
            MenuPage rootPage = new MenuPage(numpadPrefab.transform.FindChild("RootPage").gameObject);

            // Configure number page
            Transform rootPageTransform = numpadPrefab.transform.FindChild("RootPage");

            TextProperties textProperties = new TextProperties(4, Color.white);

            // Digits
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("0").gameObject, "0", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("1").gameObject, "1", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("2").gameObject, "2", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("3").gameObject, "3", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("4").gameObject, "4", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("5").gameObject, "5", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("6").gameObject, "6", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("7").gameObject, "7", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("8").gameObject, "8", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("9").gameObject, "9", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild(".").gameObject, ".", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));

            // Buttons
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Backspace").gameObject, "<-", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Done").gameObject, "Done", new TextProperties(3, Color.white), Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { numpad.OnKeyPressed(s); }));
            numpad.AddPage(rootPage);
        }

        #endregion

        #region Keyboard
        
        public static void OpenKeyboard()
        {
            if (keyboard == null || keyboard.gameObject == null)
            {
                BuildKeyboard();
            }
            else
            {
                keyboard.OpenMenu();
            }
        }

        private static void BuildKeyboard()
        {
            GameObject keyboardPrefab = GameObject.Instantiate(Utilities.AssetManager.keyboardPrefab, Utilities.AssetManager.playerPelvis.position + 0.8f * Utilities.AssetManager.playerPelvis.transform.forward, Quaternion.identity);

            keyboard = new Keyboard(keyboardPrefab);
            MenuPage rootPage = new MenuPage(keyboardPrefab.transform.FindChild("RootPage").gameObject);

            Transform rootPageTransform = keyboardPrefab.transform.FindChild("RootPage");

            TextProperties textProperties = new TextProperties(4, Color.white);

            // Digits
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("1").gameObject, "1", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("2").gameObject, "2", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("3").gameObject, "3", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("4").gameObject, "4", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("5").gameObject, "5", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("6").gameObject, "6", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("7").gameObject, "7", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("8").gameObject, "8", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("9").gameObject, "9", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("0").gameObject, "0", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));

            // Row 1
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("q").gameObject, "q", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("w").gameObject, "w", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("e").gameObject, "e", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("r").gameObject, "r", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("t").gameObject, "t", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("y").gameObject, "y", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("u").gameObject, "u", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("i").gameObject, "i", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("o").gameObject, "o", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("p").gameObject, "p", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));

            // Row 2
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("a").gameObject, "a", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("s").gameObject, "s", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("d").gameObject, "d", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("f").gameObject, "f", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("g").gameObject, "g", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("h").gameObject, "h", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("j").gameObject, "j", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("k").gameObject, "k", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("l").gameObject, "l", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));

            // Row 3
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("z").gameObject, "z", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("x").gameObject, "x", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("c").gameObject, "c", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("v").gameObject, "v", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("b").gameObject, "b", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("n").gameObject, "n", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("m").gameObject, "m", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));

            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Space").gameObject, "-", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Backspace").gameObject, "<-", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Caps").gameObject, "Caps", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));
            rootPage.AddElement(new Button(rootPage, rootPageTransform.FindChild("Done").gameObject, "Done", textProperties, Button.ButtonHighlightType.Color, null, null, null, delegate (string s) { keyboard.OnKeyPressed(s); }));

            keyboard.AddPage(rootPage);
        }

        #endregion

        public void OnKeyPressed(string key)
        {
            switch (key)
            {
                case "<-":
                    if (activeInputField.GetDisplayValue().ToString().Length > 0)
                    {
                        // Remove the last character
                        activeInputField.SetDisplayValue(activeInputField.GetDisplayValue().Remove(activeInputField.GetDisplayValue().Length - 1));
                    }
                    break;
                case "Done":
                    activeInputField.OnEnterPressed();
                    activeInputField.OnDeactivated();
                    activeInputField = null;
                    CloseMenu();
                    break;
                case "-":
                    activeInputField.SetDisplayValue(activeInputField.GetDisplayValue() + " ");
                    break;
                case "Caps":
                    if(!capsLock)
                    {
                        capsLock = true;
                        ((Button)keyboard.GetPage("RootPage").GetElement("Caps")).SetColor(new Color(0.3f, 0.3f, 0.3f, 0.75f));

                        for (char c = 'a'; c <= 'z'; c++)
                        {
                            keyboard.GetPage("RootPage").GetElement(c.ToString()).SetValue(c.ToString().ToUpper());
                        }
                    }
                    else
                    {
                        capsLock = false;
                        ((Button)keyboard.GetPage("RootPage").GetElement("Caps")).SetColor(((Button)keyboard.GetPage("RootPage").GetElement("Caps")).defaultColor);

                        for (char c = 'a'; c <= 'z'; c++)
                        {
                            keyboard.GetPage("RootPage").GetElement(c.ToString()).SetValue(c.ToString());
                        }
                    }
                    break;
                default:
                    if(capsLock)
                    {
                        activeInputField.SetDisplayValue(activeInputField.GetDisplayValue() + key.ToUpper());
                    }
                    else
                    {
                        activeInputField.SetDisplayValue(activeInputField.GetDisplayValue() + key);
                    }
                    break;
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
    }
}
