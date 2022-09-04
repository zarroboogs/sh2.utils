using HarmonyLib;
using UnityEngine;

namespace SoulHackers2Utils.Patches;

[HarmonyPatch]
internal class MiscPatches
{
    [HarmonyPatch(typeof(Application), nameof(Application.runInBackground), MethodType.Setter)]
    [HarmonyPrefix]
    private static void BackgroundPatch(ref bool value)
    {
        if (!Plugin.Instance.RunInBackground.Value) return;

        value = true;
    }
}