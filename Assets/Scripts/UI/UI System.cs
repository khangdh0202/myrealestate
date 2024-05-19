using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class UISystem : SingletonMonobehaviours<UISystem>, IUIManager
{
    [SerializeField]
    GameObject UIControllerLeft;

    [SerializeField]
    GameObject UIControllerRight;

    [SerializeField]
    GameObject BaseControllerLeft;

    [SerializeField]
    GameObject BaseControllerRight;

    // truyền vào đối tượng Canvas UI
    public GameObject UICanvasGameobject;

    [SerializeField]
    InputActionReference menuButtonAction;
    [SerializeField]
    InputActionReference inputTrigger;

    private void OnEnable()
    {
        menuButtonAction.action.performed += ActivateUIMode;
    }
    private void OnDisable()
    {
        menuButtonAction.action.performed -= ActivateUIMode;
    }
    // Start is called before the first frame update
    public void Start()
    {
        UIControllerLeft = Player.Instance.transform.Find("Camera Offset").Find("LeftHand Father").Find("LeftHand UI Controller").gameObject;
        UIControllerRight = Player.Instance.transform.Find("Camera Offset").Find("RightHand Father").Find("RightHand UI Controller").gameObject;
        BaseControllerLeft = Player.Instance.transform.Find("Camera Offset").Find("LeftHand Father").Find("Left Base Controller").gameObject;
        BaseControllerRight = Player.Instance.transform.Find("Camera Offset").Find("RightHand Father").Find("Right Base Controller").gameObject;

        UICanvasGameobject = UIManager.Instance.menuUI;

        // Luôn tắt UI khi bắt đầu
        UICanvasGameobject.SetActive(false);
        // Tẳt ray cho UI Controller
        UIControllerLeft.GetComponent<XRRayInteractor>().enabled = false;
        UIControllerLeft.GetComponent<XRInteractorLineVisual>().enabled = false;
        UIControllerRight.GetComponent<XRRayInteractor>().enabled = false;
        UIControllerRight.GetComponent<XRInteractorLineVisual>().enabled = false;
    }

    private void Update()
    {
        /*if (inputActionReference_UISwitcher.action.ReadValue<float>() >= 0.5f)
        {
            Debug.Log("Menu Button ");
        }*/
    }
    private void ActivateUIMode(InputAction.CallbackContext obj)
    {
        UIActive();
    }
    public void UIActive()
    {
        Debug.Log("Open Menu UI");
        if (!Settings.isMainMenuActive)
        {
            // Bật tia Ray 
            UIControllerLeft.GetComponent<XRRayInteractor>().enabled = true;
            UIControllerLeft.GetComponent<XRInteractorLineVisual>().enabled = true;
            UIControllerRight.GetComponent<XRRayInteractor>().enabled = true;
            UIControllerRight.GetComponent<XRInteractorLineVisual>().enabled = true;

            // Tắt Direct Interactor cho 2 base controller
            BaseControllerLeft.GetComponent<XRDirectInteractor>().enabled = false;
            BaseControllerRight.GetComponent<XRDirectInteractor>().enabled = false;

            // Sử dụng vị trí của headset VR để đặt vị trí của UI Canvas
            Vector3 headsetPosition = Camera.main.transform.position;
            Vector3 directionVec = Camera.main.transform.forward;

            // Điều chỉnh vị trí và hướng của UI Canvas theo headsetPosition
            UICanvasGameobject.transform.position = headsetPosition + directionVec * 2f; // 2f là khoảng cách 

            UICanvasGameobject.transform.rotation = Quaternion.LookRotation(directionVec);

            // kiểm tra inventory có mở không, đang mở thì đóng lại 
            if (Settings.isOpenInventory == true)
            {
                InventoryManager.Instance.canvasUiInventory.SetActive(false);
            }

            Settings.isMainMenuActive = true;
            // Kích hoạt UI Canvas Gameobject
            UICanvasGameobject.SetActive(true);
        }
        else if (Settings.isMainMenuActive)
        {
            Settings.isMainMenuActive = false;

            // Tắt Controller UI 
            UIControllerLeft.GetComponent<XRRayInteractor>().enabled = false;
            UIControllerLeft.GetComponent<XRInteractorLineVisual>().enabled = false;
            UIControllerRight.GetComponent<XRRayInteractor>().enabled = false;
            UIControllerRight.GetComponent<XRInteractorLineVisual>().enabled = false;

            // Bật controller base 
            BaseControllerLeft.GetComponent<XRDirectInteractor>().enabled = true;
            BaseControllerRight.GetComponent<XRDirectInteractor>().enabled = true;

            UICanvasGameobject.SetActive(false);

            for (int i = 0; i < UICanvasGameobject.transform.parent.childCount; i++)
            {
                UICanvasGameobject.transform.parent.GetChild(i).gameObject.SetActive(false);
            }

            PlacementSystem.Instance.CancelItem(); // Xóa đối tượng build hiện đang làm việc nếu có
            RoadController.Instance.StopCreateStartPointRoad();
        }
    }
    public void UIActive(GameObject uiGameObject)
    {
        if (!uiGameObject.activeSelf)
        {
            // Sử dụng vị trí của headset VR để đặt vị trí của UI Canvas
            Vector3 headsetPosition = Camera.main.transform.position;
            Vector3 directionVec = Camera.main.transform.forward;

            // Điều chỉnh vị trí và hướng của UI Canvas theo headsetPosition
            uiGameObject.transform.position = headsetPosition + directionVec * 2f; // 2f là khoảng cách 

            uiGameObject.transform.rotation = Quaternion.LookRotation(directionVec);

            // Hiển thị UI Canvas và đặt trạng thái hiển thị là true.
            uiGameObject.SetActive(true);

            if (UICanvasGameobject.activeSelf)
            {
                UICanvasGameobject.SetActive(false);
            }
        }
        else
        {
            uiGameObject.SetActive(false);
        }
    }
}
