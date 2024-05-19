using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("Menu navigation")]
    [SerializeField] private MainMenu mainMenu;

    private SaveSlot[] saveSlots;

    [SerializeField] private Button backButton;

    [Header("Confirm Popup")]
    [SerializeField] private ComfirmationPopupMenu confirmationPopupMenu;
    //[Header("Loading Popup")]
    //[SerializeField] private

    private bool isLoadingGame = false;


    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot slot)
    {
        DisableMenuButton();

        // case - loading game
        if(isLoadingGame)
        {
            DataPersistantceManagerce.Instance.ChangedSelectedProfileId(slot.GetProfileId());
            SaveGameAndLoadScene();
        }
        // case - new game, but the save has data
        else if (slot.hasData)
        {
            confirmationPopupMenu.ActivateMenu(
                "Starting a new slot overwrites the currently saved slot. Are you sure?",
                // yes choice 
                () =>
                {
                    DataPersistantceManagerce.Instance.ChangedSelectedProfileId(slot.GetProfileId());
                    DataPersistantceManagerce.Instance.NewGame();
                    SaveGameAndLoadScene();
                },
                // cancel choice
                () =>
                {
                    this.ActivateMenu(isLoadingGame);
                }
                );
        }
        // case - new game, slot game no has data
        else
        {
            DataPersistantceManagerce.Instance.ChangedSelectedProfileId(slot.GetProfileId());
            DataPersistantceManagerce.Instance.NewGame();
            SaveGameAndLoadScene();
        }

    }

    public void OnBackClick()
    {
        this.mainMenu.ActiveMenu();
        this.DeactivateMenu();
    }

    private void SaveGameAndLoadScene()
    {
        // Save game
        DataPersistantceManagerce.Instance.SaveGame();

        // Load Scene
        SceneManager.LoadSceneAsync("SimpleEnviroment", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("PersistantceScene", LoadSceneMode.Additive);
    }

    public void OnDeleteClicked(SaveSlot savelot)
    {
        DisableMenuButton();

        confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete saved data?",
            // Confirm choice
            () =>
            {
                DataPersistantceManagerce.Instance.DeleteProfileId(savelot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            // Cancel choice
            () =>
            {
                ActivateMenu(isLoadingGame);
            });
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        // Set mode
        this.isLoadingGame = isLoadingGame;

        // Load tất cả file profile hiện có
        Dictionary<string, GameData> profileGameData = DataPersistantceManagerce.Instance.GetAllProfilesGameData();

        GameObject firstSelected = backButton.gameObject;

        // Đảm bảo backButton bật khi active menu
        backButton.interactable = true;

        // vòng lặp tạo các saveslot trên UI 
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profileGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData== null && isLoadingGame)
            {
                saveSlot.SetInteractable(false); 
            }
            else
            {
                saveSlot.SetInteractable(true);
                if(firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButton()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }

        backButton.interactable = false;
    }
}
