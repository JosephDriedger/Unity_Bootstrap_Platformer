using UnityEngine;

// Add to any obstacle that should reset the player on contact (spikes, saw blades, etc.)
public class HazardObstacle : MonoBehaviour
{
    private PlayerReset playerReset;

    void Start()
    {
        playerReset = FindFirstObjectByType<PlayerReset>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerReset?.ResetPlayer(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerReset?.ResetPlayer(other.gameObject);
    }
}
