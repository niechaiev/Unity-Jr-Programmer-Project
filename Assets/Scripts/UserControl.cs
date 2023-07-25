using System.Collections.Generic;
using Buildings;
using DefaultNamespace;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserControl : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private LayerMask clickable;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Camera gameCamera;

    private readonly float panSpeed = 10.0f;
    private readonly float minZoom = 20;
    private readonly float maxZoom = 3;

    private List<Unit> selected;
    private bool isMultiSelect;
    private bool isSelectPerforming;
    private Transform gameCameraTransform;
    private Vector2 startMousePosition;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        gameCameraTransform = gameCamera.transform;
        selected = new List<Unit>();

        inputReader.OnSelectStarted += HandleSelectStarted;
        inputReader.OnSelectPerformed += HandleSelectPerformed;
        inputReader.OnSelectCanceled += HandleSelectCanceled;
        inputReader.OnOrderStarted += HandleOrder;
        inputReader.OnZoomCameraStarted += HandleCameraZoom;
        inputReader.OnMultiSelectStarted += HandleMultiSelect;
    }

    private void HandleMultiSelect(bool state)
    {
        isMultiSelect = state;
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
        var ray = gameCamera.ScreenPointToRay(startMousePosition);

        if (!isMultiSelect)
            DeselectAll();

        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, clickable))
        {
            //the collider could be children of the unit, so we make sure to check in the parent

            var unit = hit.transform.GetComponentInParent<Unit>();
            if (unit != null)
            {
                selected.Add(unit);
                unit.ToggleSelection(true);
            }

            //check if the hit object have a IUIInfoContent to display in the UI
            //if there is none, this will be null, so this will hid the panel if it was displayed
            var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
            UIMainScene.Instance.SetNewInfoContent(uiInfo);
        }
    }

    private void HandleSelectPerformed(InputAction.CallbackContext obj)
    {
        isSelectPerforming = true;
        ResizeSelectionBox(obj.ReadValue<Vector2>());
        
    }

    private void HandleSelectCanceled(InputAction.CallbackContext obj)
    {
        selectionBox.sizeDelta = Vector2.zero;
        selectionBox.gameObject.SetActive(false);
        isSelectPerforming = false;
    }

    private void DeselectAll()
    {
        foreach (var selectedUnit in selected)
            selectedUnit.ToggleSelection(false);

        selected.Clear();
    }

    private void HandleOrder(InputAction.CallbackContext callbackContext)
    {
        // Debug.Log(callbackContext.action);
        if (selected.Count == 0) return;
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = gameCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var building = hit.collider.GetComponentInParent<Building>();
            foreach (var selectedUnit in selected)
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
        Debug.Log(mousePosition);
        float width = Input.mousePosition.x - startMousePosition.x;
        float height = Input.mousePosition.y - startMousePosition.y;
        
        

        selectionBox.anchoredPosition = startMousePosition + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);

        /*for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {
            if (UnitIsInSelectionBox(
                    Camera.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))
            {
                if (!SelectionManager.Instance.IsSelected(SelectionManager.Instance.AvailableUnits[i]))
                {
                    newlySelectedUnits.Add(SelectionManager.Instance.AvailableUnits[i]);
                }

                deselectedUnits.Remove(SelectionManager.Instance.AvailableUnits[i]);
            }
            else
            {
                deselectedUnits.Add(SelectionManager.Instance.AvailableUnits[i]);
                newlySelectedUnits.Remove(SelectionManager.Instance.AvailableUnits[i]);
            }
        }*/
    }

    private void Update()
    {
        HandleCameraMovement();
        
    }
}