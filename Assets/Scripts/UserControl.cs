using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// This script handle all the control code, so detecting when the users click on a unit or building and selecting those
/// If a unit is selected it will give the order to go to the clicked point or building when right clicking.
/// </summary>
public class UserControl : MonoBehaviour
{
    public Camera GameCamera;
    public float PanSpeed = 10.0f;
    public GameObject Marker;
    
    private Unit selected;

    private float minZoom = 20;
    private float maxZoom = 3;
    
    
    private void Start()
    {
        Marker.SetActive(false);
    }
    public void HandleSelection()
    {
        var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            //the collider could be children of the unit, so we make sure to check in the parent
            var unit = hit.transform.GetComponentInParent<Unit>();
            selected = unit;


            //check if the hit object have a IUIInfoContent to display in the UI
            //if there is none, this will be null, so this will hid the panel if it was displayed
            var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
            UIMainScene.Instance.SetNewInfoContent(uiInfo);
        }
    }
    private void HandleAction()
    {
        var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var building = hit.collider.GetComponentInParent<Building>();

            if (building != null)
            {
                selected.GoTo(building);
            }
            else
            {
                selected.GoTo(hit.point);
            }
        }
    }

    private void Update()
    {
        var gameCameraTransform = GameCamera.transform;
        Vector2 move = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        gameCameraTransform.position += PanSpeed * Time.deltaTime * new Vector3(move.y, 0, -move.x);
        Vector2 zoom = Input.mouseScrollDelta;
        
        if ((GameCamera.transform.position.y > maxZoom || zoom.y < 0) &&
            (GameCamera.transform.position.y < minZoom || zoom.y > 0))
        {
            gameCameraTransform.position += gameCameraTransform.forward * (zoom.y * (PanSpeed * 10 * Time.deltaTime));
        }


        if (Input.GetMouseButtonDown(0))
        {
            HandleSelection();
        }
        else if (selected != null && Input.GetMouseButtonDown(1))
        {//right click give order to the unit
            HandleAction();
        }

        MarkerHandling();
    }

    // Handle displaying the marker above the unit that is currently selected (or hiding it if no unit is selected)
    void MarkerHandling()
    {
        if (selected == null && Marker.activeInHierarchy)
        {
            Marker.SetActive(false);
            Marker.transform.SetParent(null);
        }
        else if (selected != null && Marker.transform.parent != selected.transform)
        {
            Marker.SetActive(true);
            Marker.transform.SetParent(selected.transform, false);
            Marker.transform.localPosition = Vector3.zero;
        }    
    }
}
