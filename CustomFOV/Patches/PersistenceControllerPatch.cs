using System.Globalization;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AetharNet.Mods.ZumbiBlocks2.CustomFOV.Patches;

[HarmonyPatch(typeof(PersistenceController))]
public static class PersistenceControllerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PersistenceController.Init))]
    public static void AddFOVSlider(PersistenceController __instance)
    {
        // Clone the FPS slider to create an FOV slider
        var fpsSliderGameObject = __instance.graphicsMenu.gameObject.transform.Find("Menu/Body/Panel/SettingAnimation (1)");
        var fovSliderGameObject = Object.Instantiate(fpsSliderGameObject, fpsSliderGameObject.parent);

        // Retrieve the FOV slider's label, logic component, and display
        // The current tree looks like so:
        // - (0) Label
        // - (1) Slider
        // - (2) LabelBox
        //   - (0) Label
        var fovSliderLabel = fovSliderGameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        var fovSliderTranslation = fovSliderGameObject.transform.GetChild(0).gameObject.GetComponent<TranslatedTMPText>();
        var fovSliderSlider = fovSliderGameObject.transform.GetChild(1).gameObject.GetComponent<Slider>();
        var fovSliderDisplay = fovSliderGameObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        // Change the GameObject's name for easier debugging using UnityExplorer
        fovSliderGameObject.name = "SettingFieldOfView";
        // Change the slider's label directly (translations will be done later)
        fovSliderTranslation.displayTag = "Field of View";
        fovSliderLabel.text = "Field of View";
        // Set the minimum and maximum allowed values
        fovSliderSlider.minValue = CustomFOV.MinFOV;
        fovSliderSlider.maxValue = CustomFOV.MaxFOV;
        // Add a listener to the slider's onValueChanged event
        fovSliderSlider.onValueChanged.AddListener(delegate
        {
            fovSliderDisplay.text = fovSliderSlider.value.ToString(CultureInfo.InvariantCulture);
            CustomFOV.CurrentSliderFOV = (int)fovSliderSlider.value;
        });
        // Set the slider's value
        fovSliderSlider.value = CustomFOV.BaseFOV;
    }
}
