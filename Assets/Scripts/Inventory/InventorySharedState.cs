using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySharedState : MonoBehaviour
{
    [Header("State")]
    public int selectedItemIndex;
    public List<ItemData> items;
    public List<GameObject> spawnedItems;
    public Quaternion previousSpawnedItemRotation;

    [Header("References")]
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;

    public ItemData GetSelectedItem()
    {
        return items[selectedItemIndex];
    }

    public GameObject GetSelectedSpawnedItem()
    {
        return spawnedItems[selectedItemIndex];
    }

    public void CachePreviousSpawnedItemRotation()
    {
        previousSpawnedItemRotation = GetSelectedSpawnedItem().transform.rotation;
    }

    public void UpdateText()
    {
        _itemNameText.SetText(GetSelectedItem().itemName);
        _itemDescriptionText.SetText(GetSelectedItem().itemDescription);
    }

    public void ClearText()
    {
        _itemNameText.ClearMesh();
        _itemDescriptionText.ClearMesh();
    }
}
