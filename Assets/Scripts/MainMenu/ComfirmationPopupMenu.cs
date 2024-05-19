using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ComfirmationPopupMenu : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Text displayText;
    [SerializeField] Button confirmButton;
    [SerializeField] private Button cancelButton;


    public void ActivateMenu(string displayText,UnityAction confirmAction, UnityAction cancelAction)
    {
        this.gameObject.SetActive(true);

        this.displayText.text = displayText;

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });
    }

    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
