using System;
using UnityEngine;
using TMPro;

namespace AIModifier.UI
{
    class AIHeadPlateController : AIPlateController
    {
        public AIHeadPlateController(IntPtr ptr) : base(ptr) { }

        private float headPlateOffset;
        private float startHP;
        private float currentHP;
        private float barSpeed;
        private float barTarget;
        private Transform aiHead;
        private GameObject headPlate;
        private TextMeshProUGUI hpValueText;

        private float headPlateWidth;
        private RectTransform hpBar;
        private RectTransform hpBarDelayed;

        private GameObject selected;

        protected override void Awake()
        {
            base.Awake();

            // Initialise vars
            startHP = aiBrain.behaviour.health.cur_hp;
            currentHP = aiBrain.behaviour.health.cur_hp;
            headPlate = GameObject.Instantiate(Utilities.Utilities.headPlatePrefab);
            headPlate.transform.SetParent(transform);
            headPlateWidth = headPlate.GetComponent<RectTransform>().sizeDelta.x;

            // AIData
            aiHead = transform.FindChild(aiData.headPlateTransformChildPath);
            headPlateOffset = aiData.headPlateHeightOffset;

            // Text components
            AddTextComponents();
            hpValueText = headPlate.transform.FindChild("HPValue").GetComponent<TextMeshProUGUI>();
            hpBar = headPlate.transform.FindChild("HPBar").GetComponent<RectTransform>();
            hpBarDelayed = headPlate.transform.FindChild("HPBarDelayed").GetComponent<RectTransform>();
            selected = headPlate.transform.FindChild("Selected").gameObject;
        }

        void Start()
        {
            UpdateHealthBar(currentHP);
            selected.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();

            // If the current HP exceeds the max HP update the max
            if (aiBrain.behaviour.health.cur_hp > startHP)
            {
                startHP = aiBrain.behaviour.health.cur_hp;
            }

            // If the HP has changed update health bar
            if (aiBrain.behaviour.health.cur_hp != currentHP)
            {
                barTarget = -((startHP - aiBrain.behaviour.health.cur_hp) / startHP * 0.6f);
                barSpeed += (barTarget - hpBar.offsetMax.x) * 3;
                UpdateHealthBar(aiBrain.behaviour.health.cur_hp);
            }

            // Update health plate position and rotation
            headPlate.transform.position = new Vector3(aiHead.transform.position.x, aiHead.transform.position.y + headPlateOffset, aiHead.transform.position.z);
            headPlate.transform.rotation = Quaternion.Slerp(headPlate.transform.rotation, Quaternion.LookRotation(headPlate.transform.position - new Vector3(playerHead.transform.position.x, headPlate.transform.position.y, playerHead.transform.position.z)), 5f * Time.deltaTime);

            // Animate delayed health bar movement
            if (hpBarDelayed.offsetMax.x > barTarget)
            {
                hpBarDelayed.offsetMax = new Vector2(hpBarDelayed.offsetMax.x + barSpeed * Time.deltaTime, hpBarDelayed.offsetMax.y);
            }
            else
            {
                hpBarDelayed.offsetMax = new Vector2(barTarget, hpBarDelayed.offsetMax.y);
                barSpeed = 0;
            }

            currentHP = aiBrain.behaviour.health.cur_hp;
        }

        private void UpdateHealthBar(float newHealth)
        {
            if (newHealth == 0)
            {
                hpValueText.text = "DEAD";
                hpBar.offsetMax = new Vector2(0f, hpBar.offsetMax.y);
                hpBarDelayed.offsetMax = new Vector2(0f, hpBarDelayed.offsetMax.y);
                hpBarDelayed.offsetMin = new Vector2(headPlateWidth, hpBarDelayed.offsetMin.y);
                barTarget = 0f;
                barSpeed = 0f;
            }
            else
            {
                hpBarDelayed.offsetMin = new Vector2(headPlateWidth + barTarget, hpBarDelayed.offsetMin.y);
                hpBar.offsetMax = new Vector2(barTarget, hpBar.offsetMax.y);
                hpValueText.text = ((int)Math.Ceiling(newHealth * 100)).ToString();
            }
        }

        private void AddTextComponents()
        {
            TextMeshProUGUI hpValue = headPlate.transform.FindChild("HPValue").gameObject.AddComponent<TextMeshProUGUI>();
            hpValue.fontSize = 1.5f;
            hpValue.enableWordWrapping = false;
            hpValue.characterSpacing = 5f;
            hpValue.text = "100";
            hpValue.alignment = TextAlignmentOptions.Center;
        }

        public void EnableSelectedIcon()
        {
            selected.SetActive(true);
        }

        public void DisableSelectedIcon()
        {
            selected.SetActive(false);
        }
    }
}
