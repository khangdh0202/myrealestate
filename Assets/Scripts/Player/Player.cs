using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : SingletonMonobehaviours<Player>, IDataPersistantce
{
    [SerializeField]
    private InputActionAsset inputActions;

    private GameObject instance;

    [SerializeField] SO_ItemList itemList;

    private ActionBasedContinuousMoveProvider move;

    //thiết lập nút (ngón tay trỏ được ấn)
    [SerializeField]
    private InputActionReference triggerButtonLeftController;
    [SerializeField]
    private InputActionReference triggerButtonRightController;
    [SerializeField]
    private InputActionReference primaryButtonLeftController;
    [SerializeField]
    private InputActionReference primaryButtonRightController;
    [SerializeField]
    private InputActionReference joystickRightController;
    [SerializeField]
    private InputActionReference gripButtonRightController;
    [SerializeField] 
    private InputActionReference secondaryButtonLeftController;

    private void OnEnable()
    {
        triggerButtonLeftController.action.Enable();
        triggerButtonRightController.action.Enable();
        primaryButtonLeftController.action.Enable();
        primaryButtonRightController.action.Enable();
        joystickRightController.action.Enable();
        gripButtonRightController.action.Enable();
        secondaryButtonLeftController.action.Enable();
    }
    private void OnDisable()
    {
        triggerButtonLeftController.action.Disable();
        triggerButtonRightController.action.Disable();
        primaryButtonLeftController.action.Disable();
        primaryButtonRightController.action.Disable();
        joystickRightController.action.Disable();
        gripButtonRightController.action.Disable();
        secondaryButtonLeftController.action.Disable();
    }


    protected override void Awake()
    {
        base.Awake();
    }

    public void LoadData(GameData data)
    {
        this.transform.Find("Camera Offset").Find("Main Camera").transform.rotation = Quaternion.Euler(data.cameraRotation);
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.cameraRotation = Camera.main.transform.rotation.eulerAngles;
        data.playerPosition = transform.position;
    }


    public Action<float, float, float> UpdateScale;

    private void Start()
    {
        instance = this.gameObject;
        move = instance.GetComponent<ActionBasedContinuousMoveProvider>();
    }

    Action inputActionReference_UISwitcher;
    public Action<float> ItemScale;
    public Action<float, float> ItemRotation;
    public Action ItemInLeftHand, ItemInRightHand;
    public Action ItemCancelSnap, PutBuildingInMap, StopSetPointRoad;
    public Action<Transform> ItemDeleteInMap;
    public Action MenuUI;

    private void Update()
    {
        

        //DontCanMove();

        ////////////////////////
        float triggerThreshold = 0.5f; // ngưỡng xác định nút đã được nhấn

        Vector2 thumbstickValue = joystickRightController.action.ReadValue<Vector2>();

        // Quit Quick
        if (secondaryButtonLeftController && primaryButtonLeftController && primaryButtonRightController && triggerButtonLeftController)
        {
            if (secondaryButtonLeftController.action.ReadValue<float>() >= triggerThreshold &&
                primaryButtonLeftController.action.ReadValue<float>() >= triggerThreshold &&
                primaryButtonRightController.action.ReadValue<float>() >= triggerThreshold &&
                triggerButtonLeftController.action.ReadValue<float>() >= triggerThreshold)
            {
                GC.Collect();
                Application.Quit();
            }
        }

        if (gripButtonRightController && gripButtonRightController.action.ReadValue<float>() >= triggerThreshold)
        {
            // Scale
            ItemScale?.Invoke(joystickRightController.action.ReadValue<Vector2>().y);
        }
        else
        {
            // Xoay
            ItemRotation?.Invoke(thumbstickValue.x, thumbstickValue.y);
        }

        if(secondaryButtonLeftController && secondaryButtonLeftController.action.ReadValue<float>() >= triggerThreshold)
        {
            if (PlacementSystem.Instance.TypeOfMouseIndicator == "Road")
            {
                StopSetPointRoad?.Invoke();
                return;
            }
            ItemDeleteInMap?.Invoke(this.transform.Find("Camera Offset").Find("LeftHand Father").Find("LeftHand UI Controller"));
            
            ItemCancelSnap?.Invoke();
        }

        if(triggerButtonLeftController && triggerButtonLeftController.action.ReadValue<float>() >= triggerThreshold)
        {
            Debug.Log("Left");
            ItemInLeftHand?.Invoke();
        }
        if (triggerButtonRightController && triggerButtonRightController.action.ReadValue<float>() >= triggerThreshold)
        {
            Debug.Log("Right");
            ItemInRightHand?.Invoke();
        }
        if(primaryButtonLeftController && primaryButtonLeftController.action.ReadValue<float>() >= triggerThreshold)
        {
            PutBuildingInMap?.Invoke();
        }
        if(primaryButtonRightController && primaryButtonRightController.action.ReadValue<float>() >= triggerThreshold)
        {
            PutBuildingInMap?.Invoke();
        }
    }

    /// <summary>
    ///  Đề nghị để di chuyển là 10 và 0 để cấm di chuyển
    /// </summary>
    /// <param name="value"></param>
    public void MovementSpeed(float value)
    {
        //move.moveSpeed = value;

        /*if (!InputManager.IsPointerOverUI()) // Nếu đang tương tác UI, cấm di chuyển
        {
            move.moveSpeed=10;
        }
        else
        {
            move.moveSpeed = 0;
        }*/
    }
}
