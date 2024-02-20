using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ChaosMod.Patches;
using ChaosMod.Twitch;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaosMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class ChaosMod : BaseUnityPlugin
    {
        private const string modGUID = "SliceCraft.ChaosMod";
        private const string modName = "Chaos Mod";
        private const string modVersion = "1.0.0";

        public static ConfigEntry<int> ConfigTimeBetweenEffects;
        public static ConfigEntry<string> ConfigActivator;
        public static ConfigEntry<int> ConfigDelayBeforeSpawn;
        public static ConfigEntry<string> ConfigTwitchUsername;
        public static ConfigEntry<string> ConfigTwitchOAuthToken;
        public static ConfigEntry<string> ConfigTwitchOptionsShowcase;

        private readonly Harmony harmony = new Harmony(modGUID);

        private static ChaosMod Instance;

        internal ManualLogSource logsource;

        internal TwitchIRCClient twitchIRCClient;

        public static ChaosMod getInstance()
        {
            return Instance;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            logsource = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            
            harmony.PatchAll(typeof(ChaosMod));
            harmony.PatchAll(typeof(MenuManagerPatch));
            harmony.PatchAll(typeof(StartOfRoundPatch));
            harmony.PatchAll(typeof(PlayerControllerBPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));

            logsource.LogInfo("Chaos Mod loaded");
            
            ConfigTimeBetweenEffects = Config.Bind("Timer", "TimeBetweenEffects", 30, "The amount of time between choosing what effect to execute next.");
            ConfigActivator = Config.Bind("Timer", "Activator", "random", "The system that chooses how effects are chosen. Available activators: 'random', 'twitch'");
            ConfigDelayBeforeSpawn = Config.Bind("EnemySpawns", "DelayBeforeSpawn", 3, "The amount of seconds that will be waited when spawning enemies. Value can't be changed to above 10, if the value is above 10 it will still use 10 seconds. This it to prevent monsters from insta killing the player. This delay does not apply for every spawn effect, this does not apply to YIPPEEE for example.");
            ConfigTwitchUsername = Config.Bind("Twitch", "Username", "username", "The username of your Twitch account.");
            ConfigTwitchOAuthToken = Config.Bind("Twitch", "OAuthToken", "oauth", "The OAuth token to connect to your Twitch account. You can obtain this at: https://twitchapps.com/tmi/");
            ConfigTwitchOptionsShowcase = Config.Bind("Twitch", "OptionsShowcase", "onscreen", "The way voting options will be shown to the chat. Available options are: 'onscreen', 'chatmessage'");
        }
    }
}
