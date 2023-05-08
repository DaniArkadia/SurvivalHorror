using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryNavigation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _pivot;
    [SerializeField] private InventorySharedState _inventoryState;
    [SerializeField] private InventoryContextMenu _contextMenu;

    [Header("Settings")]
    [SerializeField] private List<Quaternion> _pivotRotations;
    [SerializeField] private float _pivotRotationSpeed;

    [Header("In Game")]
    [SerializeField] private float _selectedItemRotationSpeed;


    private void Start()
    {
        SpawnItems();
    }

    private void OnEnable()
    {
        _inventoryState.UpdateText();
    }

    private void Update()
    {
        HandleInput();
        RotateToTargetItem();
        RotateSelectedItem();
    }


    private void SpawnItems()
    {
        var rotationAngle = 360 / _inventoryState.items.Count;

        foreach (var item in _inventoryState.items)
        {
            var itemInstance = Instantiate(item.itemPrefab);
            _inventoryState.spawnedItems.Add(itemInstance);
            itemInstance.transform.SetParent(_pivot);
            itemInstance.transform.position = _pivot.position + Vector3.forward * 10f;
            _pivotRotations.Add(_pivot.rotation);
            _pivot.Rotate(0, rotationAngle, 0);
        }
    }

    private void HandleInput()
    {
        var textNeedsUpdating = false;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_inventoryState.selectedItemIndex == 0)
            {
                _inventoryState.selectedItemIndex = _inventoryState.items.Count - 1;
            }
            else
            {
                _inventoryState.selectedItemIndex--;
            }
            textNeedsUpdating = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_inventoryState.selectedItemIndex == _inventoryState.items.Count - 1)
            {
                _inventoryState.selectedItemIndex = 0;
            }
            else
            {
                _inventoryState.selectedItemIndex++;
            }
            textNeedsUpdating = true;
        }

        if (textNeedsUpdating)
        {
            _inventoryState.UpdateText();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Quaternion.Angle(_pivot.transform.rotation, _pivotRotations[_inventoryState.selectedItemIndex]) < 3)
            {
                _pivot.rotation = _pivotRotations[_inventoryState.selectedItemIndex];
                _contextMenu.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void RotateToTargetItem()
    {
        _pivot.rotation = Quaternion.Slerp(_pivot.rotation, _pivotRotations[_inventoryState.selectedItemIndex], _pivotRotationSpeed * Time.deltaTime);
    }

    private void RotateSelectedItem()
    {
        _inventoryState.GetSelectedSpawnedItem().transform.Rotate(0, _selectedItemRotationSpeed * Time.deltaTime, 0);
    }
}
