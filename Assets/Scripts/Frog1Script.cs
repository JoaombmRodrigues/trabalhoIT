using System.Collections;
using UnityEngine;

public class Frog1Script : MonoBehaviour
{
    [SerializeField]
    private float maxJumpForce = 15f;    // Max force applied when fully charged
    [SerializeField]
    private float minJumpForce = 5f;     // Minimum force applied
    [SerializeField]
    private float forceStep = 5f;
    [SerializeField]
    private BoxCollider2D ground;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private GameObject frog2;
    private float chargeForce;
    private Rigidbody2D rb;
    private Vector2 inputDirection;
    private Vector2 jumpDirection;
    private bool isCharging;
    private bool increasing = true;
    private bool isGrounded = true;
    private bool jumping = false;
    [SerializeField]
    private Rigidbody2D rig;
    [SerializeField]
    private GameObject follow;
    [SerializeField]
    private GameObject showDirection;
    [SerializeField]
    private AudioSource frogSound;

    void Start()
    {
        //rig.bodyType = RigidbodyType2D.Kinematic;
        rb = GetComponent<Rigidbody2D>();
        chargeForce = minJumpForce;
    }

    void Update()
    {
        Frog2Script isSticked = GetComponent<Frog2Script>();
        IsGrounded();
        //Vector3 ola = follow.transform.position;
        //ola.y -= 1;
        //this.gameObject.transform.position = ola;
        if (isGrounded)
        {

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            inputDirection = new Vector2(horizontal, vertical);
            Charge();
        }
        else
        {
            IsSticked();
            if (rb.linearVelocityY < -3)
            {
                animator.SetTrigger("GoingDown");
            }

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



    private void Charge()
    {
        if (inputDirection.magnitude > 1f || Input.GetMouseButton(0)) // Joystick moved or left click
        {
            if (!isCharging)
                animator.SetTrigger("Jump");

            isCharging = true;
            jumpDirection = GetJumpDirection();
            if (increasing)
            {
                chargeForce += forceStep;
                if (chargeForce >= maxJumpForce)
                {
                    increasing = false; // Start decreasing
                }
            }
            else
            {
                chargeForce -= forceStep;
                if (chargeForce <= minJumpForce)
                {
                    increasing = true; // Start increasing again
                }
            }
        }
        else if (isCharging || Input.GetMouseButtonUp(0)) // Joystick released
        {
            Jump();
            frogSound.Play();
            isCharging = false;
            chargeForce = minJumpForce;
            jumping = true;
            StartCoroutine(DelayedGroundJump());
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
    private IEnumerator DelayedGroundJump()
    {
        yield return new WaitForSeconds(0.1f);
        if (rb.linearVelocityY > 0.1)
            animator.SetTrigger("GoingUp");
    }

    private Vector2 GetJumpDirection()
    {
        if (inputDirection.magnitude > 1f) return inputDirection.normalized;

        else if (Input.GetMouseButton(0)) return GetMouseDirection();

        else return Vector2.zero;
    }

    private Vector2 GetMouseDirection()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        return direction;
    }
    
    private void ShowDirection()
    {
        if ((-jumpDirection.x > 0f && -jumpDirection.y > 0f) ||
            jumpDirection.x > 0f && -jumpDirection.y > 0f &&
            isGrounded && isCharging)
        {
            showDirection.SetActive(true);
            float angle = Mathf.Atan2(-jumpDirection.y, -jumpDirection.x) * Mathf.Rad2Deg; // Convert to degrees
            showDirection.transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation to Z-axis
        }
        else
        {
            showDirection.SetActive(false);
        }


    }

    private void Jump()
    {
        if ((-jumpDirection.x > 0f && -jumpDirection.y > 0f) ||
            jumpDirection.x > 0f && -jumpDirection.y > 0f)
        {
            rb.linearVelocity = -jumpDirection * chargeForce;
        }

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
        else
        {
            isGrounded = false;
        }
    }

    private void IsSticked()
    {

    }

    private void FlipObject()
    {
        if (jumpDirection.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public float MaxValue() { return maxJumpForce; }
    public float MinValue() { return minJumpForce; }
    public float NowValue() { return chargeForce; }
}