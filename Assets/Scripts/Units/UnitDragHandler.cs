using UnityEngine;

public class UnitDragHandler : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    private void Update() => HandleMouseInput();

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) OnPointerDown(GetMouseWorldPosition());
        else if (Input.GetMouseButton(0) && isDragging) OnPointerDrag(GetMouseWorldPosition());
        else if (Input.GetMouseButtonUp(0)) OnPointerUp();
    }

    private void OnPointerDown(Vector3 pointerPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                offset = transform.position - pointerPosition;
                isDragging = true;
            }
        }
    }

    private void OnPointerDrag(Vector3 pointerPosition) => transform.position = pointerPosition + offset;

    private void OnPointerUp()
    {
        if (isDragging)
        {
            isDragging = false;
            SnapToNearestTile();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance)) return ray.GetPoint(distance);
        return Vector3.zero;
    }

    private void SnapToNearestTile()
    {
        Tile nearestTile = unit.FindNearestTile();
        if (nearestTile != null)
        {
            Unit otherUnit = nearestTile.GetUnit();
            if (otherUnit != null && 
                otherUnit.UnitType == unit.UnitType &&
                otherUnit.UnitLevel == unit.UnitLevel &&
                UnitMergeHandler.Instance.TryMergeUnits(unit, otherUnit))
            {
                return;
            }

            if (nearestTile.CanSpawn)
            {
                transform.position = new Vector3(nearestTile.transform.position.x, transform.position.y, nearestTile.transform.position.z);
            
                if (unit.CurrentTile != null) unit.CurrentTile.SetUnit(null);
                nearestTile.SetUnit(unit);
                unit.CurrentTile = nearestTile;
            }
            else
            {
                transform.position = new Vector3(unit.CurrentTile.transform.position.x, transform.position.y, unit.CurrentTile.transform.position.z);
            }
        }
    }
}