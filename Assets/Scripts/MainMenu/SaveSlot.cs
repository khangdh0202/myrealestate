using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject haveDataContent;
    [SerializeField] private Text nameFileSlot;
    [SerializeField] private Text timeSave;

    [Header("Clear data button")]
    [SerializeField] private Button clearButton;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;
    private void Awake()
    {
        this.saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData gameData)
    {
        // không có data trong profileId này
        if(gameData == null) 
        {
            hasData = false;
            noDataContent.SetActive(true);
            haveDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
        }
        // Có data trong profileId này
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            haveDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);

            nameFileSlot.text = profileId;
            timeSave.text = gameData.timeSave;
        }
    }
    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        clearButton.interactable = interactable;
    }
}
