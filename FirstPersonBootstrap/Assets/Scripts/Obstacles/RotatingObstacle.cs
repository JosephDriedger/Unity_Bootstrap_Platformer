using UnityEngine;

// Add to saw blades, spin spikes, spin platforms — rotates the object continuously.
public class RotatingObstacle : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 90f;

    void Update()
    {
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
