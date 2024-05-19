using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public Collider objectCollider; // Đặt collider của đối tượng của bạn vào đây
    public MeshCollider mapMeshCollider; // Đặt mesh collider của map vào đây
    public float skinWidth = 0.1f; // Độ dày của skin để tránh việc xuyên qua collider

    private void Update()
    {
        Debug.Log("Can Move: " + CanMove(Settings.currentPositionRay));
    }

    // Hàm này kiểm tra xem vị trí tiếp theo có gặp va chạm với mesh collider hay không
    public bool CanMove(Vector3 nextPosition)
    {
        if (objectCollider == null || mapMeshCollider == null)
        {
            Debug.LogError("ColliderManager: Collider or MeshCollider is not assigned.");
            return false;
        }

        // Tính toán vị trí mới với một skin để tránh việc xuyên qua collider
        Vector3 direction = nextPosition - objectCollider.transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        RaycastHit hit;

        // Kiểm tra va chạm với mesh collider
        if (Physics.Raycast(objectCollider.transform.position, direction, out hit, distance + skinWidth, mapMeshCollider.gameObject.layer))
        {
            return false; // Không thể di chuyển đến vị trí tiếp theo vì gặp va chạm với mesh collider
        }

        return true; // Có thể di chuyển đến vị trí tiếp theo mà không gặp va chạm với mesh collider
    }
}