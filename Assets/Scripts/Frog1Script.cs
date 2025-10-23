using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class Frog1Script : MonoBehaviour
{
    [SerializeField] private float maxJumpForce = 15f;
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float forceStep = 5f;
    [SerializeField] private BoxCollider2D ground;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject frog2;
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private GameObject follow;
    [SerializeField] private GameObject showDirection;
    [SerializeField] private AudioSource frogSound;
    [SerializeField] private bool lockedDirection;
    [SerializeField] private float flySearchRange = 15f;
    [SerializeField] private LayerMask flyLayer; // <-- new serializefield for the fly layer

    private float chargeForce;
    private Rigidbody2D rb;
    private Vector2 jumpDirection;
    private Vector2 joystickInput;
    private bool isCharging;
    private bool increasing = true;
    private bool isGrounded = true;
    private bool jumping = false;
    private Transform targetFly;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chargeForce = minJumpForce;
    }

    void FixedUpdate()
    {
        IsGrounded();
        if (isGrounded) Charge();
        else
        {
            if (rb.linearVelocityY < -3) animator.SetTrigger("GoingDown");
        }
        FlipObject();
        ShowDirection();
    }

    public void ChangePositionX(float posX)
    {
        Vector3 newvector = frog2.transform.localPosition;
        newvector.x = posX / 100;
        frog2.transform.localPosition = newvector;
    }

    public void ChangePositionY(float posY)
    {
        Vector3 newvector = frog2.transform.localPosition;
        newvector.y = posY / 100;
        frog2.transform.localPosition = newvector;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        joystickInput = context.ReadValue<Vector2>();
    }
    public void OnAimArduino(Vector2 v1)
    {
        joystickInput = v1;
    }

    private void Charge()
    {
        if (joystickInput.magnitude > 0.5f || Input.GetMouseButton(0))
        {
            if (!isCharging) animator.SetTrigger("Jump");
            isCharging = true;
            jumpDirection = GetJumpDirection();
            if (increasing)
            {
                chargeForce += forceStep;
                if (chargeForce >= maxJumpForce) increasing = false;
            }
            else
            {
                chargeForce -= forceStep;
                if (chargeForce <= minJumpForce) increasing = true;
            }
        }
        else if (isCharging || Input.GetMouseButtonUp(0))
        {
            Jump();
            frogSound.Play();
            isCharging = false;
            chargeForce = minJumpForce;
            jumping = true;
            StartCoroutine(DelayedGroundJump());
        }
        else animator.SetTrigger("Idle");
    }

    private IEnumerator DelayedGroundJump()
    {
        yield return new WaitForSeconds(0.1f);
        if (rb.linearVelocityY > 0.1) animator.SetTrigger("GoingUp");
    }

    private Vector2 GetJumpDirection()
    {
        if (lockedDirection)
        {
            targetFly = FindClosestFly();
            if (targetFly != null) return -(targetFly.position - transform.position).normalized;
            else return Vector2.zero;
        }

        if (joystickInput.magnitude > 0.1f) return -joystickInput.normalized;
        else if (Input.GetMouseButton(0)) return GetMouseDirection();
        else return Vector2.zero;
    }

    private Transform FindClosestFly()
    {
        Collider2D[] flies = Physics2D.OverlapCircleAll(transform.position, flySearchRange, flyLayer);
        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector2 pos = transform.position;
        foreach (var f in flies)
        {
            float dist = Vector2.Distance(pos, f.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = f.transform;
            }
        }
        return closest;
    }

    private Vector2 GetMouseDirection()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        float zDistanceFromCamera = transform.position.z - Camera.main.transform.position.z;
        mouseScreenPosition.z = zDistanceFromCamera;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        return (mouseWorldPosition - transform.position).normalized;
    }

    private void ShowDirection()
    {
        Vector2 dir;

        if (lockedDirection && targetFly != null)
            dir = (targetFly.position - transform.position).normalized;
        else
            dir = jumpDirection;

        if (isCharging && dir != Vector2.zero && isGrounded)
        {
            showDirection.SetActive(true);
            float angle = lockedDirection ? Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg 
                                        : Mathf.Atan2(-dir.y, -dir.x) * Mathf.Rad2Deg;
            showDirection.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            showDirection.SetActive(false);
        }
    }



    private void Jump()
    {
        Vector2 dir = lockedDirection && targetFly != null ? (targetFly.position - transform.position).normalized : jumpDirection;

        if (dir != Vector2.zero)
            rb.linearVelocity = lockedDirection ? dir * chargeForce : -dir * chargeForce;
    }
    
    private void IsGrounded()
    {
        if (ground.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            if (!isGrounded)
            {
                rb.linearVelocity = Vector2.zero;
                animator.ResetTrigger("Jump");
                animator.SetTrigger("HitGround");
            }
            animator.ResetTrigger("GoingUp");
            animator.ResetTrigger("GoingDown");
            isGrounded = true;
            jumping = false;
        }
        else isGrounded = false;
    }

    private void FlipObject()
    {
        if (jumpDirection.x > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        else transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnDrawGizmosSelected()
    {
        if (lockedDirection)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, flySearchRange);
        }
    }

    public float MaxValue() { return maxJumpForce; }
    public float MinValue() { return minJumpForce; }
    public float NowValue() { return chargeForce; }
}
