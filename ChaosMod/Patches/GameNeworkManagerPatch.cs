using ChaosMod.Activator;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal class GameNetworkManagerPatch
    {
        [HarmonyPatch("StartHost")]
        [HarmonyPostfix]
        static void StartHostPatch()
        {
            if (ChaosMod.getInstance().twitchOauthToken == null || ChaosMod.getInstance().twitchOauthUsername == null)
            {
                if (ChaosMod.ConfigActivator.Value == "twitch")
                {
                    try
                    {
                        Process.Start("http://localhost:8000");
                    }
                    catch { }
                }
                return;
            }
        }

        [HarmonyPatch("Disconnect")]
        [HarmonyPostfix]
        static void DisconnectPatch()
        {
            TimerSystem.Disable();
        }
    }
}
