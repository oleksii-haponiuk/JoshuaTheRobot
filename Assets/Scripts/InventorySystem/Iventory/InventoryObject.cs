using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Inventory", menuName = "InventorySystem/Inventory")
,System.Serializable]
public class InventoryObject: ScriptableObject
{
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public void AddItem(ItemObject item, int amount){
        bool hasItem = false;
        for(int i = 0; i < inventory.Count; i++){
            if(inventory[i].item == item){
                inventory[i].AddAmount(amount);
                hasItem = true;
                break;
            }
        }
        if(!hasItem) inventory.Add(new InventorySlot(item,amount));
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject item, int amount){
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int amount){
        this.amount = amount;
    }
}
