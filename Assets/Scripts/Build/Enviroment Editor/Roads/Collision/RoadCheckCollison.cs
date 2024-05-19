using UnityEngine;

public class RoadCheckCollison : MonoBehaviour
{
    private void Awake()
    {
        Settings.EndRoadExit = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Road"))
        {
            Settings.EndRoadExit = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Road"))
        {

                Settings.RoadSnapToRoad = false;
                PlacementSystem.Instance.directionRoadSnap = "";
                //Debug.Log("222");
                Settings.EndRoadExit = true;
        }
    }
}
