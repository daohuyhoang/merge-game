using UnityEngine;

public class HealthBarOrientation : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}