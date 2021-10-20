using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using HarmonyLib;
using MEC;

namespace GrenadeMachineGun
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "GrenadeMachineGun";
        public override string Author => "Neil";
        private static Harmony _hInstance;

        public override void OnEnabled()
        {
            try
            {
                Harmony.DEBUG = true;
                _hInstance = new Harmony($"patchtests-{DateTime.Now.Ticks}");
                _hInstance.PatchAll();
            }
            catch (Exception ex)
            {
                Log.Error($"An exception has occured {ex}");
            }

            Timing.CallDelayed(5f, () =>
            {
                if (!new GrenadeMachineGun { Type = ItemType.GunLogicer }.TryRegister())
                    Log.Error("Something went wrong!");
            });

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            _hInstance?.UnpatchAll();
            _hInstance = null;

            base.OnDisabled();
        }
    }
}
