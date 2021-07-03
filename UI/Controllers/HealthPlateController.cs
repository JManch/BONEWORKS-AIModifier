using UnityEngine;
using System;
using StressLevelZero.AI;
using ModThatIsNotMod;
using AIModifier.Utilities;
using TMPro;
using MelonLoader;
using AIModifier.AI;

namespace AIModifier.UI
{
    class HealthPlateController : MonoBehaviour
    {
        public HealthPlateController(IntPtr ptr) : base(ptr) { }

        private float healthPlateOffset;
        private float startHP;
        private float currentHP;
        private float barSpeed;
        private float barTarget;
        private AIBrain aiBrain;
        private Transform playerHead;
        private Transform aiHead;
        private TextMeshProUGUI hpValueText;
        private RectTransform hpBar;

        void Awake()
        {
            // Initialise vars
            aiBrain = transform.parent.GetComponent<AIBrain>();
            startHP = aiBrain.behaviour.health.cur_hp;
            currentHP = aiBrain.behaviour.health.cur_hp;
            playerHead = Player.GetPlayerHead().transform;

            // AIData
            AIData aiData = AIDataManager.aiDataDictionary[SimpleHelpers.GetCleanObjectName(aiBrain.gameObject.name)];
            aiHead = transform.parent.FindChild(aiData.headPlateTransformChildPath);
            healthPlateOffset = aiData.headPlateHeightOffset;

            // Text components
            AddTextComponents();
            hpValueText = transform.FindChild("HPValue").GetComponent<TextMeshProUGUI>();
            hpBar = transform.FindChild("HPBar").GetComponent<RectTransform>();
        }

        void Start()
        {
            UpdateHealthPlate(currentHP);
        }

        void Update()
        {
            // If the current HP exceeds the max HP update the max
            if (aiBrain.behaviour.health.cur_hp > startHP)
            {
                startHP = aiBrain.behaviour.health.cur_hp;
            }

            // If the HP has changed update health bar
            if (aiBrain.behaviour.health.cur_hp != currentHP)
            {
                barTarget = -((startHP - aiBrain.behaviour.health.cur_hp) / startHP * 0.6f);
                barSpeed = (barTarget - hpBar.offsetMax.x) * 3;
                UpdateHealthPlate(aiBrain.behaviour.health.cur_hp);
            }

            // Update health plate position and rotation
            transform.position = new Vector3(aiHead.transform.position.x, aiHead.transform.position.y + healthPlateOffset, aiHead.transform.position.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - new Vector3(playerHead.transform.position.x, transform.position.y, playerHead.transform.position.z)), 5f * Time.deltaTime);

            // Animate health bar movement
            if(hpBar.offsetMax.x > barTarget)
            {
                hpBar.offsetMax = new Vector2(hpBar.offsetMax.x + barSpeed * Time.deltaTime, hpBar.offsetMax.y);
            }
            else if(hpBar.offsetMax.x != barTarget)
            {
                hpBar.offsetMax = new Vector2(barTarget, hpBar.offsetMax.y);
            }

            currentHP = aiBrain.behaviour.health.cur_hp;
        }

        public void UpdateHealthPlate(float newHealth)
        {
            if(newHealth == 0)
            {
                hpValueText.text = "DEAD";
                hpBar.offsetMax = new Vector2(0f, hpBar.offsetMax.y);
                barTarget = 0f;
                barSpeed = 0f;
            }
            else
            {
                hpValueText.text = ((int)(newHealth * 100)).ToString();
            }
        }

        private void AddTextComponents()
        {
            TextMeshProUGUI hpValue = transform.FindChild("HPValue").gameObject.AddComponent<TextMeshProUGUI>();
            hpValue.fontSize = 1.5f;
            hpValue.enableWordWrapping = false;
            hpValue.characterSpacing = 5f;
            hpValue.text = "100";
            hpValue.alignment = TextAlignmentOptions.Center;
        }
    }
}
