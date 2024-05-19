/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnapGridCollison : MonoBehaviour
{
    private bool _CanSnap;

    [Tooltip("true nếu snap đang được bật và false nếu snap đã được tắt")] public bool CanSnap { get => _CanSnap; set => _CanSnap = value; }

    private void Awake()
    {
        CanSnap = true; 
    }

    private void OnTriggerStay(Collider other)
    {
        // layer 10 = grid
        if(other.gameObject.layer==10)
        {
            for(int i=0;i< PlacementSystem.Instance.mouseIndicator.transform.childCount; i++)
            {
                if (transform.parent.parent.gameObject == PlacementSystem.Instance.mouseIndicator.transform.GetChild(i).gameObject)
                {
                    return;
                }
            }
            //Debug.Log($"NearestGameObject name: {other.transform.parent.Find(Settings.gridName).gameObject.GetComponent<GridController>().NearestGameObject}");

            if (other.gameObject.transform.parent.Find(Settings.gridName).GetComponent<GridController>().NearestGameObject != this.gameObject)
            {
                Debug.Log("1?");
                return;
            }

            *//*if (!Settings.canSnap)
            {
                return;
            }*//*


            if (other.gameObject.transform.parent.Find(Settings.gridName).GetComponent<GridController>().OnMap)
            {
                Debug.Log("2?");
                CanSnap = false;
                return;
            }

            if (!CanSnap)
            {
                Debug.Log("3?");
                return;            
            }

            Settings.isSnap = true;

            *//*// Vùng snap là Front và Back => trục Z
            if (gameObject.name == "Front" || gameObject.name == "Back")
            {
                newPosition.z += transform.localPosition.z > 0 ? (((10f * transform.parent.parent.transform.localScale.z) / 2f) + ((10f * other.transform.parent.transform.localScale.z) / 2f)) : -(((10f * transform.parent.parent.transform.localScale.z) / 2f) + ((10f * other.transform.parent.transform.localScale.z) / 2f));
            }

            if (gameObject.name == "Right" || gameObject.name == "Left")
            {
                newPosition.x += transform.localPosition.x > 0 ? (((10f * transform.parent.parent.transform.localScale.x) / 2f) + ((10f * other.transform.parent.transform.localScale.x) / 2f)) : -(((10f * transform.parent.parent.transform.localScale.x) / 2f) + ((10f * other.transform.parent.transform.localScale.x) / 2f));
            }*//*

            GameObject newObj = new GameObject();

            // Sao chép thông tin transform từ đối tượng hiện tại
            newObj.transform.position = transform.parent.parent.gameObject.transform.position;
            newObj.transform.rotation = transform.parent.parent.gameObject.transform.rotation;
            newObj.transform.localScale = transform.parent.parent.gameObject.transform.localScale;


            // Vùng snap là Front và Back => trục Z
            if (gameObject.name == "Back")
            {
                PlacementSystem.Instance.UpdatePositionMouseIndicator(newObj, (((10f * transform.parent.parent.transform.localScale.z) / 2f) + ((10f * other.transform.parent.transform.localScale.z) / 2f)), 3);
}
            else if (gameObject.name == "Front")
            {
                PlacementSystem.Instance.UpdatePositionMouseIndicator(newObj, (((10f * transform.parent.parent.transform.localScale.z) / 2f) + ((10f * other.transform.parent.transform.localScale.z) / 2f)), 4);
            }

            // Vùng snap là Right và Left => trục X
            else if (gameObject.name == "Right")
            {
                PlacementSystem.Instance.UpdatePositionMouseIndicator(newObj, (((10f * transform.parent.parent.transform.localScale.x) / 2f) + ((10f * other.transform.parent.transform.localScale.x) / 2f)), 1);
            }
            else if (gameObject.name == "Left")
            {
                PlacementSystem.Instance.UpdatePositionMouseIndicator(newObj, (((10f * transform.parent.parent.transform.localScale.x) / 2f) + ((10f * other.transform.parent.transform.localScale.x) / 2f)), 2);
            }

            //Settings.canSnap = false;

            // Cập nhật vị trí mới của đối tượng
            //PlacementSystem.Instance.UpdatePositionMouseIndicator(newPosition.x,newPosition.y,newPosition.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // layer 10 = grid
        if (other.gameObject.layer == 10)
        {
            *//*if (transform.parent.parent.gameObject == PlacementSystem.Instance.mouseIndicator)
            {
                return;
            }*//*
            //Settings.canSnap = true;

            Settings.isSnap = false;
        }
    }
}
*/