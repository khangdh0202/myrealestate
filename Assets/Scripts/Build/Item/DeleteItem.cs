using UnityEngine;
using UnityEngine.UI;

public class DeleteItem : MonoBehaviour
{
    public GameObject controller; // Controller sẽ thực hiện raycast.

    [SerializeField]
    private GameObject itemToDelete; // Đối tượng cần xóa.

    [SerializeField]
    private GameObject deleteUICanvas;
    private bool isDeleteUICanvas = false;

    private void Awake()
    {
        deleteUICanvas.SetActive(false);

        Button btnYes = deleteUICanvas.transform.Find("Yes").GetComponent<Button>();
        btnYes.onClick.AddListener(() =>
        {
            YesChoice();
        });
        Button btnNo = deleteUICanvas.transform.Find("No").GetComponent<Button>();
        btnNo.onClick.AddListener(() =>
        {
            NoChoice();
        });
    }
    public void ActivateDelete(Transform transform)
    {
        if(PlacementSystem.Instance.mouseIndicator != null)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if(hit.collider.CompareTag(Settings.roadTag) || hit.collider.CompareTag(Settings.contructionTag))
            {
                Debug.Log("Delete Item");

                // Xác định đối tượng cần xóa.
                itemToDelete = hit.collider.gameObject;

                // Sử dụng vị trí của headset VR để đặt vị trí của UI Canvas
                Vector3 headsetPosition = Camera.main.transform.position;
                Vector3 directionVec = Camera.main.transform.forward;

                // Điều chỉnh vị trí và hướng của UI Canvas theo headsetPosition
                deleteUICanvas.transform.position = headsetPosition + directionVec * 2f; // 2f là khoảng cách 

                deleteUICanvas.transform.rotation = Quaternion.LookRotation(directionVec);

                // Hiển thị UI Canvas và đặt trạng thái hiển thị là true.
                deleteUICanvas.SetActive(true);
                isDeleteUICanvas = true;
            }
        }

    }

    public void YesChoice()
    {
        if (isDeleteUICanvas)
        {
            // Xóa đối tượng và tắt UI Canvas.
            deleteUICanvas.SetActive(false);
            isDeleteUICanvas = false;

            // Xóa đối tượng.
            PlacementSystem.Instance.DeleteItemInMap(itemToDelete);
        }
    }
    public void NoChoice() {
        if (isDeleteUICanvas)
        {
            // Chỉ tắt UI Canvas mà không xóa đối tượng.
            deleteUICanvas.SetActive(false);
            isDeleteUICanvas = false;
        }
    }
}
