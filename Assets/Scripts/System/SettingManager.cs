using System.Collections;
using UnityEngine;

public class SettingManager : SingletonMonobehaviours<SettingManager>
{
    public bool borderContructionView = true ;
    public float maxSizeBorder = 0.3f;
    public float minSizeBorder = 0.01f;
    public float borderContructionWidth = 0.03f;
    public bool snapToConner =true;
    public bool SnapToAngle = true;

    public float volumeMaster = Settings.volumeMaster;
    public float volumeBGM = Settings.volumeBGM;
    public float volumeSFX = Settings.volumeSFX;

    private void Start()
    {
        borderContructionView = true;
        borderContructionWidth = 0.03f;
        SnapToAngle=true;
        snapToConner = true;
        volumeBGM = Settings.volumeBGM;
        volumeSFX = Settings.volumeSFX;
        volumeMaster = Settings.volumeMaster;

        DontDestroyOnLoad(this.gameObject);
    }
}
