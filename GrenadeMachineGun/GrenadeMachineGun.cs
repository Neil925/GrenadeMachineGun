using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace GrenadeMachineGun
{
    public class GrenadeMachineGun : CustomWeapon
    {
        public override uint Id { get; set; } = 1;
        public override string Name { get; set; } = "GrenadeMachineGun";
        public override string Description { get; set; } = "Machine gun that fires grenades.";
        public override float Weight { get; set; }

        public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
        {
            Limit = 1,
            DynamicSpawnPoints = new List<DynamicSpawnPoint>
            {
                new DynamicSpawnPoint
                {
                    Chance = 100,
                    Location = SpawnLocation.InsideGateA
                }
            }
        };

        public override Modifiers Modifiers { get; set; } = default;
        public override float Damage { get; set; }
        public override byte ClipSize { get; set; } = 255;
        public static Dictionary<GameObject, ExplosiveGrenade> grenades = new Dictionary<GameObject, ExplosiveGrenade>();

        protected override void OnShooting(ShootingEventArgs ev)
        {
            ev.IsAllowed = false;

            ExplosiveGrenade grenade = new ExplosiveGrenade(ItemType.GrenadeHE);

            grenade.Base.FullThrowSettings.StartVelocity *= 4;
            grenade.Base.FullThrowSettings.UpwardsFactor = 0.1f;
            grenade.Base._weight = 0;
            grenade.BurnDuration = 0;
            grenade.ConcussDuration = 0;
            grenade.DeafenDuration = 0;

            grenades.Add(grenade.Base.gameObject, grenade);

            ev.Shooter.ThrowItem(grenade);

            Timing.CallDelayed(grenade.FuseTime, () => grenades.Remove(grenade.Base.gameObject));

            base.OnShooting(ev);
        }
    }
}
