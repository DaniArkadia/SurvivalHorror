using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject _contextMenuPanel;
    [SerializeField] private ItemInspector _itemInspector;

    private void OnEnable()
    {
        _contextMenuPanel.SetActive(true);
    }

    private void OnDisable()
    {
        _contextMenuPanel.SetActive(false);
    }

    public void OnSelectInspect()
    {
        _itemInspector.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
