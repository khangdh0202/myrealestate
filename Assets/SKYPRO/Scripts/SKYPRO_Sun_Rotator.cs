using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKYPRO_Sun_Rotator : MonoBehaviour, IDataPersistantce
{
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private bool realTime = true;

    public void LoadData(GameData data)
    {
        if(realTime== true)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(data.sunRotation, 20, 0));
        }
        else
        {
            transform.localRotation = Quaternion.Euler(new Vector3(80, 20, 0));
        }
    }

    public void SaveData(GameData data)
    {
        if (realTime == true)
        {
            data.sunRotation = transform.localRotation.x;
        }
    }

    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        //transform.localEulerAngles.x + ((rotationSpeed / 10) * Time.deltaTime)
        if(realTime == true)
        {
            transform.localEulerAngles = new Vector3(Time.time * rotationSpeed, 20, 0);
        }
    }
}
