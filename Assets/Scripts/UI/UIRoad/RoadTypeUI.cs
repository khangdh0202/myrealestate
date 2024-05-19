using UnityEngine;
using UnityEngine.UI;

public class RoadTypeUI : MonoBehaviour
{
    public Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            RoadController.Instance.CreateStartPointRoad(gameObject.name);
        });
    }
}
