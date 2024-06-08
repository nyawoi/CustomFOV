using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks2.CustomFOV.Patches;

[HarmonyPatch(typeof(MainCamera))]
public static class MainCameraPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MainCamera.Init))]
    public static void InitializeFOV()
    {
        CustomFOV.SetFOV(CustomFOV.BaseFOV);
    }
}
