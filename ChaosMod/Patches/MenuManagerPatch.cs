using HarmonyLib;
using UnityEngine;

namespace ChaosMod.Patches
{
    [HarmonyPatch(typeof(MenuManager))]
    internal class MenuManagerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        static void AwakePatch()
        {
            if (ChaosMod.ConfigTwitchOptionsShowcase.Value == "newwindow")
            {
                Display.displays[1].Activate();
            }
        }
    }
}
