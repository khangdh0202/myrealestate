using FluffyUnderware.Curvy.Generator;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : MonoBehaviour, IDataPersistantce
{
    [Header("Component")]
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject displayText;
    [SerializeField] private float timeAutoSave, timeCooldown;
    public bool autoSaving;

    private Coroutine displayNotification;

    public static SaveGame instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        timeAutoSave = timeCooldown;

        saveButton.onClick.RemoveAllListeners();

        saveButton.onClick.AddListener(() =>
        {
            StartCoroutine(DisplayNotification());
        });
    }

    private void Update()
    {
        if (timeAutoSave > 0)
        {
            timeAutoSave -= Time.deltaTime;
        }
        else
        {
            if (autoSaving)
            {
                StartCoroutine(DisplayNotification());
            }
            timeAutoSave = timeCooldown;
        }

        if (displayText.activeSelf)
        {
            // Sử dụng vị trí của headset VR để đặt vị trí của UI Canvas
            Vector3 headsetPosition = Camera.main.transform.position;
            Vector3 directionVec = Camera.main.transform.forward;

            // Điều chỉnh vị trí và hướng của UI Canvas theo headsetPosition
            displayText.transform.position = headsetPosition + directionVec * 2f; // 2f là khoảng cách 

            displayText.transform.rotation = Quaternion.LookRotation(directionVec);
        }
    }

    private IEnumerator DisplayNotification()
    {
        displayText.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Saving...";
        saveButton.interactable = false;

        // Sử dụng vị trí của headset VR để đặt vị trí của UI Canvas
        Vector3 headsetPosition = Camera.main.transform.position;
        Vector3 directionVec = Camera.main.transform.forward;

        // Điều chỉnh vị trí và hướng của UI Canvas theo headsetPosition
        displayText.transform.position = headsetPosition + directionVec * 2f; // 2f là khoảng cách 

        displayText.transform.rotation = Quaternion.LookRotation(directionVec);

        displayText.gameObject.SetActive(true);
        DataPersistantceManagerce.Instance.SaveGame();
        System.GC.Collect();
        yield return new WaitForSeconds(3f);
        saveButton.interactable = true;
        displayText.gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        autoSaving = data.autoSaving;
    }

    public void SaveData(GameData data)
    {
        data.autoSaving = autoSaving;
    }
}
