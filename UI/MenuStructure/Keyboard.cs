using UnityEngine;

namespace AIModifier.UI
{
    public class Keyboard : Menu
    {
        private InputField activeInputField;

        public Keyboard(GameObject gameObject, MenuPage page) : base(gameObject, page)
        {
            gameObject.AddComponent<KeyboardController>();
        }

        public void OnKeyPressed(string key)
        {
            if(key == "<-")
            {
                if(activeInputField.GetValue().ToString().Length > 0)
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
    }
}
