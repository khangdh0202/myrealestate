using UnityEngine;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour, IUIManager
{
    [SerializeField] private GameObject uiInventory;
    Button btn;
    public void UIActive()
    {
        UISystem.Instance.UIActive(uiInventory);
        InventoryManager.Instance.FirstTimeActiveInventory();
    }

    public void UIActive(GameObject UI)
    {
       
    }

    private void Start()
    {
        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            UIActive();
        });
    }
}
