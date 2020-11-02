using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<InventorySlot,GameObject> itemsDisplayed;
    
    private void Start()
    {
        CreateSlots();
    }

    public void CreateSlots(){
        itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
        for (int i = 0; i < inventory.inventory.Count; i++)
        {
            GameObject obj = Instantiate(inventory.inventory[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<Text>().text = inventory.inventory[i].amount.ToString();

            itemsDisplayed.Add(inventory.inventory[i], obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)),
            Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0f);
    }
}
