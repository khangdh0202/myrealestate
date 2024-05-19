using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class UserSettings : MonoBehaviour,IDataPersistantce
{

    public void LoadData(GameData data)
    {
        SettingManager.Instance.borderContructionView = data.settingsData.borderContruction;
        SettingManager.Instance.borderContructionWidth = data.settingsData.borderWidth;
        SettingManager.Instance.snapToConner = data.settingsData.snapToConner;
        SettingManager.Instance.SnapToAngle = data.settingsData.snapToAngle;

        SettingManager.Instance.volumeMaster = data.settingsData.soundMaster;
        SettingManager.Instance.volumeBGM = data.settingsData.BGM;
        SettingManager.Instance.volumeSFX = data.settingsData.SFX;

        Settings.volumeMaster = SettingManager.Instance.volumeMaster;
        Settings.volumeBGM = SettingManager.Instance.volumeBGM;
        Settings.volumeSFX = SettingManager.Instance.volumeSFX;

    }

    public void SaveData(GameData data)
    {
        SettingsData settingsData = new SettingsData();

        settingsData.borderContruction = SettingManager.Instance.borderContructionView;
        settingsData.borderWidth = SettingManager.Instance.borderContructionWidth;
        settingsData.snapToConner = SettingManager.Instance.snapToConner;
        settingsData.snapToAngle = SettingManager.Instance.SnapToAngle;

        settingsData.soundMaster = Settings.volumeMaster;
        settingsData.BGM = Settings.volumeBGM;
        settingsData.SFX = Settings.volumeSFX;

        data.settingsData = settingsData;
    }
}
