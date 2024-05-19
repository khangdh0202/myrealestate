using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionCheckCollision : MonoBehaviour
{
    private void Awake()
    {
        Settings.EndConstructionExit = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            if (other.gameObject == ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
            {
                Settings.EndConstructionExit = false;
            }
            else
            {
                Settings.insideConstructionOther = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Building"))
        {
            if (other.gameObject == ContructionController.Instance.ContructionBuild.currentBuidingInMouse)
            {
                //Debug.Log("333");
                PlacementSystem.Instance.CancelSnap();
                Settings.EndConstructionExit = true;
            }
            else
            {
                Settings.insideConstructionOther = false;
            }
        }
    }
}
