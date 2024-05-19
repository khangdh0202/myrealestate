using UnityEngine;
using UnityEngine.UI;

public class RoadUI : MonoBehaviour, IUIManager
{
    [SerializeField] private GameObject roadMenuUI;

    public Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("Open Road UI");
            UIActive();
            for(int i = 0; i< roadMenuUI.transform.childCount; i++)
            {
                if(!roadMenuUI.transform.GetChild(i).gameObject.GetComponent<RoadTypeUI>())
                    roadMenuUI.transform.GetChild(i).gameObject.AddComponent<RoadTypeUI>();
            }
        });
    }
    public void UIActive()
    {
        UISystem.Instance.UIActive(roadMenuUI);
    }

    public void UIActive(GameObject UI)
    {
        
    }
}
