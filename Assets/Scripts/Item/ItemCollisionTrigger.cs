using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollisionTrigger : MonoBehaviour
{
    public Material errorBuilding; // Material highlight đỏ
    private Material originalMaterial; // Material gốc

    private Dictionary<GameObject, Material[]> buildingMaterials = new Dictionary<GameObject, Material[]>();

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Building"))
        {
            DontCanBuild(other);
        }*/
    }
    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject.CompareTag("Building"))
        {
            DontCanBuild(other);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        // Kiểm tra xem building có còn va chạm với building khác không?
        // Nếu không, bật canBuild thành true và cho phép xây dựng, trả lại material cho building bị chạm thảnh origin
        /*if (other.gameObject.CompareTag("Building"))
        {
            CanBuild(other);
        }*/
    }

    private void DontCanBuild(Collider other)
    {
        
            // kiểm tra xem khi đặt building có va chạm phải một building nào không?
            // nếu có thì sẽ cấm xây dựng building và building bị va chạm sẽ có material erroBuilding
            if (Settings.canBuild)
            {
                Settings.canBuild = false;

                if (!buildingMaterials.ContainsKey(other.gameObject))
                {
                    MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
                    originalMaterial = meshRenderer.material;

                    Material[] originalMaterials = meshRenderer.materials;
                    Material[] modifiedMaterials = new Material[originalMaterials.Length + 1];


                    System.Array.Copy(originalMaterials, modifiedMaterials, originalMaterials.Length);
                    modifiedMaterials[originalMaterials.Length] = errorBuilding;

                    buildingMaterials.Add(other.gameObject, originalMaterials);
                    meshRenderer.materials = modifiedMaterials;
                }
            }
    }
    private void CanBuild(Collider other)
    {
        if (!Settings.canBuild)
        {
            Settings.canBuild = true;

            if (buildingMaterials.TryGetValue(other.gameObject, out Material[] originalMaterials))
            {
                MeshRenderer meshRenderer = other.gameObject.GetComponent<MeshRenderer>();
                Material[] materials = new Material[meshRenderer.materials.Length];
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    materials[i] = null;
                }
                materials[0] = originalMaterial;
                meshRenderer.materials = originalMaterials;
                buildingMaterials.Remove(other.gameObject);
            }
        }
    }
}
