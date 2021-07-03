using UnityEngine;
using TMPro;
using AIModifier.Utilities;

namespace AIModifier.UI
{
    public static class MenuManager
    {
        public static GameObject aiMenu;

        public static void BuildAIMenu()
        {
            aiMenu = Object.Instantiate(Util.aiMenuPrefab);
            
            TextMeshProUGUI aiModifierTitle = aiMenu.transform.FindChild("RootPage/AIModifierTitle").gameObject.AddComponent<TextMeshProUGUI>();
            aiModifierTitle.text = "AIModifier";
            aiModifierTitle.fontSize = 3;
            aiModifierTitle.alignment = TextAlignmentOptions.Center;

            TextMeshProUGUI globalAISettingsText = aiMenu.transform.FindChild("RootPage/GlobalSettingsButton/GlobalAISettingsText").gameObject.AddComponent<TextMeshProUGUI>();
            globalAISettingsText.text = "Global AI Settings";
            globalAISettingsText.fontSize = 9;
            globalAISettingsText.alignment = TextAlignmentOptions.Center;

            TextMeshProUGUI individualAISettingsText = aiMenu.transform.FindChild("RootPage/GlobalSettingsButton/IndividualAISettingsText").gameObject.AddComponent<TextMeshProUGUI>();
            individualAISettingsText.text = "Individual AI Settings";
            individualAISettingsText.fontSize = 9;
            individualAISettingsText.alignment = TextAlignmentOptions.Center;
        }
    }
}
