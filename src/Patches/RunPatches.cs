using AtLib.AtInput;
using Game.Common;
using HarmonyLib;
using MapNew;
using UnityEngine.InputSystem;

namespace SoulHackers2Utils.Patches;

[HarmonyPatch]
internal class RunPatches
{
    private static bool _isSpeedSet = false;

    private static float _savedSpeedValue = 1f;
    private static bool _savedSpeedEffect = false;

    [HarmonyPatch(typeof(MapCharaCtrlBase), nameof(MapCharaCtrlBase.Update))]
    [HarmonyPostfix]
    private static void RunPatch(MapCharaCtrlBase __instance)
    {
        if (Plugin.Instance.RunSpeed.Value == 1f) return;

        if (!_isSpeedSet)
        {
            _savedSpeedEffect = MapManager.SkillParams.m_IsAttachGaleEffect;
        }

        if (__instance.Chara == null) return;
        if (__instance.Chara.ChrID != eChrId.PC_1002_Ringo) return;

        var shouldRun = false;
        var padData = AtPadManager.GetPadData();

        if (!shouldRun && padData != null) shouldRun = padData.IsHold(eAtButton.R2);
        if (!shouldRun && Keyboard.current != null) shouldRun = Keyboard.current[Key.LeftShift].isPressed;

        if (!_isSpeedSet && shouldRun)
        {
            _isSpeedSet = true;

            _savedSpeedValue = __instance.Chara.GetAnimationSpeed(eMapAnimSpeedCategory.System);

            MapManager.SkillParams.m_IsAttachGaleEffect = Plugin.Instance.RunEffect.Value;
            SetAnimationSpeed(__instance, Plugin.Instance.RunSpeed.Value);

            foreach (var comp in Util.GetCompanions())
            {
                SetAnimationSpeed(comp, Plugin.Instance.RunSpeed.Value);
            }
        }
        else if (_isSpeedSet && !shouldRun)
        {
            _isSpeedSet = false;

            MapManager.SkillParams.m_IsAttachGaleEffect = _savedSpeedEffect;
            SetAnimationSpeed(__instance, _savedSpeedValue);

            foreach (var comp in Util.GetCompanions())
            {
                SetAnimationSpeed(comp, _savedSpeedValue);
            }
        }
    }

    private static void SetAnimationSpeed(MapCharaCtrlBase mccb, float speed)
    {
        if (mccb == null) return;
        if (mccb.Chara == null) return;

        mccb.Chara.SetAnimationSpeed(speed, eMapAnimSpeedCategory.System);
    }
}