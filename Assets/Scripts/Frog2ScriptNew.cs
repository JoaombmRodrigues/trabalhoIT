using UnityEngine;
using System.Collections.Generic;

public class Frog2ScriptNew : MonoBehaviour
{
    [SerializeField] private GameObject Direction;
    [SerializeField] private Transform tongueTransform;
    [SerializeField] private AudioSource tongueOutSound;
    [SerializeField] private float comboGraceTime = 0.5f;

    private Animator _anim;
    private Vector2 inputDirection;

    private HashSet<KeyCode> heldKeys = new HashSet<KeyCode>();
    private bool comboActive = false;
    private float comboStartTime = 0f;
    private Vector2 comboDirection;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        TrackWASDKeys();
        inputDirection = GetCurrentDirection();

        UpdateAim();

        CheckComboFire();
    }

    private void TrackWASDKeys()
    {
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                heldKeys.Add(key);
            }

            if (Input.GetKeyUp(key))
            {
                // On first key release in a combo, start the timer
                if (!comboActive && heldKeys.Count > 0)
                {
                    comboDirection = GetCurrentDirection();
                    comboStartTime = Time.time;
                    comboActive = true;
                }
                heldKeys.Remove(key);
            }
        }
    }

    private Vector2 GetCurrentDirection()
    {
        float x = 0f, y = 0f;

        if (heldKeys.Contains(KeyCode.W)) y += 1f;
        if (heldKeys.Contains(KeyCode.S)) y -= 1f;
        if (heldKeys.Contains(KeyCode.D)) x += 1f;
        if (heldKeys.Contains(KeyCode.A)) x -= 1f;

        return new Vector2(x, y).normalized;
    }

    private void UpdateAim()
    {
        if (inputDirection.magnitude > 0)
        {
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            Direction.SetActive(true);
            Direction.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void CheckComboFire()
    {
        if (comboActive && Time.time - comboStartTime >= comboGraceTime)
        {
            if (comboDirection.magnitude > 0)
            {
                FireTongue(comboDirection);
            }
            comboActive = false;
        }
    }

    private void FireTongue(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tongueOutSound.Play();
        tongueTransform.rotation = Quaternion.Euler(0, 0, angle);
        _anim.SetTrigger("tongueOut");
    }
}
