using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using NUnit.Framework;

public class Frog2ScriptNew : MonoBehaviour
{
    [SerializeField] private GameObject Direction;
    [SerializeField] private Transform tongueTransform;
    [SerializeField] private AudioSource tongueOutSound;
    [SerializeField] private float comboGraceTime = 0.5f;
    [SerializeField] private float tongueCooldown = 1f;
    [SerializeField] private bool lockedDirection;
    [SerializeField] private bool lockedRange;
    [SerializeField] private float rangeToActivate = 5f;
    [SerializeField] private LayerMask flyLayer;
    [SerializeField] private float queueExpireTime = 3f;

    private float nextTongueTime = 0f;
    private Animator _anim;
    private Vector2 inputDirection;
    private Vector2 joystickInput;

    private HashSet<KeyCode> heldKeys = new HashSet<KeyCode>();
    private bool comboActive = false;
    private float comboStartTime = 0f;
    private Vector2 comboDirection;

    private bool queuedShot = false;
    private Vector2 queuedDirection;
    private float queuedStartTime;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        TrackWASDKeys();
        inputDirection = GetCurrentDirection();
        if(!lockedDirection)
            UpdateAim();
        CheckComboFire();

        if (queuedShot)
        {
            if (Time.time - queuedStartTime > queueExpireTime)
            {
                queuedShot = false;
                return;
            }

            Transform nearestFly = FindNearestFly(rangeToActivate);
            if (nearestFly != null)
            {
                Vector2 dir = queuedDirection;
                if (lockedDirection)
                    dir = (nearestFly.position - transform.position).normalized;

                FireTongue(dir);
                nextTongueTime = Time.time + tongueCooldown;
                queuedShot = false;
            }
        }
    }

    private void TrackWASDKeys()
    {
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
                heldKeys.Add(key);

            if (Input.GetKeyUp(key))
            {
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

    public void OnTongue(InputAction.CallbackContext context)
    {
        joystickInput = context.ReadValue<Vector2>();
    }
    
    public void OnAimArduino(Vector2 v2)
    {
        joystickInput = v2;
    }

    private Vector2 GetCurrentDirection()
    {
        if (joystickInput.magnitude > 0.1f) return joystickInput.normalized;

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
        if (nextTongueTime < Time.time)
        {
            if (comboActive && Time.time - comboStartTime >= comboGraceTime)
            {
                if (comboDirection.magnitude > 0)
                {
                    TryFireTongue(comboDirection);
                    nextTongueTime = Time.time + tongueCooldown;
                }
                comboActive = false;
            }
            else if (joystickInput.magnitude > 0.6f)
            {
                TryFireTongue(inputDirection);
                nextTongueTime = Time.time + tongueCooldown;
            }
        }
    }

    private void TryFireTongue(Vector2 dir)
    {
        Transform nearestFly = null;

        if (lockedRange)
            nearestFly = FindNearestFly(rangeToActivate);
        else if (lockedDirection)
            nearestFly = FindNearestFly(Mathf.Infinity);

        if (lockedRange && nearestFly == null)
        {
            queuedShot = true;
            queuedDirection = dir;
            queuedStartTime = Time.time;
            return;
        }

        if (lockedDirection && nearestFly != null)
            dir = (nearestFly.position - transform.position).normalized;

        FireTongue(dir);
        Debug.Log("dir" + dir);
    }

    private void FireTongue(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Debug.Log("angle" + angle);
        tongueOutSound.Play();
        tongueTransform.rotation = Quaternion.Euler(0, 0, angle);
        Debug.Log("rotation" + tongueTransform.rotation);
        _anim.SetTrigger("tongueOut");
    }

    private Transform FindNearestFly(float range)
    {
        Collider2D[] flies = Physics2D.OverlapCircleAll(transform.position, range, flyLayer);
        Transform nearest = null;
        float closestDist = Mathf.Infinity;

        foreach (var fly in flies)
        {
            float dist = Vector2.Distance(transform.position, fly.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                nearest = fly.transform;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        if (lockedRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rangeToActivate);
        }
    }
}
