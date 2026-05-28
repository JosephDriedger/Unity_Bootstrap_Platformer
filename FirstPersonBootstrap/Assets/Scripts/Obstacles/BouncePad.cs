using System.Collections;
using UnityEngine;

// Add to a bounce pad — launches the player straight up when they walk or land on it.
// The spring child (Props_4a) squishes on contact then springs back.
public class BouncePad : MonoBehaviour
{
    [Tooltip("Upward velocity applied to the player on contact.")]
    public float bounceForce = 15f;

    [Tooltip("How much the spring squishes (fraction of original Y scale).")]
    public float squishScale = 0.4f;

    [Tooltip("Total duration of the squish-and-spring animation in seconds.")]
    public float animDuration = 0.35f;

    private Transform _spring;      // Props_4a child — the coil visual
    private Vector3   _springRestScale;
    private bool      _animating;

    void Start()
    {
        // Props_4a is the first (and only) child
        if (transform.childCount > 0)
        {
            _spring = transform.GetChild(0);
            _springRestScale = _spring.localScale;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Accept contact from any direction the player arrives — just require
        // the player to be at or above the pad's centre so side-bumps are ignored.
        if (collision.transform.position.y < transform.position.y) return;

        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Launch upward
        Vector3 v = rb.linearVelocity;
        v.y = bounceForce;
        rb.linearVelocity = v;

        // Play spring animation (only one at a time)
        if (!_animating && _spring != null)
            StartCoroutine(SpringAnim());
    }

    IEnumerator SpringAnim()
    {
        _animating = true;

        float squish  = animDuration * 0.3f;   // time to compress
        float stretch = animDuration * 0.4f;   // time to overshoot upward
        float settle  = animDuration * 0.3f;   // time to return to rest

        Vector3 squishTarget  = new Vector3(_springRestScale.x,
                                            _springRestScale.y * squishScale,
                                            _springRestScale.z);
        Vector3 stretchTarget = new Vector3(_springRestScale.x,
                                            _springRestScale.y * (2f - squishScale),
                                            _springRestScale.z);

        // Compress
        yield return LerpScale(_spring, _springRestScale, squishTarget,  squish);
        // Overshoot spring-back
        yield return LerpScale(_spring, squishTarget,     stretchTarget, stretch);
        // Settle
        yield return LerpScale(_spring, stretchTarget,    _springRestScale, settle);

        _animating = false;
    }

    static IEnumerator LerpScale(Transform t, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            t.localScale = Vector3.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        t.localScale = to;
    }
}
