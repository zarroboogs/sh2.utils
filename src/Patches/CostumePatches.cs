using Game.Common;
using Game.UI.Camp;
using HarmonyLib;
using MapNew;

namespace SoulHackers2Utils.Patches;

[HarmonyPatch]
public class CostumePatches
{
    [HarmonyPatch(typeof(CharacterManager), nameof(CharacterManager._ConvertFromConditionTable))]
    [HarmonyPostfix]
    private static void LoadDressPatch(ref CharacterManager.RequestParam prm)
    {
        if (!Plugin.Instance.GlobalCostumes.Value) return;
        if (!MapManager.IsActiveAreaDungeon) return;

        var shouldSetDress = prm.m_CharID switch
        {
            eChrId.PC_1001_Arrow => true,
            eChrId.PC_1002_Ringo => true,
            eChrId.PC_1004_Milady => true,
            eChrId.PC_1005_Psyzow => true,
            _ => false,
        };

        if (shouldSetDress && prm.m_ForceDressID <= 20)
        {
            var charData = PartyCtrl.Instance.GetCharData(prm.m_CharID);
            var equipData = charData.GetEquipData();
            var itemCosParam = ItemCtrl.GetItemCosParam((eItemId)equipData.CostumeId);

            prm.m_DressValue = itemCosParam.dressNum;
        }
    }

    [HarmonyPatch(typeof(MapCharaCtrlBase), nameof(MapCharaCtrlBase.Update))]
    [HarmonyPostfix]
    private static void SetDressPatch(MapCharaCtrlBase __instance)
    {
        if (!Plugin.Instance.GlobalCostumes.Value) return;
        if (!MapManager.IsActiveAreaDungeon) return;

        if (__instance.Chara == null) return;
        if (__instance.Chara.Model == null) return;

        bool isValidId = __instance.Chara.ChrID switch
        {
            eChrId.PC_1001_Arrow => true,
            eChrId.PC_1002_Ringo => true,
            eChrId.PC_1004_Milady => true,
            eChrId.PC_1005_Psyzow => true,
            _ => false,
        };

        bool isValidCtrlType = __instance.GetCtrlType() switch
        {
            eMapCharaCtrlType.Player_Field => true,
            eMapCharaCtrlType.Player_Dungeon => true,
            eMapCharaCtrlType.Companion => true,
            _ => false,
        };

        if (isValidId && isValidCtrlType &&
            __instance.Chara.Model.m_DressID <= 20 && !CampManager.IsOpen())
        {
            var charData = PartyCtrl.Instance.GetCharData(__instance.Chara.ChrID);
            if (charData == null) return;

            var equipData = charData.GetEquipData();
            if (equipData == null) return;

            var itemCosParam = ItemCtrl.GetItemCosParam((eItemId)equipData.CostumeId);
            if (itemCosParam == null) return;

            if (__instance.Chara.Model.m_DressID != itemCosParam.dressNum)
            {
                __instance.Chara.ChangeCharaDress(itemCosParam.dressNum);
                Util.ChangeCharaStyle(__instance, MapManager.PlayerStyle);
            }
        }
    }
}