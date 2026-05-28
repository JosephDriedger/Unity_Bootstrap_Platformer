using UnityEngine;

/// <summary>
/// Attach to the Saw_Blade ROOT. Cycles the blade child (Props_9a) between
/// active (visible + hazardous) and inactive (hidden + safe) on a configurable timer.
/// The root/track stays visible at all times so the player can read the obstacle.
/// </summary>
public class SpikeToggle : MonoBehaviour
{
    [Tooltip("Seconds the blade is active and dangerous.")]
    public float onDuration = 1.5f;

    [Tooltip("Seconds the blade is hidden and safe to pass through.")]
    public float offDuration = 1.0f;

    private GameObject _blade;
    private float _timer;
    private bool _isOn = true;

    void Start()
    {
        // Props_9a is always the first (and only) child
        if (transform.childCount > 0)
            _blade = transform.GetChild(0).gameObject;

        _timer = onDuration;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0f) return;

        _isOn = !_isOn;
        _timer = _isOn ? onDuration : offDuration;

        if (_blade != null)
            _blade.SetActive(_isOn);
    }
}
