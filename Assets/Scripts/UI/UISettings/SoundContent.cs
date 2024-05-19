using UnityEngine;
using UnityEngine.UI;

public class SoundContent : MonoBehaviour
{
    Slider soundMasterSlider;
    Slider BGMSlider;
    Slider SFXSlider;

    private void Start()
    {
        soundMasterSlider = transform.Find("SoundMaster").Find("Slider").GetComponent<Slider>();
        BGMSlider = transform.Find("BGM").Find("Slider").GetComponent<Slider>();
        SFXSlider = transform.Find("SFX").Find("Slider").GetComponent<Slider>();

        soundMasterSlider.value = Mathf.InverseLerp(0, 1, Settings.volumeMaster);
        BGMSlider.value = Mathf.InverseLerp(0, 1, Settings.volumeBGM);
        SFXSlider.value = Mathf.InverseLerp(0, 1, Settings.volumeSFX);

        soundMasterSlider.onValueChanged.AddListener(delegate { SoundMasterSliderChanged(); });
        BGMSlider.onValueChanged.AddListener(delegate { BGMSliderChanged(); });
        SFXSlider.onValueChanged.AddListener(delegate { SFXSliderChanged(); });
    }
    private void SoundMasterSliderChanged()
    {
        Settings.volumeMaster = ConvertSliderValue(soundMasterSlider.value, 0, 1);
        SettingManager.Instance.volumeMaster = Settings.volumeMaster;
    }
    private void BGMSliderChanged()
    {
        Settings.volumeBGM = ConvertSliderValue(BGMSlider.value, 0, 1);
        SettingManager.Instance.volumeBGM = Settings.volumeBGM;
    }
    private void SFXSliderChanged()
    {
        Settings.volumeSFX = ConvertSliderValue(SFXSlider.value, 0, 1);
        SettingManager.Instance.volumeSFX = Settings.volumeSFX;
    }

    float ConvertSliderValue(float sliderValue, float minValue, float maxValue)
    {
        // Đảm bảo sliderValue nằm trong khoảng từ 0 đến 1
        sliderValue = Mathf.Clamp01(sliderValue);

        // Chuyển đổi sliderValue từ 0 đến 1 thành các giá trị từ minValue đến maxValue
        float convertedValue = Mathf.Lerp(minValue, maxValue, sliderValue);

        return convertedValue;
    }
}
