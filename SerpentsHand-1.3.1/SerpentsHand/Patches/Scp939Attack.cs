using Exiled.API.Features;
using HarmonyLib;
using UnityEngine;

namespace SerpentsHand.Patches
{
	[HarmonyPatch(typeof(Scp939PlayerScript), nameof(Scp939PlayerScript.CallCmdShoot))]
	class Scp939Attack
	{
		public static void Postfix(Scp939PlayerScript __instance, GameObject target)
		{
			Player player = Player.Get(target);
			if (player.Role == RoleType.Tutorial && !SerpentsHand.instance.Config.FriendlyFire)
			{
				player.ReferenceHub.playerEffectsController.DisableEffect<CustomPlayerEffects.Amnesia>();
			}
		}
	}
}
