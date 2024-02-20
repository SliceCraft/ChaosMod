using ChaosMod.Activator.Activators;
using ChaosMod.Activator;
using HarmonyLib;
using System;
using Unity.Netcode;
using UnityEngine;
using BepInEx.Configuration;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        private static KeyboardShortcut shortcut1 = new KeyboardShortcut(KeyCode.P, new KeyCode[] { KeyCode.LeftControl });
        private static KeyboardShortcut shortcut2 = new KeyboardShortcut(KeyCode.O, new KeyCode[] { KeyCode.LeftControl });

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
                TimerSystem.Enable();
                HUDManager.Instance.DisplayTip("Chaos Mod", "The mod has been reset");
            }

            if (shortcut2.IsDown())
            {
                TimerSystem.Disable();
                HUDManager.Instance.DisplayTip("Chaos Mod", "The mod has been turned off. Turn on with Ctrl + P");
            }

            ChaosMod.getInstance().twitchIRCClient?.Update();

            if(TimerSystem.GetActivator() != null && TimerSystem.GetActivator().getName().Equals("twitch"))
            {
                ((TwitchActivator)TimerSystem.GetActivator()).RefreshEffectText();
            }
        }
    }
}
