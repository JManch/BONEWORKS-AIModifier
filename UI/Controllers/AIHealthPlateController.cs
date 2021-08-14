using System;
using UnityEngine;
using TMPro;
using AIModifier.Utilities;

namespace AIModifier.UI
{
    class AIHealthPlateController : AIPlateController
    {
        public AIHealthPlateController(IntPtr ptr) : base(ptr) { }

        private float headPlateOffset;
        private float startHP;
        private float currentHP;
        private float barSpeed;
        private float barTarget;
        private Transform aiHead;
        private GameObject healthPlate;
        private TextMeshProUGUI hpValueText;

        private float headPlateWidth;
        private RectTransform hpBar;
        private RectTransform hpBarDelayed;

        protected override void Awake()
        {
            base.Awake();

            // Initialise vars
            healthPlate = GameObject.Instantiate(AssetManager.healthPlatePrefab);
            healthPlate.transform.SetParent(transform);
            headPlateWidth = healthPlate.GetComponent<RectTransform>().sizeDelta.x;

            // AIData
            aiHead = transform.FindChild(aiData.headPlateTransformChildPath);
            headPlateOffset = aiData.headPlateHeightOffset;

            // Text components
            AddTextComponents();
            hpValueText = healthPlate.transform.FindChild("HPValue").GetComponent<TextMeshProUGUI>();
            hpBar = healthPlate.transform.FindChild("HPBar").GetComponent<RectTransform>();
            hpBarDelayed = healthPlate.transform.FindChild("HPBarDelayed").GetComponent<RectTransform>();
        }

        public override void OnSpawn()
        {
            base.OnSpawn();

            startHP = aiBrain.behaviour.health.cur_hp;
            currentHP = aiBrain.behaviour.health.cur_hp;
            UpdateHealthBar(currentHP);
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
            healthPlate.transform.position = new Vector3(aiHead.transform.position.x, aiHead.transform.position.y + headPlateOffset, aiHead.transform.position.z);
            healthPlate.transform.rotation = Quaternion.Slerp(healthPlate.transform.rotation, Quaternion.LookRotation(healthPlate.transform.position - new Vector3(playerHead.transform.position.x, healthPlate.transform.position.y, playerHead.transform.position.z)), 5f * Time.deltaTime);

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
            TextMeshProUGUI hpValue = healthPlate.transform.FindChild("HPValue").gameObject.AddComponent<TextMeshProUGUI>();
            hpValue.fontSize = 1.5f;
            hpValue.enableWordWrapping = false;
            hpValue.characterSpacing = 5f;
            hpValue.text = "100";
            hpValue.alignment = TextAlignmentOptions.Center;
        }
    }
}
