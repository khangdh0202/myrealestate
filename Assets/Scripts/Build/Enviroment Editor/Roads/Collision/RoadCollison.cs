using FluffyUnderware.Curvy;
using UnityEngine;

public class RoadCollison : MonoBehaviour
{
    public Vector3 positionNearestWithMouseIndicator;
    public CurvySplineSegment firstSegment;
    public CurvySplineSegment lastSegment;

    public MeshCollider meshCollider;

    private void Start()
    {
        //Init();
    }

    public void Init()
    {
        firstSegment = transform.parent.parent.Find("RoadLine").gameObject.GetComponent<CurvySpline>().FirstSegment;
        lastSegment = transform.parent.parent.Find("RoadLine").gameObject.GetComponent<CurvySpline>()
            .GetNextControlPoint(transform.parent.parent.Find("RoadLine").gameObject.GetComponent<CurvySpline>().LastSegment);
        meshCollider= this.gameObject.GetComponent<MeshCollider>();
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("RoadCheck"))
        {
            positionNearestWithMouseIndicator = transform.parent.parent.Find("RoadLine").gameObject.GetComponent<CurvySpline>().GetNearestPoint(other.gameObject.transform.position, Space.World);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("RoadCheck"))
        {
            positionNearestWithMouseIndicator = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        if (meshCollider == null)
            return;

        // Lấy bounding box của MeshCollider
        Bounds bounds = meshCollider.bounds;

        // Vẽ bounding box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
