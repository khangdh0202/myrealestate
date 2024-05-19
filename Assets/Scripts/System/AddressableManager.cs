using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[Serializable]
public class AssetReferenceConstruction : AssetReferenceT<GameObject>
{
    public AssetReferenceConstruction(string guid) : base(guid) { }
}

public class AddressableManager : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceConstruction constructionAsset;
    [SerializeField] AsyncOperationHandle handle;
    [SerializeField] Text textDisplay;

    [SerializeField] private List<GameObject> constructionList = new List<GameObject>();

    AsyncOperationHandle<GameObject> opHandle;

    /*public IEnumerator Start()
    {
        opHandle = Addressables.LoadAssetAsync<GameObject>(constructionAsset);
        textDisplay.text = "Đang download asset bundle";
        yield return opHandle;

        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = opHandle.Result;
            Instantiate(obj, transform);
            textDisplay.text = "Download thành công! " + obj.name;
        }
        else
        {
            textDisplay.text = "Đã có lỗi trong khi download! ";
        }
    }

    void OnDestroy()
    {
        Addressables.Release(opHandle);
    }*/
}
