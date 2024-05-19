using TMPro;
using UnityEngine;

public class ViewLoader : MonoBehaviour
{
    TextMeshProUGUI nameView;

    private void Awake()
    {
        nameView = this.transform.Find("Name").GetComponent<TextMeshProUGUI>();
    }

    public void ViewName(string str)
    {
        nameView.text = str;
    }
}
