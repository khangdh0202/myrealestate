using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int poolSize = 0;

    private List<GameObject> objectPool = new List<GameObject>();
    private GameObject pool;

    public GameObject ObjectPrefab {set => objectPrefab = value; }
    public GameObject PoolFolder { get => pool; }

    public void InitializePool()
    {
        pool = new GameObject("PoolGrid");
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, new Vector3(0, 0, 0), objectPrefab.transform.rotation);
            obj.transform.parent = PoolFolder.transform;
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    public GameObject GetObjectFromPool()
    {
        foreach(GameObject obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Nếu cách đối tượng trong pool không sẵn sàng, tăng kích thước pool và trả về đối tượng
        GameObject newObj = Instantiate(objectPrefab, new Vector3(0, 0, 0), objectPrefab.transform.rotation);
        newObj.SetActive(true);
        newObj.transform.parent = PoolFolder.transform;
        objectPool.Add(newObj);

        poolSize++;

        return newObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.parent = PoolFolder.transform;
        obj.SetActive(false);
    }
}
