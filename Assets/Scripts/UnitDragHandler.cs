using UnityEngine;

public class UnitDragHandler : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Tile currentTile = null;

    void Start()
    {
        currentTile = FindNearestTile();
        if (currentTile != null) currentTile.canSpawn = false;
    }

    void Update()
    {
        HandleMouseInput();
    }

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

    void OnPointerDrag(Vector3 pointerPosition)
    {
        transform.position = pointerPosition + offset;
    }

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
        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
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
        if (nearestTile != null && nearestTile.canSpawn)
        {
            transform.position = new Vector3(nearestTile.transform.position.x, transform.position.y, nearestTile.transform.position.z);
            if (currentTile != null) currentTile.canSpawn = true;
            nearestTile.canSpawn = false;
            currentTile = nearestTile;

            GetComponent<UnitMergeHandler>().CheckForMerge();
        }
        else
        {
            transform.position = new Vector3(currentTile.transform.position.x, transform.position.y, currentTile.transform.position.z);
        }
    }
}
