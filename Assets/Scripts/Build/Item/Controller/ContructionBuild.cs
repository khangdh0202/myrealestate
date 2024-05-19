using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContructionBuild : MonoBehaviour, IInputBuilding
{
    public GameObject currentBuidingInMouse;
    public void DeleteItem(GameObject obj)
    {
        if (obj.GetComponent<ItemInfo>())
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                if (obj.transform.GetChild(i).name == "New Game Object")
                {
                    Destroy(obj.transform.GetChild(i).gameObject);
                }
            }
            obj.GetComponent<ItemInfo>().ItemPool.ReturnObjectToPool(obj);

            obj.GetComponent<BoundingBoxBuilding>().OnMap = false;

            PlacementSystem.Instance.keyDeleteObject.Add(obj.GetComponent<BoundingBoxBuilding>().Id);

            currentBuidingInMouse = null;

            Debug.Log("DeleteItem Contructione");
        }
    }

    public void PlaceItem()
    {
        if (PlacementSystem.Instance.TypeOfMouseIndicator == "Build" ||
            PlacementSystem.Instance.TypeOfMouseIndicator == "Scenary" ||
            PlacementSystem.Instance.TypeOfMouseIndicator == "Prop")
        {
            for (int i = 0; i < PlacementSystem.Instance.mouseIndicator.transform.childCount; i++)
            {
                if (PlacementSystem.Instance.mouseIndicator.transform.GetChild(i) != null)
                {
                    if (PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).name == Settings.snapAreaName)
                    {
                        Destroy(PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject);
                    }
                    if (PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).name != Settings.snapAreaName)
                    {
                        //Debug.Log(PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).name);
                        PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject.GetComponent<BoundingBoxBuilding>().OnMap = true;
                        
                        PlacementSystem.Instance.haveObjectInMouseIndicator = false;
                        PlacementSystem.Instance.directionSnap = "";
                        PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).parent = null;
                        PlacementSystem.Instance.CancelSnap();
                    }
                }
            }
            currentBuidingInMouse = null;
            Destroy(PlacementSystem.Instance.mouseIndicator);
            PlacementSystem.Instance.Sound();
        }
    }

    public void ReRotateItem(float x, float y)
    {
        if (PlacementSystem.Instance.mouseIndicator == null)
            return;
        if (!InputManager.IsPointerOverUI())
        {
            if (PlacementSystem.Instance.timeRotationCounter > 0)
                return;

            // Lấy giá trị scroll joystick
            //Vector2 thumbstickValue = joystickRight.action.ReadValue<Vector2>();

            // Tính toán góc xoay dựa trên giá trị của joystick
            float rotationZ = x * Settings.rotationSpeed * Time.deltaTime;
            float rotationY = y * Settings.rotationSpeed * Time.deltaTime;

            if (rotationZ > 0)
            {
                PlacementSystem.Instance.mouseIndicator.transform.Rotate(Vector3.up, 5f);
            }
            else if (rotationZ < 0)
            {
                PlacementSystem.Instance.mouseIndicator.transform.Rotate(Vector3.down, 5f);
            }
            if (ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
                if(ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent)
                    ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent
                    .Find(Settings.snapAreaName).GetComponent<ColliderWithBounds>().UpdateDirectionCollison(ContructionController.Instance.ContructionBuild.currentBuidingInMouse, ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent
                    .Find(Settings.snapAreaName).GetComponent<ColliderWithBounds>().ratioBoundAndCollider);
            
            if (ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
                if (ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent)
                    ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent
                    .Find(Settings.snapAreaName).rotation = Quaternion.Euler(0, PlacementSystem.Instance.mouseIndicator.transform.rotation.y, 0);

            PlacementSystem.Instance.TimeBuildCooldown(ref PlacementSystem.Instance.timeRotationCounter, PlacementSystem.Instance.timeRotation);
        }
    }

    public void ReSizeItem(float y)
    {
        if (PlacementSystem.Instance.mouseIndicator == null)
            return;
        if (!InputManager.IsPointerOverUI())
        {
            // Lấy giá trị scroll joystick
            //Vector2 thumbstickValue = joystickRight.action.ReadValue<Vector2>();

            // Lấy kích thước hiện tại của đối tượng
            Vector3 currentScale = ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.localScale;
            if (!Settings.inScale)
            {
                Settings.inScale = !Settings.inScale;
                Settings.scaleOriginBuild = currentScale;
            }

            // Tính toán tỷ lệ mới dựa trên giá trị joystick và tỷ lệ hiện tại
            float xScale = currentScale.x + (y * Settings.scaleSpeed / Settings.ratioWorld) * Time.deltaTime;
            float yScale = currentScale.y + (y * Settings.scaleSpeed / Settings.ratioWorld) * Time.deltaTime;
            float zScale = currentScale.z + (y * Settings.scaleSpeed / Settings.ratioWorld) * Time.deltaTime;

            // Giới hạn tỷ lệ mới trong khoảng [minScale, maxScale] tương ứng với kích thước hiện tại
            xScale = Mathf.Clamp(xScale, Settings.minScale * Settings.scaleOriginBuild.x, Settings.maxScale * Settings.scaleOriginBuild.x);
            yScale = Mathf.Clamp(yScale, Settings.minScale * Settings.scaleOriginBuild.y, Settings.maxScale * Settings.scaleOriginBuild.y);
            zScale = Mathf.Clamp(zScale, Settings.minScale * Settings.scaleOriginBuild.z, Settings.maxScale * Settings.scaleOriginBuild.z);

            // Gán tỷ lệ mới cho đối tượng
            ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.localScale = new Vector3(xScale, yScale , zScale);

            if (ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
                if (ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent)
                    ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent
                    .Find(Settings.snapAreaName).GetComponent<ColliderWithBounds>().UpdateDirectionCollison(ContructionController.Instance.ContructionBuild.currentBuidingInMouse, ContructionController.Instance.ContructionBuild.currentBuidingInMouse.transform.parent
                    .Find(Settings.snapAreaName).GetComponent<ColliderWithBounds>().ratioBoundAndCollider);
        }
    }

}
