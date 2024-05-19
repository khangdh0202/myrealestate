using FluffyUnderware.Curvy;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadData
{
    public List<Vector3> position = new List<Vector3>();
    public List<Vector3> rotation = new List<Vector3>();
    public List<Vector3> localScale = new List<Vector3>();
    public List<bool> autoHandles = new List<bool>();
    public List<float> distance = new List<float>();
    public List<Vector3> handleIn = new List<Vector3>();
    public List<Vector3> handleOut = new List<Vector3>();

    public RoadData(GameObject obj)
    {
        for(int i = 0; i < obj.transform.childCount; i++) 
        {
            position.Add(obj.transform.GetChild(i).position);
            rotation.Add(new Vector3(obj.transform.GetChild(i).rotation.x, obj.transform.GetChild(i).rotation.y, obj.transform.GetChild(i).rotation.z));
            localScale.Add(obj.transform.GetChild(i).localScale);

            CurvySplineSegment segment = obj.transform.GetChild(i).gameObject.GetComponent<CurvySplineSegment>();
            if (segment != null)
            {
                autoHandles.Add(segment.AutoHandles);
                distance.Add(segment.AutoHandleDistance);
                handleIn.Add(segment.HandleIn);
                handleOut.Add(segment.HandleOut);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy CurvySplineSegment tại vị trí " + i);
            }
            
        }
    }
}
