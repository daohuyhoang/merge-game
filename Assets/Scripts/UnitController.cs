using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Tile currentTile = null;
    
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            
        }
        else
        {
            HandleMouseInput();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPointerDown(GetMouseWorldPosition());
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            OnPointerDrag(GetMouseWorldPosition());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnPointerUp();
        }
    }

    void OnPointerDown(Vector3 pointerPosition)
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
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    void SnapToNearestTile()
    {
        Vector3 currentPosition = transform.position;
        Tile[] allTiles = FindObjectsOfType<Tile>();
    
        Tile nearestTile = null;
        float minDistance = Mathf.Infinity;

        foreach (Tile tile in allTiles)
        {
            float distance = Vector3.Distance(currentPosition, tile.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }
    
        if (nearestTile != null)
        {
            transform.position = new Vector3(nearestTile.transform.position.x, currentPosition.y, nearestTile.transform.position.z);
            nearestTile.canSpawn = false;
            if (currentTile != null) currentTile.canSpawn = true;
            currentTile = nearestTile;
        }
    }

}
