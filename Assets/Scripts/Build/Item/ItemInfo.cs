using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private ObjectPool pool;

    public ObjectPool ItemPool { get => pool; set => pool = value; }

}
