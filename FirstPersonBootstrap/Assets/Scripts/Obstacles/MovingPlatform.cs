using UnityEngine;

// Add to a floating platform — oscillates between its start position and start + moveDistance.
// Carries the player by applying the same delta to their Rigidbody each physics step.
public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDistance = new Vector3(5f, 0f, 0f);
    public float speed = 1f;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _t = 0f;
    private bool _movingForward = true;
    private Rigidbody _riderRb;

    void Start()
    {
        _startPosition = transform.position;
        _endPosition   = _startPosition + moveDistance;
    }

    void FixedUpdate()
    {
        _t += (_movingForward ? 1f : -1f) * speed * Time.fixedDeltaTime;
        _t = Mathf.Clamp01(_t);

        Vector3 newPos = Vector3.Lerp(_startPosition, _endPosition, Mathf.SmoothStep(0f, 1f, _t));
        Vector3 delta  = newPos - transform.position;
        transform.position = newPos;

        // Carry the rider: apply the exact same positional delta to their Rigidbody
        if (_riderRb != null)
            _riderRb.MovePosition(_riderRb.position + delta);

        if (_t >= 1f) _movingForward = false;
        else if (_t <= 0f) _movingForward = true;
    }

    // OnCollisionStay fires every physics frame the player is in contact — more reliable than
    // OnCollisionEnter for platforms that move via transform (no Rigidbody).
    // Position check avoids the contact-normal ambiguity when the player walks onto the edge.
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Only carry the player when they are on TOP of the platform.
        if (collision.transform.position.y >= transform.position.y)
            _riderRb = collision.gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            _riderRb = null;
    }
}
