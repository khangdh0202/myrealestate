using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureContent : MonoBehaviour
{
    Toggle borderContructionCheckBox;
    Toggle snapToConnerCheckBox;
    Toggle snapToAngleCheckBox;
    Slider borderWidthSlider;
    Toggle autoSaving;

    private void Start()
    {
        borderContructionCheckBox = transform.Find("BorderContruction").Find("CheckBox").GetComponent<Toggle>();
        snapToConnerCheckBox = transform.Find("SnaptoConner").Find("CheckBox").GetComponent<Toggle>();
        snapToAngleCheckBox = transform.Find("SnaptoAngle").Find("CheckBox").GetComponent<Toggle>();
        borderWidthSlider = transform.Find("BorderWidth").Find("Slider").GetComponent<Slider>();
        autoSaving = transform.Find("AutoSaving").Find("CheckBox").GetComponent<Toggle>();

        borderContructionCheckBox.isOn = SettingManager.Instance.borderContructionView;
        snapToAngleCheckBox.isOn = SettingManager.Instance.SnapToAngle;
        snapToConnerCheckBox.isOn = SettingManager.Instance.snapToConner;
        borderWidthSlider.value = Mathf.InverseLerp(SettingManager.Instance.minSizeBorder, SettingManager.Instance.maxSizeBorder, SettingManager.Instance.borderContructionWidth);
        autoSaving.isOn = SaveGame.instance.autoSaving;

        borderContructionCheckBox.onValueChanged.AddListener(delegate { BorderContructionCheckBoxChanged(); });
        snapToConnerCheckBox.onValueChanged.AddListener(delegate { SnapToConnerCheckBoxChanged(); });
        snapToAngleCheckBox.onValueChanged.AddListener(delegate { SnapToAngleCheckBoxChanged(); });
        borderWidthSlider.onValueChanged.AddListener(delegate { BorderWidthSliderChanged(); });
        autoSaving.onValueChanged.AddListener(delegate { AutoSavingCheckBoxChanged(); });
    }
    private void BorderWidthSliderChanged()
    {
        SettingManager.Instance.borderContructionWidth = ConvertSliderValue(borderWidthSlider.value, SettingManager.Instance.minSizeBorder, SettingManager.Instance.maxSizeBorder);
        
        List<Component> foundComponents = new List<Component>();
        foreach (var component in FindObjectsOfType<Component>())
        {
            if (component.GetType().Name == "BoundingBoxBuilding")
            {
                foundComponents.Add(component);
            }
        }

        foreach (BoundingBoxBuilding value in foundComponents)
        {
            value.DrawBorderBuilding(SettingManager.Instance.borderContructionWidth);
        }
    }

    private void SnapToAngleCheckBoxChanged()
    {
        SettingManager.Instance.SnapToAngle = !SettingManager.Instance.SnapToAngle;
    }

    private void BorderContructionCheckBoxChanged()
    {
        SettingManager.Instance.borderContructionView = !SettingManager.Instance.borderContructionView;

        List<Component> foundComponents = new List<Component>();
        foreach (var component in FindObjectsOfType<Component>())
        {
            if (component.GetType().Name == "BoundingBoxBuilding")
            {
                foundComponents.Add(component);
            }
        }

        foreach (BoundingBoxBuilding value in foundComponents)
        {
            value.DrawBorderBuilding(SettingManager.Instance.borderContructionWidth);
        }
    } 

    private void SnapToConnerCheckBoxChanged()
    {
        SettingManager.Instance.snapToConner = !SettingManager.Instance.snapToConner;
    }

    float ConvertSliderValue(float sliderValue, float minValue, float maxValue)
    {
        // Đảm bảo sliderValue nằm trong khoảng từ 0 đến 1
        sliderValue = Mathf.Clamp01(sliderValue);

        // Chuyển đổi sliderValue từ 0 đến 1 thành các giá trị từ minValue đến maxValue
        float convertedValue = Mathf.Lerp(minValue, maxValue, sliderValue);

        return convertedValue;
    }

    private void AutoSavingCheckBoxChanged()
    {
        SaveGame.instance.autoSaving = !SaveGame.instance.autoSaving;
    }
}
