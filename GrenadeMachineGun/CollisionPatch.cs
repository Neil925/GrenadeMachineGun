using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using HarmonyLib;
using InventorySystem.Items.Pickups;
using Object = UnityEngine.Object;

namespace GrenadeMachineGun
{
    [HarmonyPatch(typeof(CollisionDetectionPickup), nameof(CollisionDetectionPickup.OnCollisionEnter))]
    internal class CollisionPatch
    {
        public static bool Prefix(CollisionDetectionPickup __instance)
        {
            //Log.Warn("--------");
            //Log.Info(__instance.Rb.gameObject.GetInstanceID());
            //foreach (var grenade in GrenadeMachineGun.grenades.Keys)
            //    Log.Debug(grenade.GetInstanceID() - 8);

            try
            {
                var item = GrenadeMachineGun.grenades.FirstOrDefault(g => g.Key.GetInstanceID() - 8 == __instance.Rb.gameObject.GetInstanceID());

                if (item.Value == null) return true;

                GrenadeMachineGun.grenades.Remove(item.Key);

                ExplosiveGrenade grenade = new ExplosiveGrenade(ItemType.GrenadeHE);

                grenade.FuseTime = 0.1f;
                grenade.BurnDuration = 0;
                grenade.ConcussDuration = 0;
                grenade.DeafenDuration = 0;

                grenade.SpawnActive(__instance.transform.position, Player.Get(__instance.PreviousOwner.Hub));

                __instance.DestroySelf();
            }
            catch (Exception e)
            {
                Log.Error($"An exception has occurred!: {e}");
                throw;
            }


            return false;
        }
    }
}
