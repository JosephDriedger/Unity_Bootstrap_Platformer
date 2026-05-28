using UnityEngine;

/// <summary>
/// Slides this object back and forth along a local axis (e.g. a saw blade on a rail).
/// Attach to the blade child – NOT to the track root.
/// The RotatingObstacle script handles the spin; this handles the patrol.
/// </summary>
public class TrackPatrol : MonoBehaviour
{
    [Tooltip("Patrol direction in local parent space. Vector3.right slides along the track's X axis.")]
    public Vector3 axis = Vector3.right;

    [Tooltip("How far (units) to travel each side of the starting position.")]
    public float distance = 1.5f;

    [Tooltip("Full oscillations per second.")]
    public float speed = 0.6f;

    private Vector3 _startLocalPos;

    void Start()
    {
        _startLocalPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed * Mathf.PI * 2f) * distance;
        transform.localPosition = _startLocalPos + axis.normalized * offset;
    }
}
