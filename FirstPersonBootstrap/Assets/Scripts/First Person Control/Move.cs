using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public Key runKey = Key.LeftShift;

    private Rigidbody rb;
    private float _stunUntil = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Suppresses movement input for <paramref name="duration"/> seconds so that
    /// knockback impulses (e.g. from HammerKnock) aren't immediately overridden.
    /// </summary>
    public void Stun(float duration)
    {
        _stunUntil = Time.time + duration;
    }

    void FixedUpdate()
    {
        // During a stun (knockback), let physics carry the player freely.
        if (Time.time < _stunUntil) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        float speed = keyboard[runKey].isPressed ? runSpeed : walkSpeed;

        float inputX = (keyboard.dKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed ? 1f : 0f);
        float inputZ = (keyboard.wKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed ? 1f : 0f);

        Vector3 move = rb.transform.TransformDirection(new Vector3(inputX, 0, inputZ)) * speed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
}
