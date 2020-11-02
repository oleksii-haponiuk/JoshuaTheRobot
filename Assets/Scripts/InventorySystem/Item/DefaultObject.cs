using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultObject", menuName = "InventorySystem/ItemObject/DefaultObject")]
public class DefaultObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Default;
    }
}
