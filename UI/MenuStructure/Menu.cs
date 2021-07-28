using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AIModifier.UI
{
    public class Menu
    {
        public MenuPointerController menuPointer { get; set; }
        public GameObject gameObject { get; private set; }
        public bool isOpen { get; private set; }
        public AudioSource audioSource { get; private set; }
        private Dictionary<string, MenuPage> pages;
        protected MenuPage activePage;

        public Menu(GameObject gameObject)
        {
            this.gameObject = gameObject;
            audioSource = gameObject.GetComponent<AudioSource>();
            pages = new Dictionary<string, MenuPage>();
        }

        public void AddPage(MenuPage page)
        {
            page.gameObject.SetActive(false);
            pages.Add(page.gameObject.name, page);
            page.menu = this;
            if (activePage == null)
            {
                activePage = page;
                activePage.gameObject.SetActive(true);
                isOpen = true;
            }
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
            foreach(MenuElement menuElement in GetPage(activePage.gameObject.name).elements.Values.ToList())
            {
                menuElement.OnPageClose();
            }
            activePage.gameObject.SetActive(false);
            activePage = pages[page];
            foreach (MenuElement menuElement in activePage.elements.Values.ToList())
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
