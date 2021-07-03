using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace AIModifier.UI
{
    public class Menu
    {
        public MenuPointerController menuPointer { get; set; }
        public GameObject gameObject { get; private set; }
        public bool isOpen { get; private set; }
        private Dictionary<string, MenuPage> pages;
        private MenuPage activePage;

        public Menu(GameObject gameObject, MenuPage startPage)
        {
            this.gameObject = gameObject;
            pages = new Dictionary<string, MenuPage>();
            pages.Add(startPage.gameObject.name, startPage);
            startPage.gameObject.SetActive(true);
            isOpen = true;
            activePage = startPage;
        }

        public void AddPage(MenuPage page)
        {
            page.gameObject.SetActive(false);
            pages.Add(page.gameObject.name, page);
        }

        public MenuPage GetPage(string pageName)
        {
            MenuPage menuPage;
            if (!pages.TryGetValue(pageName, out menuPage))
            {
                MelonLogger.Error("Menu page " + pageName + " could not be found");
            }
            return menuPage;
        }

        public void SwitchPage(string page)
        {
            // Call On Trigger Exit For All Elements on the current page
            foreach(MenuElement menuElement in new List<MenuElement>(GetPage(activePage.gameObject.name).elements.Values))
            {
                menuElement.OnPageClose();
            }
            activePage.gameObject.SetActive(false);
            activePage = pages[page];
            foreach (MenuElement menuElement in new List<MenuElement>(activePage.elements.Values))
            {
                menuElement.OnPageOpen();
            }
            pages[page].gameObject.SetActive(true);
        }

        public void OpenMenu()
        {
            isOpen = true;
            gameObject.SetActive(true);
        }

        public void CloseMenu()
        {
            isOpen = false;
            gameObject.SetActive(false);
        }
    }
}
