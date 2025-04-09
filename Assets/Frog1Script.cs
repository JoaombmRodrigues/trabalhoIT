using Unity.VisualScripting;
using UnityEngine;

public class Frog1Script : MonoBehaviour
{
    [SerializeField]
    private float maxJumpForce = 15f;    // Max force applied when fully charged
    [SerializeField]
    private float minJumpForce = 5f;     // Minimum force applied
    [SerializeField]
    private BoxCollider2D ground;
    private float chargeForce = 5;
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

    void Start()
    {
        //rig.bodyType = RigidbodyType2D.Kinematic;
        rb = GetComponent<Rigidbody2D>();
        
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
            
        }
        ShowDirection();
        
    }

    private void Charge()
    {
        if (inputDirection.magnitude > 1f) // Joystick moved
        {
            isCharging = true;
            jumpDirection = inputDirection.normalized;
            if (increasing)
            {
                chargeForce += 0.01f;
                if (chargeForce >= maxJumpForce)
                {
                    increasing = false; // Start decreasing
                }
            }
            else
            {
                chargeForce -= 0.01f;
                if (chargeForce <= minJumpForce)
                {
                    increasing = true; // Start increasing again
                }
            }
        }
        else if (isCharging) // Joystick released
        { 
            Jump();
            isCharging = false;
            chargeForce = 5f;
            jumping = true;
        }
    }

    private void ShowDirection(){
        if (((-jumpDirection.x > 0 & -jumpDirection.y > 0) && isGrounded && isCharging) ||
        ((jumpDirection.x > 0 & -jumpDirection.y > 0) && isGrounded && isCharging))
        {
            showDirection.SetActive(true);
            float angle = Mathf.Atan2(-jumpDirection.y, -jumpDirection.x) * Mathf.Rad2Deg; // Convert to degrees
            showDirection.transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation to Z-axis
        }else{
            showDirection.SetActive(false);
        }

        
    }

    private void Jump()
    {
        if ((-jumpDirection.x > 0 & -jumpDirection.y > 0) ||
            (jumpDirection.x > 0 & -jumpDirection.y > 0))
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
            }
            isGrounded = true;
            jumping = false;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void IsSticked(){
        
    }



    public float MaxValue() { return maxJumpForce; }
    public float MinValue() { return minJumpForce; }
    public float NowValue() { return chargeForce; }
}