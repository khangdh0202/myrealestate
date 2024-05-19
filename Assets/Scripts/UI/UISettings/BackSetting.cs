using UnityEngine.UI;
using UnityEngine;

public class BackSetting : MonoBehaviour
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
        
    }
}
