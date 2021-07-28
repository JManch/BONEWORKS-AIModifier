using System;
using UnityEngine;
using AIModifier.Utilities;

namespace AIModifier.UI
{
    class AISelectedPlateController : AIPlateController
    {
        public AISelectedPlateController(IntPtr ptr) : base(ptr) { }

        private GameObject selectedPlate;

        private Transform aiHead;
        private float headPlateOffset;

        protected override void Awake()
        {
            base.Awake();

            selectedPlate = GameObject.Instantiate(AssetManager.selectedPlatePrefab);
            selectedPlate.transform.SetParent(transform);

            aiHead = transform.FindChild(aiData.headPlateTransformChildPath);
            headPlateOffset = aiData.headPlateHeightOffset;
            DisableSelectedIcon();
        }

        protected override void Update()
        {
            base.Update();

            // Update selected plate position and rotation
            if(selectedPlate.activeInHierarchy)
            {
                selectedPlate.transform.position = new Vector3(aiHead.transform.position.x, aiHead.transform.position.y + headPlateOffset, aiHead.transform.position.z);
                selectedPlate.transform.rotation = Quaternion.Slerp(selectedPlate.transform.rotation, Quaternion.LookRotation(selectedPlate.transform.position - new Vector3(playerHead.transform.position.x, selectedPlate.transform.position.y, playerHead.transform.position.z)), 5f * Time.deltaTime);
            }
        }

        public void EnableSelectedIcon()
        {
            selectedPlate.SetActive(true);
        }
        public void DisableSelectedIcon()
        {
            selectedPlate.SetActive(false);
        }
    }
}
