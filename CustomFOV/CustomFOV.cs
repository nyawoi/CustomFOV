using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks2.CustomFOV;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public class CustomFOV : BaseUnityPlugin
{
    public const string PluginGUID = "AetharNet.Mods.ZumbiBlocks2.CustomFOV";
    public const string PluginAuthor = "wowi";
    public const string PluginName = "CustomFOV";
    public const string PluginVersion = "0.1.0";

    internal new static ManualLogSource Logger;
    private new static ConfigFile Config;

    internal static int CurrentSliderFOV;
    
    public static int BaseFOV => configBaseFOV.Value;
    public static int MinFOV => configMinValue.Value;
    public static int MaxFOV => configMaxValue.Value;
    
    private static ConfigEntry<int> configBaseFOV;
    private static ConfigEntry<int> configMinValue;
    private static ConfigEntry<int> configMaxValue;
    
    private void Awake()
    {
        Logger = base.Logger;
        Config = base.Config;

        configBaseFOV = Config.Bind(
            "FOVController",
            "BaseFOV",
            70,
            "Default camera field of view, prior to any zoom factors");

        configMinValue = Config.Bind(
            "GraphicsMenu",
            "MinFOV",
            50,
            "The minimum field of view value allowed on the slider");
        
        configMaxValue = Config.Bind(
            "GraphicsMenu",
            "MaxFOV",
            120,
            "The maximum field of view value allowed on the slider");
        
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGUID);
    }

    public static void SetFOV(int currentFOV)
    {
        // If the selected FOV is a new value, save it
        if (configBaseFOV.Value != currentFOV)
        {
            configBaseFOV.Value = currentFOV;
            Config.Save();
        }
        
        // MainCamera.instance.defaultFOV is not read by any other class,
        // but we'll modify it in case it becomes more prominent in future updates
        MainCamera.instance.defaultFOV = currentFOV;
        // The camera's field of view is set from the above defaultFOV
        MainCamera.instance.cam.fieldOfView = currentFOV;
        // The FOVController is the main class that all other methods depend upon
        MainCamera.instance.fovController.baseFOV = currentFOV;
    }
}
