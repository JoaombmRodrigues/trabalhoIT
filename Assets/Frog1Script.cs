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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        IsGrounded();
        if(isGrounded){
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            inputDirection = new Vector2(horizontal, vertical);
            Charge();
        }

        
    }
    private void Charge(){

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
        }
    }

    private void Jump()
    {
        float jumpForce = chargeForce;
        rb.linearVelocity = jumpDirection * jumpForce;
    }
    private void IsGrounded(){
        if (ground.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            if (isGrounded == false){
                rb.linearVelocity = new Vector2(0, 0);
            }
            isGrounded = true;
        }else{
            isGrounded = false;
        }
    }
    public float MaxValue(){
        return maxJumpForce;
    }
    public float MinValue(){
        return minJumpForce;
    }
    public float NowValue(){
        return chargeForce;
    }
}
