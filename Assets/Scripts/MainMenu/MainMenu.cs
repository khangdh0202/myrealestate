using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu navigation")]
    [SerializeField] private SaveSlotsMenu SaveSlotsMenu;

    [SerializeField] private Button continuesButton;
    [SerializeField] private Button loadGameButton;

    private void Start()
    {
        DataPersistantceManagerce.Instance.OnSceceLoaded();
        DisableButtonDependingOnData();
        // Đặt lại vị trí player
        Player.Instance.transform.position = new Vector3(-0.8429611f, 5f, -36.80896f);
    }

    private void DisableButtonDependingOnData()
    {
        if (!DataPersistantceManagerce.Instance.HasGameData())
        {
            continuesButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }

    public void OnNewClicked()
    {
        SaveSlotsMenu.ActivateMenu(false);
        this.DeactiveMenu();
    }

    public void OnLoadClicked()
    {
        SaveSlotsMenu.ActivateMenu(true);
        this.DeactiveMenu();
    }

    public void OnContinuesClicked()
    {
        // Save game 
        //DataPersistantceManagerce.Instance.SaveGame();

        // Load Scene
        SceneManager.LoadSceneAsync("SimpleEnviroment", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("PersistantceScene", LoadSceneMode.Additive);
    }

    public void QuitApp()
    {
        System.GC.Collect();
        Application.Quit();
    }
    
    public void ActiveMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonDependingOnData();
    }
    public void DeactiveMenu()
    {
        this.gameObject.SetActive(false);
    }
}
