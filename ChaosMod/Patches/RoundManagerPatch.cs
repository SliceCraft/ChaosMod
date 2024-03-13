using ChaosMod.Activator.Activators;
using ChaosMod.Activator;
using HarmonyLib;
using System;
using Unity.Netcode;
using UnityEngine;
using BepInEx.Configuration;
using System.Diagnostics;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        private static KeyboardShortcut shortcut1 = new KeyboardShortcut(KeyCode.P, new KeyCode[] { KeyCode.LeftControl });
        private static KeyboardShortcut shortcut2 = new KeyboardShortcut(KeyCode.O, new KeyCode[] { KeyCode.LeftControl });
        private static KeyboardShortcut shortcut3 = new KeyboardShortcut(KeyCode.I, new KeyCode[] { KeyCode.LeftControl });

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void UpdatePatch()
        {
            TimerSystem.Update();
            bool isNotGoingToCompany = StartOfRound.Instance.currentLevelID != 3;
            bool isPlayerNotDead = !GameNetworkManager.Instance.localPlayerController.isPlayerDead;
            bool isNotInShip = !StartOfRound.Instance.inShipPhase;
            if (shortcut1.IsDown() && isNotGoingToCompany && isPlayerNotDead && isNotInShip && GameNetworkManager.Instance.isHostingGame)
            {
                TimerSystem.Disable();
                HUDManager.Instance.DisplayTip("Chaos Mod", "The mod has been reset");
                TimerSystem.Enable();
            }

            if (shortcut2.IsDown())
            {
                TimerSystem.Disable();
                HUDManager.Instance.DisplayTip("Chaos Mod", "The mod has been turned off. Turn on with Ctrl + P");
            }
            if (shortcut3.IsDown() && ChaosMod.ConfigActivator.Value == "twitch")
            {
                try
                {
                    Process.Start("http://localhost:8000");
                }
                catch { }
            }

            ChaosMod.getInstance().twitchIRCClient?.Update();

            if(TimerSystem.GetActivator() != null && TimerSystem.GetActivator().getName().Equals("twitch"))
            {
                ((TwitchActivator)TimerSystem.GetActivator()).RefreshEffectText();
            }
        }
    }
}
