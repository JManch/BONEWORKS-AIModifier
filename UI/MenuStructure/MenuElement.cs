using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIModifier.UI
{
    public abstract class MenuElement
    {
        public GameObject gameObject { get; private set;}

        public MenuElement(GameObject gameObject) 
        {
            this.gameObject = gameObject;
        }

        public abstract object GetValue();

        public abstract void SetValue(object value);

        public virtual void OnPageClose() { }

        public virtual void OnPageOpen() { }
    }
}
