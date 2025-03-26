using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace JustRetryPlus
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger = null!;

        private static ConfigEntry<bool> _shouldResetLevel = null!;
        public static bool ShouldResetLevel => _shouldResetLevel.Value;
        
        private static ConfigEntry<bool> _shouldHealAfterFail = null!;
        public static bool ShouldHealAfterFail => _shouldHealAfterFail.Value;

        public static bool DidFailLevel { get; set; } = false;

        private static ConfigEntry<bool> _isEnabled = null!;
        public static bool IsEnabled => _isEnabled.Value;
        
        private static ConfigEntry<int> _healthToHeal = null!;
        public static int HealthToHealValue => _healthToHeal.Value;
        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
            Logger.LogInfo($"Thanks for nickklmao for the original Idea and bits of code! please check out their mods on Thunderstore!");
            Logger.LogInfo($"Without them this mod wouldn't be possible!");
           
            var harmony = new Harmony("eu.xaru.justretryplus");
            harmony.PatchAll();
            Logger.LogInfo("Harmony patches applied.");
            
            _isEnabled = Config.Bind("Settings", "IsEnabled", true,
                new ConfigDescription("Toggle if the plugin should be enabled, if false the normal game death scene will take over", new AcceptableValueRange<bool>(false, true)));
            
            _shouldResetLevel = Config.Bind("Settings", "ShouldResetLevel", false,
                new ConfigDescription("Toggle if the game should restart at level one & reset the map when failing a level", new AcceptableValueRange<bool>(false, true)));

            _shouldHealAfterFail = Config.Bind("Settings", "ShouldHealAfterFail", true,
                new ConfigDescription("Toggle if the game should restart with full health when failing a level", new AcceptableValueRange<bool>(false, true)));
            
            _healthToHeal = Config.Bind("Settings", "HealthToHeal", 100,
                new ConfigDescription("The amount of health to heal when failing a level", new AcceptableValueRange<int>(0, 500)));
        }

        private void Start() => Logger.LogInfo("JustRetryPlus started!");
     
    }

    [HarmonyPatch(typeof(RunManager), "ChangeLevel")]
    public static class ChangeLevelPatch
    {
        static void Prefix(
            RunManager __instance, 
            ref bool _completedLevel,
            ref bool _levelFailed,
            ref RunManager.ChangeLevelType _changeLevelType)
        {
            if(!Plugin.IsEnabled) return;
            Plugin.Logger.LogInfo($"Level failed?: {_levelFailed}");
            Plugin.DidFailLevel = _levelFailed;
            if(!Plugin.DidFailLevel) return; 
            if(Plugin.ShouldResetLevel)
            {
                Plugin.Logger.LogInfo("Restarting at level one");
                __instance.ResetProgress(); // reset progress
                __instance.SetRunLevel(); // rotate levels
            }

            Plugin.Logger.LogInfo("Level failed, retrying...");
            __instance.RestartScene();
            Plugin.Logger.LogInfo("Restarted scene");
        }
        
    }
    
    [HarmonyPatch(typeof(RoundDirector), "StartRoundLogic")]
    public static class RoundDirectorPatch
    {
        static void Postfix(RoundDirector __instance)
        {
            if(!Plugin.IsEnabled) return;
            if (!Plugin.ShouldHealAfterFail || !Plugin.DidFailLevel || !SemiFunc.IsMasterClientOrSingleplayer()) return;
            foreach (var item in from player in SemiFunc.PlayerGetAll() select player.playerHealth)
            {
                if (item == null) continue;
                item.HealOther(Plugin.HealthToHealValue, true);
            }
        }
        
        
    }
    
}