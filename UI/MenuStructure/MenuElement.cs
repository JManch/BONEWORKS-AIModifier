using UnityEngine;

namespace AIModifier.UI
{
    public abstract class MenuElement
    {
        public GameObject gameObject { get; private set;}
        public MenuPage menuPage { get; private set; }

        public MenuElement(MenuPage menuPage, GameObject gameObject) 
        {
            this.gameObject = gameObject;
            this.menuPage = menuPage;
        }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public virtual void OnPageClose() { }

        public virtual void OnPageOpen() { }
    }
}
