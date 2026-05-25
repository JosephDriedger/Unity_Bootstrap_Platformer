using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    public float jumpStrength = 5;
    public Key jumpKey = Key.Space;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current[jumpKey].wasPressedThisFrame && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(rb.transform.up * jumpStrength, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }
}
