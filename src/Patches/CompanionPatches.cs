using Game.Common;
using HarmonyLib;
using MapNew;

namespace SoulHackers2Utils.Patches;

[HarmonyPatch]
public class CompanionPatches
{
    private static MapCharaCtrl_Companion _compArrow = null;
    private static MapCharaCtrl_Companion _compMilady = null;
    private static MapCharaCtrl_Companion _compSaizo = null;

    [HarmonyPatch(typeof(MapManager), nameof(MapManager.Update))]
    [HarmonyPostfix]
    private static void CompanionPatch(MapManager __instance)
    {
        if (!__instance.IsLoadedPlayer()) return;

        if (!Plugin.Instance.SpawnCompanions.Value) return;
        if (!MapManager.IsActiveAreaDungeon) return;

        AddCompanions();
    }

    private static void AddCompanions()
    {
        var comps = Util.GetCompanions();

        var isArrow = false;
        foreach (var comp in comps)
        {
            isArrow = comp.Chara.ChrID == eChrId.PC_1001_Arrow;
            if (isArrow) break;
        }

        if (!isArrow)
        {
            var pcRingo = MapCharaManager.Instance.GetPlayerChara();

            if (pcRingo == null) return;

            if (!PartyCtrl.Instance.IsPartyEnter(eChrId.PC_1001_Arrow)) return;

            _compArrow = MapManager.Instance.SpawnCompanion(eChrId.PC_1001_Arrow);
            _compArrow.SetupCompanion(pcRingo, true);
            Util.ChangeCharaStyle(_compArrow, MapManager.PlayerStyle);

            if (!PartyCtrl.Instance.IsPartyEnter(eChrId.PC_1004_Milady)) return;

            _compMilady = MapManager.Instance.SpawnCompanion(eChrId.PC_1004_Milady);
            _compMilady.SetupCompanion(_compArrow, true);
            Util.ChangeCharaStyle(_compMilady, MapManager.PlayerStyle);

            if (!PartyCtrl.Instance.IsPartyEnter(eChrId.PC_1005_Psyzow)) return;

            _compSaizo = MapManager.Instance.SpawnCompanion(eChrId.PC_1005_Psyzow);
            _compSaizo.SetupCompanion(_compMilady, true);
            Util.ChangeCharaStyle(_compSaizo, MapManager.PlayerStyle);
        }
    }

    private static void RemoveCompanions()
    {
        var comps = Util.GetCompanions();

        var isArrow = false;
        foreach (var comp in comps)
        {
            isArrow = comp.Chara.ChrID == eChrId.PC_1001_Arrow;
            if (isArrow) break;
        }

        if (isArrow)
        {
            MapManager.Instance.LeaveCompanion(eChrId.PC_1005_Psyzow);
            MapManager.Instance.LeaveCompanion(eChrId.PC_1004_Milady);
            MapManager.Instance.LeaveCompanion(eChrId.PC_1001_Arrow);

            _compSaizo = null;
            _compMilady = null;
            _compArrow = null;
        }
    }
}