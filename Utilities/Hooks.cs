using StressLevelZero.Zones;
using StressLevelZero.Props.Weapons;
using PuppetMasta;
using HarmonyLib;
using ModThatIsNotMod;

namespace AIModifier.Utilities
{
    class Hooks
    {
        public static void CreateHooks()
        {
            Hooking.CreateHook(typeof(ZoneSpawner).GetMethod("Spawn", AccessTools.all), typeof(AI.AIManager).GetMethod("OnZoneSpawnerSpawn", AccessTools.all), false);
            Hooking.CreateHook(typeof(SpawnGun).GetMethod("OnFire", AccessTools.all), typeof(AI.AIManager).GetMethod("OnFireSpawnGun", AccessTools.all), false);
        }
    }
}
