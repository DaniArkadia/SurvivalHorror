using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInspector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventorySharedState _inventoryState;
    [SerializeField] private InventoryNavigation _inventoryController;
    [SerializeField] private Camera _inventoryCamera;

    [Header("Settings")]
    [SerializeField] private float _itemRotationSpeed;
    [SerializeField] private float _cameraZoomSpeed;
    [SerializeField] private float _cameraZoomFOVTarget;

    private float _oldCameraFOV;
    private bool _isExiting;

    private float _startBuffer;

    private void OnEnable()
    {
        _startBuffer = 0.3f;
        _oldCameraFOV = _inventoryCamera.fieldOfView;
        _isExiting = false;
        _inventoryState.CachePreviousSpawnedItemRotation();
        _inventoryState.ClearText();
    }

    private void Update()
    {
        if (_isExiting) return;
        HandleItemRotation();
        ZoomInCamera();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartCoroutine(ZoomOut());
            _isExiting = true;
        }
    }


    private void HandleItemRotation()
    {
        var inspectionTarget = _inventoryState.GetSelectedSpawnedItem();

        if (_startBuffer > 0)
        {
            _startBuffer -= Time.deltaTime;
            return;
        }

        if (inspectionTarget == null)
        {
            Debug.LogWarning("Inspection target not set.");
            return;
        }

        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        var rollInput = Input.GetAxisRaw("Roll");

        if (horizontalInput != 0)
        {
            inspectionTarget.transform.RotateAround(inspectionTarget.transform.position, Vector3.up, -horizontalInput * _itemRotationSpeed * Time.deltaTime);
        }

        if (verticalInput != 0)
        {
            inspectionTarget.transform.RotateAround(inspectionTarget.transform.position, Vector3.right, -verticalInput * _itemRotationSpeed * Time.deltaTime);
        }

        if (rollInput != 0)
        {
            inspectionTarget.transform.RotateAround(inspectionTarget.transform.position, Vector3.forward, -rollInput * _itemRotationSpeed * Time.deltaTime);
        }
    }

    private void ZoomInCamera()
    {
        _inventoryCamera.fieldOfView = Mathf.Lerp(_inventoryCamera.fieldOfView, _cameraZoomFOVTarget, _cameraZoomSpeed * Time.deltaTime);
    }

    private IEnumerator ZoomOut()
    {
        var inspectionTarget = _inventoryState.GetSelectedSpawnedItem();

        while (Mathf.Abs(_inventoryCamera.fieldOfView - _oldCameraFOV) > 0.03f)
        {
            _inventoryCamera.fieldOfView = Mathf.Lerp(_inventoryCamera.fieldOfView, _oldCameraFOV, _cameraZoomSpeed * Time.deltaTime);
            inspectionTarget.transform.rotation = Quaternion.Slerp(inspectionTarget.transform.rotation, _inventoryState.previousSpawnedItemRotation, 10f * Time.deltaTime);
            yield return null;
        }
        _inventoryController.gameObject.SetActive(true);
        gameObject.SetActive(false);
        yield return null;
    }
}
