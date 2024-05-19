using System;
using UnityEngine;

[Serializable]
public class SettingsData
{
    // Feature
    public bool borderContruction;
    public float borderWidth;
    public bool snapToConner;
    public bool snapToAngle;

    // Sound
    public float soundMaster;
    public float BGM;
    public float SFX;

    public SettingsData() {
        borderContruction = true;
        borderWidth = 0.03f;
        snapToConner = true;
        snapToAngle = true;

        soundMaster = 1;
        BGM = 1;
        SFX = 1;
    }
}
