using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistantceManagerce : SingletonMonobehaviours<DataPersistantceManagerce>
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;


    [SerializeField] private bool modeDeveloper;
    GameData gameData;

    private List<IDataPersistantce> dataPersistantceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";


    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        GC.Collect();

        if (disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceceLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceceLoaded;
    }

    public void OnSceceLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistantceObjects = FindAllDataPersistanceObjects();
        LoadGame();

    }
    public void OnSceceLoaded()
    {
        this.dataPersistantceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // trả về lập tức nếu disable Data Persistence tắt
        if (disableDataPersistence)
            return;

        // Load bất kì dữ liệu đã lưu từ một file bằng cách dùng data handler
        this.gameData = dataHandler.Load(selectedProfileId);

        // Chế độ nhà phát triển để fix bug
        if (this.gameData == null && modeDeveloper)
            NewGame();

        // Nếu không có dữ liệu để load, không tiếp tục
        if (this.gameData == null)
        {
            Debug.Log("Không có dữ liệu để load, cần bắt đầu trước khi có dữ liệu để load");
            return;
        }
        this.dataPersistantceObjects = FindAllDataPersistanceObjects();
        foreach (IDataPersistantce dataPersistantce in dataPersistantceObjects)
        {
            dataPersistantce.LoadData(gameData);
        }
    }

    public void SaveGame()
    { 
        // trả về lập tức nếu disable Data Persistence tắt
        if (disableDataPersistence)
            return;

        if (this.gameData == null)
        {
            Debug.Log("Không có savedata để save. Cần bắt đầu trước khi save");
            return;
        }

        this.dataPersistantceObjects = FindAllDataPersistanceObjects();

        foreach (IDataPersistantce dataPersistantce in dataPersistantceObjects)
        {
            dataPersistantce.SaveData(gameData);
        }

        gameData.lastUpdated =System.DateTime.Now.ToBinary();

        DateTime currentTime = DateTime.Now;

        // Format lại thời gian theo định dạng "giờ:phút:giây ngày/tháng/năm"
        string formattedTime = currentTime.ToString("HH:mm:ss dd/MM/yyyy");

        gameData.timeSave = formattedTime;

        dataHandler.Save(gameData,selectedProfileId);
    }

    public void DeleteProfileId(string profileId)
    {
        // Xóa data từ profileId
        dataHandler.Delete(profileId);

        InitializeSelectedProfileId();
        // Reload để có dữ liệu mới 
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistantce> FindAllDataPersistanceObjects()
    {
        // 
        IEnumerable<IDataPersistantce> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistantce>();

        return new List<IDataPersistantce>(dataPersistanceObjects);
    }

    public bool HasGameData()
    {
        return gameData!=null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public void ChangedSelectedProfileId(string profileId)
    {
        this.selectedProfileId = profileId;

        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }
}
