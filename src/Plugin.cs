using AtLib.AtInput;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace SoulHackers2Utils;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("SOUL HACKERS2.exe")]
public class Plugin : BasePlugin
{
    public static Plugin Instance { get; private set; }
    public new ManualLogSource Log { get; private set; }

    public ConfigEntry<bool> RunInBackground { get; set; }

    public ConfigEntry<bool> GlobalCostumes { get; set; }
    public ConfigEntry<bool> SpawnCompanions { get; set; }

    public ConfigEntry<float> RunSpeed { get; set; }
    public ConfigEntry<bool> RunEffect { get; set; }
    public ConfigEntry<Key> RunInputKey { get; set; }
    public ConfigEntry<eAtButton> RunInputPad { get; set; }

    public override void Load()
    {
        Instance = this;
        Log = base.Log;

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        InitConfig();

        var harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }

    private void InitConfig()
    {
        Config.SaveOnConfigSet = true;

        RunInBackground = Config.Bind("Misc", "RunInBackground", false);

        GlobalCostumes = Config.Bind("Game", "GlobalCostumes", false);

        SpawnCompanions = Config.Bind("Game", "SpawnCompanions", false);

        RunSpeed = Config.Bind("Run", "RunSpeed", 2f,
            new ConfigDescription(
                "1 - disabled",
                new AcceptableValueRange<float>(1f, 3f)));

        RunEffect = Config.Bind("Run", "RunEffect", true);

        RunInputKey = Config.Bind("Run", "RunInputKey", Key.LeftShift);

        RunInputPad = Config.Bind("Run", "RunInputPad", eAtButton.R2);
    }
}