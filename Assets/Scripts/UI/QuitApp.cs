using System;
using UnityEngine;

public class QuitApp : MonoBehaviour
{
    public void Quit()
    {
        GC.Collect();
        Application.Quit();
    }
}
