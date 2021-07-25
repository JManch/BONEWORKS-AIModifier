using UnityEngine;
using ModThatIsNotMod;

namespace AIModifier.UI
{
    public static class KeyboardManager
    {

        public static Keyboard numpad;

        public static void OpenNumpad()
        {
            if (numpad == null || numpad.gameObject == null)
            {
                Transform playerPelvis = Player.GetRigManager().transform.FindChild("[SkeletonRig (Realtime SkeleBones)]").FindChild("Pelvis");
                GameObject numpad = GameObject.Instantiate(Utilities.AssetManager.numpadPrefab, playerPelvis.position + 0.8f * playerPelvis.transform.forward, Quaternion.identity);
                BuildNumpad(numpad);
            }
            else
            {
                numpad.OpenMenu();
            }
        }

        public static void CloseNumpad()
        {
            if (numpad != null && numpad.isOpen)
            {
                numpad.CloseMenu();
            }
        }

        public static void BuildNumpad(GameObject numpadPrefab)
        {
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
