using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public GameObject itemPrefab;
    public string itemName;
    public string itemDescription;
}
