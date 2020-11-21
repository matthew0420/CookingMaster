using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour
{

    public bool isItemOnPlate;
    private GameObject itemOnPlate;
    
    public void AddToPlate(GameObject ItemToAdd)
    {
        ItemToAdd.transform.SetParent(this.gameObject.transform);
        isItemOnPlate = true;
        itemOnPlate = ItemToAdd;
    }

    public void RemoveFromPlate(GameObject playerObject)
    {
        itemOnPlate.transform.parent = playerObject.gameObject.transform.GetChild
            (playerObject.GetComponent<PlayerScript>().Inventory.Count).gameObject.transform;
        playerObject.GetComponent<PlayerScript>().Inventory.Add(itemOnPlate);
        isItemOnPlate = false;
    }
}
