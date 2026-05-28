using UnityEngine;

// Add to the swinging hammer — rocks it back and forth like a pendulum around its local axis.
// Pair with HazardObstacle on the same GameObject (or a child collider) to kill the player on contact.
public class SwingingObstacle : MonoBehaviour
{
    public float swingAngle = 45f;
    public float swingSpeed = 1.5f;
    public Vector3 swingAxis = Vector3.forward;

    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localRotation = startRotation * Quaternion.AngleAxis(angle, swingAxis.normalized);
    }
}
