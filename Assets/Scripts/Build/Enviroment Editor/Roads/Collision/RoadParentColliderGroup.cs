using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadParentColliderGroup : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Xử lý va chạm khi một đối tượng va chạm vào bất kỳ Mesh Collider nào trong group
        Debug.Log("Đã va chạm với một trong các Mesh Collider con trong group: " + collision.collider.name);
    }
}
