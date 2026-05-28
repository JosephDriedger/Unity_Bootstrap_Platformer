using UnityEngine;

/// <summary>
/// Attach to Swinging_Hammer in place of HazardObstacle.
/// On contact the hammer knocks the player in the direction it is currently swinging,
/// calculated from the pendulum's instantaneous angular velocity at the contact point.
/// </summary>
[RequireComponent(typeof(SwingingObstacle))]
public class HammerKnock : MonoBehaviour
{
    [Tooltip("Horizontal impulse applied to the player.")]
    public float knockForce = 15f;

    [Tooltip("Upward impulse added alongside the horizontal knock.")]
    public float knockUpForce = 8f;

    [Tooltip("Seconds the player's movement input is suppressed after a hit " +
             "(prevents Move.cs from immediately fighting the impulse).")]
    public float stunDuration = 0.6f;

    private SwingingObstacle _swing;
    private float _lastKnockTime = -999f;

    void Start()
    {
        _swing = GetComponent<SwingingObstacle>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // One knock per swing pass — ignore rapid re-contacts.
        if (Time.time - _lastKnockTime < stunDuration) return;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null) return;

        _lastKnockTime = Time.time;

        // ── Angular velocity of the pendulum at this exact moment ─────────
        // SwingingObstacle drives:  angle = sin(t * swingSpeed) * swingAngle
        // d(angle)/dt             = cos(t * swingSpeed) * swingAngle * swingSpeed  (°/s)
        float angVelDeg = Mathf.Cos(Time.time * _swing.swingSpeed)
                          * _swing.swingAngle
                          * _swing.swingSpeed;
        float angVelRad = angVelDeg * Mathf.Deg2Rad;

        // ── Velocity at the contact point:  v = ω × r ────────────────────
        // ω = angular velocity vector in world space
        // r = vector from pivot (transform.position) to contact point
        Vector3 worldAxis = transform.TransformDirection(_swing.swingAxis.normalized);
        Vector3 omega     = worldAxis * angVelRad;
        Vector3 r         = collision.contacts[0].point - transform.position;
        Vector3 hitVel    = Vector3.Cross(omega, r);

        // ── Build the impulse: horizontal knock + upward kick ─────────────
        Vector3 knockDir = new Vector3(hitVel.x, 0f, hitVel.z);

        if (knockDir.sqrMagnitude < 0.01f)
        {
            // Fallback (contact near pivot, or hammer at apex):
            // push perpendicular to the swing axis based on the angular velocity sign.
            Vector3 perp = Vector3.Cross(worldAxis, Vector3.up).normalized;
            knockDir = perp * Mathf.Sign(angVelDeg >= 0f ? 1f : -1f);
        }

        Vector3 impulse = knockDir.normalized * knockForce + Vector3.up * knockUpForce;

        // Zero existing velocity so the impulse magnitude is consistent
        // regardless of how fast the player was moving.
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(impulse, ForceMode.Impulse);

        // ── Stun player movement so Move.cs doesn't override the impulse ──
        Move move = collision.gameObject.GetComponent<Move>();
        move?.Stun(stunDuration);
    }
}
