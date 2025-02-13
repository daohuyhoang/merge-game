using UnityEngine;

[RequireComponent(typeof(UnitModelHandler))]
public class UnitDragHandler : MonoBehaviour
{
    public int unitLevel = 1;
    public string unitType;

    private Vector3 offset;
    private bool isDragging = false;
    public Tile currentTile = null;
    private UnitModelHandler modelHandler;

    void Start()
    {
        modelHandler = GetComponent<UnitModelHandler>();
        currentTile = FindNearestTile();
        if (currentTile != null) currentTile.canSpawn = false;
    }

    void Update() => HandleMouseInput();

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) OnPointerDown(GetMouseWorldPosition());
        else if (Input.GetMouseButton(0) && isDragging) OnPointerDrag(GetMouseWorldPosition());
        else if (Input.GetMouseButtonUp(0)) OnPointerUp();
    }

    void OnPointerDown(Vector3 pointerPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (currentTile != null) currentTile.canSpawn = true;
                offset = transform.position - pointerPosition;
                isDragging = true;
            }
        }
    }

    void OnPointerDrag(Vector3 pointerPosition) => transform.position = pointerPosition + offset;

    void OnPointerUp()
    {
        if (isDragging)
        {
            isDragging = false;
            SnapToNearestTile();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float distance)) return ray.GetPoint(distance);
        return Vector3.zero;
    }

    Tile FindNearestTile()
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        Tile nearestTile = null;
        float minDistance = Mathf.Infinity;

        foreach (Tile tile in allTiles)
        {
            float distance = Vector3.Distance(transform.position, tile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }
        return nearestTile;
    }

    void SnapToNearestTile()
    {
        Tile nearestTile = FindNearestTile();
        if (nearestTile != null)
        {
            UnitDragHandler otherUnit = nearestTile.GetUnit();
            if (otherUnit != null && 
                otherUnit.unitType == this.unitType &&
                otherUnit.unitLevel == this.unitLevel &&
                UnitMergeHandler.Instance.TryMergeUnits(this, otherUnit))
            {
                return;
            }

            if (nearestTile.canSpawn)
            {
                transform.position = new Vector3(nearestTile.transform.position.x, transform.position.y, nearestTile.transform.position.z);
            
                if (currentTile != null) currentTile.SetUnit(null);
                nearestTile.SetUnit(this);
                currentTile = nearestTile;
            }
            else
            {
                transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, currentTile.transform.position.z);
            }
        }
    }
}