using System;
using UnityEngine;
using AIModifier.AI;

namespace AIModifier.UI
{
    public class PointSelector : Pointer
    {
        public PointSelector(IntPtr ptr) : base(ptr) { }

        public PointerManager<PointSelector> pointerManager { get; set; }

        protected override void OnPointerClick(RaycastHit pointerHit)
        {
            AIMenuFunctions.SetSelectedPoint(pointerHit.point);
        }
    }
}
