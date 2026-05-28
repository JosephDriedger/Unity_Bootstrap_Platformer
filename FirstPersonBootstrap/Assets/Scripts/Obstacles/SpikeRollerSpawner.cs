using System.Collections.Generic;
using UnityEngine;

public class SpikeRollerSpawner : MonoBehaviour
{
    public GameObject spikeRollerPrefab;
    public float spawnInterval = 3f;

    [Tooltip("Force applied in the spawner's local space. Default fires forward (+Z). " +
             "The spawner's blue arrow should point toward where rollers should travel.")]
    public Vector3 launchForce = new Vector3(0f, 0f, 5f);

    public int maxRollers = 5;
    public float rollerLifetime = 12f;

    private float timer;
    private readonly List<GameObject> active = new();

    void Update()
    {
        active.RemoveAll(r => r == null);

        timer += Time.deltaTime;
        if (timer >= spawnInterval && active.Count < maxRollers)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        if (spikeRollerPrefab == null) return;

        // The Spike_Roller capsule is oriented along its local Z axis (m_Direction: 2).
        // For the roller to roll (not slide) it must lie with its cylinder axis
        // perpendicular to the launch direction.  Rotating 90° around local Y achieves
        // that: the capsule's Z becomes the spawner's X, so a forward (+Z) launch
        // rolls the cylinder cleanly like a log.
        Quaternion spawnRot = transform.rotation * Quaternion.Euler(0f, 90f, 0f);

        GameObject roller = Instantiate(spikeRollerPrefab, transform.position, spawnRot);
        Rigidbody rb = roller.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddForce(transform.TransformDirection(launchForce), ForceMode.Impulse);

        active.Add(roller);
        Destroy(roller, rollerLifetime);
    }
}
