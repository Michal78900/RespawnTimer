using HarmonyLib;

namespace SerpentsHand.Patches
{
    [HarmonyPatch(typeof(Respawning.RespawnTickets), nameof(Respawning.RespawnTickets.DrawRandomTeam))]
    class UIUSpawn
    {
        public static void Postfix(ref Respawning.SpawnableTeamType __result)
        {
            SerpentsHand.instance.EventHandlers.CalculateChance();
        }
    }
}
