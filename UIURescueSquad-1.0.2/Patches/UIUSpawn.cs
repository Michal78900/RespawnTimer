using HarmonyLib;

namespace UIURescueSquad.Patches
{
    [HarmonyPatch(typeof(Respawning.RespawnTickets), nameof(Respawning.RespawnTickets.DrawRandomTeam))]
    class UIUSpawn
    {
        public static void Postfix(ref Respawning.SpawnableTeamType __result)
        {
            if (__result == Respawning.SpawnableTeamType.NineTailedFox)
            {
                UIURescueSquad.Instance.EventHandlers.IsSpawnable();
            }
        }
    }
}
