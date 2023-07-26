using System.Collections.Generic;
using Buildings;
using DefaultNamespace;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class UserControl : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private LayerMask clickable;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Camera gameCamera;
    private Selector selector;

    private readonly float panSpeed = 10.0f;
    private readonly float minZoom = 20;
    private readonly float maxZoom = 3;

    private Transform gameCameraTransform;
    private Vector2 startMousePosition;


    [Inject]
    private void Construct(Selector selectorRef)
    {
        selector = selectorRef;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        gameCameraTransform = gameCamera.transform;

        inputReader.OnSelectStarted += HandleSelectStarted;
        inputReader.OnSelectPerformed += HandleSelectPerformed;
        inputReader.OnSelectCanceled += HandleSelectCanceled;
        inputReader.OnOrderStarted += HandleOrder;
        inputReader.OnZoomCameraStarted += HandleCameraZoom;
        inputReader.OnMultiSelectStarted += HandleMultiSelect;
    }

    private void HandleMultiSelect(bool state)
    {
        selector.IsMultiSelect = state;
    }

    private void HandleCameraMovement()
    {
        if (inputReader.MoveComposite == Vector2.zero) return;

        gameCameraTransform.position += panSpeed * Time.deltaTime *
                                        new Vector3(inputReader.MoveComposite.y, 0, -inputReader.MoveComposite.x);
    }

    private void HandleCameraZoom(InputAction.CallbackContext callbackContext)
    {
        var zoom = callbackContext.ReadValue<float>();
        if ((gameCamera.transform.position.y > maxZoom || zoom < 0) &&
            (gameCamera.transform.position.y < minZoom || zoom > 0))
        {
            gameCameraTransform.position += gameCameraTransform.forward * (zoom * (panSpeed * 10 * Time.deltaTime));
        }
    }

    public void HandleSelectStarted(InputAction.CallbackContext callbackContext)
    {
        selectionBox.sizeDelta = Vector2.zero;
        selectionBox.gameObject.SetActive(true);
        startMousePosition = Mouse.current.position.ReadValue();
    }

    private void HandleSelectPerformed(InputAction.CallbackContext obj)
    {
        ResizeSelectionBox(obj.ReadValue<Vector2>());
    }

    private void HandleSelectCanceled(InputAction.CallbackContext obj)
    {
        ConfirmSelectionBox();
        selectionBox.sizeDelta = Vector2.zero;
        selectionBox.gameObject.SetActive(false);

        startMousePosition = Mouse.current.position.ReadValue();
        var ray = gameCamera.ScreenPointToRay(startMousePosition);

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, clickable))
        {
            var unit = hit.transform.GetComponentInParent<Unit>();
            selector.Select(unit);

            var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
            UIMainScene.Instance.SetNewInfoContent(uiInfo);
        }
    }

    private void HandleOrder(InputAction.CallbackContext callbackContext)
    {
        // Debug.Log(callbackContext.action);
        if (selector.SelectedUnits.Count == 0) return;
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = gameCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var building = hit.collider.GetComponentInParent<Building>();
            foreach (var selectedUnit in selector.SelectedUnits)
            {
                if (building != null)
                {
                    selectedUnit.GoTo(building);
                }
                else
                {
                    selectedUnit.GoTo(hit.point);
                }
            }
        }
    }

    private void ResizeSelectionBox(Vector2 mousePosition)
    {
        var width = Mouse.current.position.x.ReadValue() - startMousePosition.x;
        var height = Mouse.current.position.y.ReadValue() - startMousePosition.y;

        selectionBox.anchoredPosition = startMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));


    }

    private void ConfirmSelectionBox()
    {
        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        var unitsToSelect = new List<Unit>();
        foreach (var availableUnit in selector.AvailableUnits)
        {
            if (IsUnitInBounds(gameCamera.WorldToScreenPoint(availableUnit.transform.position), bounds))
            {
                unitsToSelect.Add(availableUnit);
            }
        }
        selector.Select(unitsToSelect);
    }

    private bool IsUnitInBounds(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x && position.x < bounds.max.x
                                         && position.y > bounds.min.y && position.y < bounds.max.y;
    }

    private void Update()
    {
        HandleCameraMovement();
    }
}