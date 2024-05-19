using FluffyUnderware.Curvy.Generator.Modules;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class CheckCanPutRoadCollision : MonoBehaviour
{
    public string blueColor  = "#00AAFF";
    public string redColor = "#FF0A00";

    private void Start()
    {
        SetColorMaterial(RoadController.Instance.transform.GetChild(0).Find("Volume Mesh").gameObject.GetComponent<BuildVolumeMesh>().GetMaterial(0), blueColor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject)
        {
            if (other.transform.parent.gameObject)
            {
                if (other.transform.parent.parent.gameObject)
                {
                    if (other.transform.parent.parent.gameObject && this.transform.parent.parent.gameObject)
                    {
                        if (other.transform.parent.parent.gameObject == this.transform.parent.parent.gameObject)
                        {
                            if (other.gameObject.CompareTag("Road"))
                            {
                                RoadController.Instance.CanPutRoadInMap = false;

                                SetColorMaterial(RoadController.Instance.transform.GetChild(0).Find("Volume Mesh").gameObject.GetComponent<BuildVolumeMesh>().GetMaterial(0), redColor);
                            }
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Road") && !Settings.EndRoadExit)
        {
            Debug.Log("1");
            if(RoadController.Instance.CanPutRoadInMap)
            {
                Debug.Log("2");
                Settings.RoadSnapToRoad = true;
                PlacementSystem.Instance.directionRoadSnap = "SnapToRoad";
                if (other.gameObject.GetComponent<RoadCollison>())
                {
                    Debug.Log("3");
                    //PlacementSystem.Instance.mouseIndicator.transform.position = other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator;
                    if (other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator != Vector3.zero)
                    {
                        Debug.Log("4");
                        if (other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator
                            == other.gameObject.GetComponent<RoadCollison>().firstSegment.gameObject.transform.position)
                        {
                            Debug.Log($"firstSegment");

                            //Settings.snapToFirstOrLastSegment = "FirstSegment";
                            //Settings.parentOfSnapToSegment = other.gameObject.GetComponent<RoadCollison>().firstSegment.transform.parent.gameObject;
                        }
                        else if(other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator
                            == other.gameObject.GetComponent<RoadCollison>().lastSegment.gameObject.transform.position)
                        {
                            Debug.Log($"lastSegment");
                            //Settings.snapToFirstOrLastSegment = "LastSegment";
                            //Settings.parentOfSnapToSegment = other.gameObject.GetComponent<RoadCollison>().lastSegment.transform.parent.gameObject;
                        }
                        else
                        {
                            //Settings.snapToFirstOrLastSegment = "";
                            //Settings.parentOfSnapToSegment = null;
                        }

                        Settings.positionRoadSnapToRoad = other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator;

                        Vector3 vectorDoLech = Settings.positionRoadSnapToRoad - transform.position;

                        float khoangCach = Vector3.Distance(transform.position, Settings.positionRoadSnapToRoad);

                        transform.Translate(vectorDoLech.normalized * khoangCach);


                        RoadController.Instance.EndRoadTemporary.transform.position = other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator;
                        //Settings.positionRoadSnapToRoad = this.transform.position;

                        Settings.roadSnaptoRoad = true;
                    }
                    //Settings.positionRoadSnapToRoad = other.gameObject.GetComponent<RoadCollison>().positionNearestWithMouseIndicator;
                    
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject && other.transform.parent.gameObject && other.transform.parent.parent.gameObject)
        {
            if (other.transform.parent.parent.gameObject && this.transform.parent.parent.gameObject)
            {
                if (other.transform.parent.parent.gameObject == this.transform.parent.parent.gameObject)
                {
                    if (other.gameObject.CompareTag("Road"))
                    {
                        RoadController.Instance.CanPutRoadInMap = true;

                        SetColorMaterial(RoadController.Instance.transform.GetChild(0).Find("Volume Mesh").gameObject.GetComponent<BuildVolumeMesh>().GetMaterial(0), blueColor);
                        Settings.roadSnaptoRoad = false;
                    }
                }
            }
        }
    }

    private void SetColorMaterial(Material material,string baseColor)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(baseColor, out color))
        {
            // Đặt màu cho Material
            material.SetColor("_BaseColor", color); // "_Color" là tên của thuộc tính màu trong Shader của Material
        }
        else
        {
            Debug.LogError("Invalid hex color format!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Settings.positionRoadSnapToRoad, 0.2f);
    }
}
