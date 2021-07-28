using System;
using System.Collections.Generic;
using UnityEngine;
using MelonLoader;

namespace AIModifier.UI
{
    public class MenuPage
    {
        public GameObject gameObject { get; private set; }
        public Menu menu { get; set; }

        public Dictionary<string, MenuElement> elements { get; private set; }

        public MenuPage(GameObject gameObject)
        {
            elements = new Dictionary<string, MenuElement>();
            this.gameObject = gameObject;
        }

        public void AddElement(MenuElement menuElement)
        {
            elements.Add(menuElement.gameObject.name , menuElement);
        }

        public MenuElement GetElement(string elementName)
        {
            MenuElement menuElement;
            if(!elements.TryGetValue(elementName, out menuElement))
            {
                MelonLogger.Error("Menu element " + elementName + " could not be found");
            }
            return menuElement;
        }
    }
}
