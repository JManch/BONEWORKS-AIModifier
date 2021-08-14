using MelonLoader;

namespace AIModifier.Utilities
{
    public static class UserPreferences
    {
        private static MelonPreferences_Category userPreferences;
        public static MelonPreferences_Entry<bool> healthBars;

        public static bool MenuFollowsPlayer = true;

        public static void Initialise()
        {
            userPreferences = MelonPreferences.CreateCategory("AIModifier");
            healthBars = userPreferences.CreateEntry("HealthBarsEnabled", true);
            MelonPreferences.Save();
        }
    }
}
