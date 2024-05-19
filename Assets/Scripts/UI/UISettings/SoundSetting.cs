using UnityEngine.UI;
using UnityEngine;

public class SoundSetting : MonoBehaviour, ISettingsFeature
{
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Run();
        });
    }

    public void Run()
    {
        this.transform.parent.Find("Scroll View").GetComponent<ViewLoader>().ViewName(this.name);
        for (int i = 0; i < this.transform.parent.Find("Scroll View").Find("Viewport").childCount; i++)
            this.transform.parent.Find("Scroll View").Find("Viewport").GetChild(i).gameObject.SetActive(false);
        this.transform.parent.Find("Scroll View").Find("Viewport").Find(this.name).gameObject.SetActive(true);
    }
}
