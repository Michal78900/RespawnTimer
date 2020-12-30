using HarmonyLib;

namespace SerpentsHand.Patches
{
    [HarmonyPatch(typeof(Respawning.RespawnTickets), nameof(Respawning.RespawnTickets.DrawRandomTeam))]
    class SHSpawn
    {
        public static void Postfix(ref Respawning.SpawnableTeamType __result)
        {
            if(__result == Respawning.SpawnableTeamType.ChaosInsurgency)
            {
                EventHandlers.IsSpawnable();
            }
        }
    }
}
