using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour, IUIManager
{
    [SerializeField] private GameObject uiSettingObj;
    public ViewLoader viewLoader;
    Button btn;

    public void UIActive()
    {
        UISystem.Instance.UIActive(uiSettingObj);
        viewLoader.ViewName("Credits");

        for(int i = 0;i< viewLoader.transform.Find("Viewport").childCount;i++)
            viewLoader.transform.Find("Viewport").GetChild(i).gameObject.SetActive(false);

        viewLoader.transform.Find("Viewport").Find("Credits").gameObject.SetActive(true);
    }

    public void UIActive(GameObject UI)
    {
        
    }

    private void Awake()
    {
        uiSettingObj.SetActive(false);
        for (int i = 0; i < uiSettingObj.transform.Find("Scroll View").Find("Viewport").childCount;i++)
        {
            uiSettingObj.transform.Find("Scroll View").Find("Viewport").GetChild(i).gameObject.SetActive(false);
        }

        btn = this.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            UIActive();

            if (!uiSettingObj.transform.Find("Feature").GetComponent<FeatureSetting>())
                uiSettingObj.transform.Find("Feature").AddComponent<FeatureSetting>();
            if (!uiSettingObj.transform.Find("Sound").GetComponent<SoundSetting>())
                uiSettingObj.transform.Find("Sound").AddComponent<SoundSetting>();
            if (!uiSettingObj.transform.Find("Back").GetComponent<BackSetting>())
                uiSettingObj.transform.Find("Back").AddComponent<BackSetting>();
        });
    }
}
