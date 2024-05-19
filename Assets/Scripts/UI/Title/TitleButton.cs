using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public void OnTitleClicked()
    {
        // Load Scene
        DataPersistantceManagerce.Instance.SaveGame();
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
