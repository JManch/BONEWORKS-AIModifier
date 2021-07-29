using System;
using UnityEngine;
using UnityEngine.UI;
using AIModifier.Utilities;

namespace AIModifier.UI
{
    public class AISelectedPlateController : AIPlateController
    {
        public AISelectedPlateController(IntPtr ptr) : base(ptr) { }

        private GameObject selectedPlate;
        private Image plateImage;
        private Color standardColor;
        private Color targetColor = Color.red;

        private Transform aiHead;
        private float headPlateOffset;

        public enum SelectedType 
        {
            Standard,
            Target
        }

        protected override void Awake()
        {
            base.Awake();

            selectedPlate = GameObject.Instantiate(AssetManager.selectedPlatePrefab);
            selectedPlate.transform.SetParent(transform);
            plateImage = selectedPlate.transform.FindChild("Selected").GetComponent<Image>();
            standardColor = plateImage.color;

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

        public void EnableSelectedIcon(SelectedType selectedType)
        {
            if(selectedType == SelectedType.Standard)
            {
                plateImage.color = standardColor;
            }
            else
            {
                plateImage.color = targetColor;
            }

            selectedPlate.SetActive(true);
        }
        public void DisableSelectedIcon()
        {
            selectedPlate.SetActive(false);
        }
    }
}
