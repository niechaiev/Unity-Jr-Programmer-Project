using System;
using System.Collections.Generic;
using System.Linq;
using Buildings;
using DefaultNamespace;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handle all the control code, so detecting when the users click on a unit or building and selecting those
/// If a unit is selected it will give the order to go to the clicked point or building when right clicking.
/// </summary>
public class UserControl : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private LayerMask clickable;

    public Camera GameCamera;
    public float PanSpeed = 10.0f;

    private List<Unit> selected;

    private float minZoom = 20;
    private float maxZoom = 3;

    private PlayerInput playerInput;
    private InputAction movement;
    private Transform gameCameraTransform;
    private Vector2 moveComposite;

    private void Start()
    {
        gameCameraTransform = GameCamera.transform;
        selected = new List<Unit>();
        playerInput = new PlayerInput();
        playerInput.Enable();
        movement = playerInput.Player.MoveCamera;
        inputReader.OnSelectPerformed += HandleSelection;
        inputReader.OnOrderPerformed += HandleOrder;
        inputReader.OnZoomCameraPerformed += HandleCameraZoom;
    }

    private void HandleCameraMovement()
    {
        gameCameraTransform.position += PanSpeed * Time.deltaTime *
                                        new Vector3(inputReader.MoveComposite.y, 0, -inputReader.MoveComposite.x);
    }

    private void HandleCameraZoom(InputAction.CallbackContext callbackContext)
    {
        var zoom = callbackContext.ReadValue<float>();
        if ((GameCamera.transform.position.y > maxZoom || zoom < 0) &&
            (GameCamera.transform.position.y < minZoom || zoom > 0))
        {
            gameCameraTransform.position += gameCameraTransform.forward * (zoom * (PanSpeed * 10 * Time.deltaTime));
        }
    }

    public void HandleSelection(InputAction.CallbackContext callbackContext)
    {
        /*RaycastHit m_Hit;

        var m_HitDetect = Physics.BoxCast(Mouse.current.position.ReadValue(), transform.localScale, transform.forward,
            out m_Hit, transform.rotation, 100f);
        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);
        }*/

        var mousePosition = Mouse.current.position.ReadValue();
        var ray = GameCamera.ScreenPointToRay(mousePosition);
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

    private void DeselectAll()
    {
        foreach (var selectedUnit in selected)
            selectedUnit.ToggleSelection(false);

        selected.Clear();
    }

    private void HandleOrder(InputAction.CallbackContext callbackContext)
    {
        // Debug.Log(callbackContext.action);
        if (!selected.Any()) return;
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = GameCamera.ScreenPointToRay(mousePosition);
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

    private void Update()
    {
        HandleCameraMovement();
    }
}