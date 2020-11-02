using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Default
}

public class ItemObject : ScriptableObject
{
    public string itemName;
    public int id;
    public GameObject prefab;
    public ItemType type;
}
