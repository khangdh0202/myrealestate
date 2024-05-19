/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KhangLibrary;

public class BuildOnGrid : MonoBehaviour
{

    private Material objectMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Toi da enter building");
            Settings.canBuild = false;
            OnGrid(Color.red);
            return;
        }

        if (other.gameObject.transform.Find(Settings.gridName).gameObject.layer == 10)
        {
            Bounds boundOther = other.gameObject.GetComponent<BoxCollider>().bounds; // Bounds của grid other
            Bounds boundsCellIndicator = PlacementSystem.Instance.cellIndicator.gameObject.GetComponent<BoxCollider>().bounds; // Bounds của grid cellIndicator


            if (PlacementSystem.Instance.mouseIndicator.gameObject.transform.Find(Settings.gridName).gameObject.layer == 10)
            {
                return;
            }

            // Kiểm tra xem boundOther có hoàn toàn nằm trong boundsCellIndicator không
            if (boundsCellIndicator.Contains(boundOther.min) && boundsCellIndicator.Contains(boundOther.max))
            {
                Settings.onGrid = true;
                OnGrid(Color.green);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Toi da stay building");
            Settings.canBuild = false;
            OnGrid(Color.red);
            return;
        }
        if (other.gameObject.layer == 10)
        {
            Bounds boundOther = other.gameObject.GetComponent<BoxCollider>().bounds; // Bounds của grid other
            Bounds boundsCellIndicator = PlacementSystem.Instance.cellIndicator.gameObject.GetComponent<BoxCollider>().bounds; // Bounds của grid cellIndicator

            DirectionGridCurrentToGridInMap(other.gameObject);


            if (PlacementSystem.Instance.mouseIndicator.gameObject.transform.Find(Settings.gridName).gameObject == other.gameObject)
            {
                return;
            }

            // Kiểm tra xem boundCellIndicator có hoàn toàn nằm trong boundOther không
            if (boundOther.Contains(boundsCellIndicator.min) && boundOther.Contains(boundsCellIndicator.max))
            {
                // Grid boundCellIndicator hoàn toàn nằm trong Grid boundOther
                Settings.onGrid = true;
                PlacementSystem.Instance.SetColorCellIndicatorMaterial(Color.green);
                //OnGrid(Color.green);
            }
            else if (boundsCellIndicator.max.x < boundOther.min.x || boundsCellIndicator.min.x > boundOther.max.x ||
                     boundsCellIndicator.max.y < boundOther.min.y || boundsCellIndicator.min.y > boundOther.max.y ||
                     boundsCellIndicator.max.z < boundOther.min.z || boundsCellIndicator.min.z > boundOther.max.z)
            {
                // Grid boundCellIndicator và Grid boundOther không chạm nhau hoàn toàn
                Settings.onGrid = false;
                PlacementSystem.Instance.SetColorCellIndicatorMaterial(Color.green);
                //NotTouchingGrid();
            }
            else
            {
                // Grid boundCellIndicator chưa hoàn toàn ra khỏi Grid boundOther
                Settings.onGrid = false;
                PlacementSystem.Instance.SetColorCellIndicatorMaterial(Color.red);
                //OnGrid(Color.red);
            }


            *//*// Kiểm tra xem boundCellIndicator có hoàn toàn nằm trong boundOther không
            if (boundOther.Contains(boundsCellIndicator.min) && boundOther.Contains(boundsCellIndicator.max))
            {
                // Grid boundCellIndicator hoàn toàn nằm trong Grid boundOther
                Settings.onGrid = true;
                OnGrid(Color.green);
            }
            else
            {
                // Grid boundCellIndicator chưa hoàn toàn ra khỏi Grid boundOther
                Settings.onGrid = false;
                OnGrid(Color.red);
            }*//*
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Toi da exit builging");
            Settings.canBuild = true;
            //OnGrid(Color.green);
            return;
        }
        if (other.gameObject.layer == 10)
        {
            if (PlacementSystem.Instance.mouseIndicator.gameObject.transform.Find(Settings.gridName).gameObject == other.gameObject)
            {
                return;
            }

            Settings.onGrid = false;
            PlacementSystem.Instance.SetColorCellIndicatorMaterial(Color.green);
            //OnGrid(Color.red);
        }
    }

    private void OnGrid(Color color)
    {
        objectMaterial = PlacementSystem.Instance.cellIndicator.gameObject.GetComponent<MeshRenderer>().material;
        UnityEngine.Color colorMaterial = objectMaterial.GetColor("_Color");
        colorMaterial = color;
        objectMaterial.SetColor("_Color", colorMaterial);
    }

    private void DirectionGridCurrentToGridInMap(GameObject gameObject)
    {
        // Tạo một đối tượng từ lớp LineIntersection3D
        LineIntersection3D lineIntersection3D = new LineIntersection3D();

        LineIntersection3D.Line line1;

        // các cạnh của Grid trên map 
        line1 = new LineIntersection3D.Line { point1 = new Vector3(gameObject.transform.position.x + (gameObject.transform.localScale.x * 10) / 2, 0, gameObject.transform.position.z + (gameObject.transform.localScale.z * 10) / 2), point2 = new Vector3(gameObject.transform.position.x + (gameObject.transform.localScale.x * 10) / 2, 0, gameObject.transform.position.z - (gameObject.transform.localScale.x * 10) / 2) };

        // Đường thẳng từ grid hiện tại đến grid trên map 
        LineIntersection3D.Line line2 = new LineIntersection3D.Line { point1 = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z), point2 = new Vector3(transform.position.x, 0, transform.position.z) };

        if (lineIntersection3D.DoLinesIntersect(line1, line2))
        {
            Debug.Log("Hai đường thẳng chạm nhau.");
        }
        else
        {
            Debug.Log("Hai đường thẳng không chạm nhau.");
        }
    }
}
*/