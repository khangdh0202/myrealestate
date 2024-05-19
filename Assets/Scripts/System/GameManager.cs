using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonobehaviours<GameManager>
{
    // hệ thông xây dựng 
    public Player PlayerPrefabs;

    [Header("Development mode")]
    public bool developmentMode = false;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);
    }

   

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PersistantceScene") {
            //Instantiate(PlayerPrefabs.gameObject, new Vector3(0, 5, 0), Quaternion.Euler(Vector3.zero));
            //UISystem.Instance.Init();
        }
        /*else if (scene.name == "MainMenu")
        {
            Player.Instance.transform.position = new Vector3(0f,5f,0f);
        }*/
    }
}
