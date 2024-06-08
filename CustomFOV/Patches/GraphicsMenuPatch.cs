using HarmonyLib;

namespace AetharNet.Mods.ZumbiBlocks2.CustomFOV.Patches;

[HarmonyPatch(typeof(GraphicsMenu))]
public static class GraphicsMenuPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(GraphicsMenu.ApplyGraphics))]
    public static void OnApplyChanges()
    {
        CustomFOV.SetFOV(CustomFOV.CurrentSliderFOV);
    }
}
