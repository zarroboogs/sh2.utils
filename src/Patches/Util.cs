using Il2CppSystem.Collections.Generic;
using MapNew;

namespace SoulHackers2Utils.Patches;

internal class Util
{
    public static List<MapCharaCtrl_Companion> GetCompanions()
    {
        var comps = new List<MapCharaCtrl_Companion>();
        MapCharaManager.Instance.GetMapCharaCtrlTypeAll(
            comps.Cast<ICollection<MapCharaCtrl_Companion>>(), eMapCharaCtrlType.Companion);
        return comps;
    }

    public static void ChangeCharaStyle(MapCharaCtrlBase mccb, eMapPlayerStyle style)
    {
        if (mccb == null) return;
        if (mccb.Chara == null) return;

        mccb.Chara.PlayAnimation(eMapCharaAnimState.Idle, style == eMapPlayerStyle.Dungeon, false, false);

        if (style == eMapPlayerStyle.Field)
        {
            mccb.Chara.SetActiveWeapon(false);
        }
        else if (style == eMapPlayerStyle.Dungeon)
        {
            var mountObject = mccb.Chara.GetMountObject(eMapCharaMountLocator.RightHand);

            if (mountObject == null) return;

            if (!mountObject.IsLoaded())
            {
                mccb.Chara.MountWeapon();
            }

            mccb.Chara.SetActiveWeapon(true);
        }
    }
}