/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    private bool _onMap;
    private GameObject nearestGameObject;

    public bool OnMap { get => _onMap; set => _onMap = value; }
    public GameObject NearestGameObject { get => nearestGameObject; set => nearestGameObject = value; }

    private void Awake()
    {
        OnMap = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Settings.snapTag))
        {
            NearestGameObject = ObjectNearest();
        }
    }

    /// <summary>
    /// Trả về đối tượng gần với grid nhất 
    /// </summary>
    public GameObject ObjectNearest()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.parent.transform.localScale.x< transform.parent.transform.localScale.z? transform.parent.transform.localScale.x*10f : transform.parent.transform.localScale.z*10f);
        float minDistance = float.MaxValue;
        GameObject nearestGameobject=null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(Settings.snapTag))
            {
                //Debug.Log($"Name snap: {collider.transform.parent.parent.name}");
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                //Debug.Log($"distance: {distance} + minDistance: {minDistance}");
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestGameobject = collider.gameObject;
                }
            }
        }

        return nearestGameobject;
    }
}
*/