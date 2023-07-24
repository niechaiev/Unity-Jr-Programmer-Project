using System.Collections.Generic;
using System.Linq;
using Buildings;
using Units;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handle all the control code, so detecting when the users click on a unit or building and selecting those
/// If a unit is selected it will give the order to go to the clicked point or building when right clicking.
/// </summary>
public class UserControl : MonoBehaviour
{
    public Camera GameCamera;
    public float PanSpeed = 10.0f;
    public GameObject Marker;

    private List<Unit> selected;

    private float minZoom = 20;
    private float maxZoom = 3;

    private PlayerInput playerInput;

    private void Start()
    {
        //Marker.SetActive(false);
        selected = new List<Unit>();
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.Player.Select.performed += HandleSelection;
        playerInput.Player.Order.performed += HandleOrder;
        playerInput.Player.MoveCamera.started += HandleCameraMovement;
    }

    private void HandleCameraMovement(InputAction.CallbackContext obj)
    {
        var gameCameraTransform = GameCamera.transform;

        var move = obj.ReadValue<Vector2>();
        gameCameraTransform.position += PanSpeed * Time.deltaTime * new Vector3(move.y, 0, -move.x);
        var zoom = Input.mouseScrollDelta;

        if ((GameCamera.transform.position.y > maxZoom || zoom.y < 0) &&
            (GameCamera.transform.position.y < minZoom || zoom.y > 0))
        {
            gameCameraTransform.position += gameCameraTransform.forward * (zoom.y * (PanSpeed * 10 * Time.deltaTime));
        }
    }

    public void HandleSelection(InputAction.CallbackContext callbackContext)
    {
        //Debug.Log(callbackContext.action);
        RaycastHit m_Hit;

        var m_HitDetect = Physics.BoxCast(Mouse.current.position.ReadValue(), transform.localScale, transform.forward,
            out m_Hit, transform.rotation, 100f);
        if (m_HitDetect)
        {
            //Output the name of the Collider your Box hit
            Debug.Log("Hit : " + m_Hit.collider.name);
        }

        var mousePosition = Mouse.current.position.ReadValue();
        var ray = GameCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            selected.Clear();
            //the collider could be children of the unit, so we make sure to check in the parent
            var unit = hit.transform.GetComponentInParent<Unit>();
            selected.Add(unit);


            //check if the hit object have a IUIInfoContent to display in the UI
            //if there is none, this will be null, so this will hid the panel if it was displayed
            var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
            UIMainScene.Instance.SetNewInfoContent(uiInfo);
        }
    }

    void FixedUpdate()
    {
        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
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
        MarkerHandling();
    }


    // Handle displaying the marker above the unit that is currently selected (or hiding it if no unit is selected)
    void MarkerHandling()
    {
        /*if (selected == null && Marker.activeInHierarchy)
        {
            Marker.SetActive(false);
            Marker.transform.SetParent(null);
        }
        else if (selected != null && Marker.transform.parent != selected.transform)
        {
            Marker.SetActive(true);
            Marker.transform.SetParent(selected.transform, false);
            Marker.transform.localPosition = Vector3.zero;
        }  */
    }
}