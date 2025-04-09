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
    private LineRenderer lineRenderer;
    [SerializeField]
    private Rigidbody2D rig;
    [SerializeField]
    private GameObject follow;

    void Start()
    {
        rig.bodyType = RigidbodyType2D.Kinematic;
        rb = GetComponent<Rigidbody2D>();
        
        //LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.enabled = false; // Hide at start
        lineRenderer.sortingLayerName = "Background"; // Set to a lower sorting layer
        lineRenderer.sortingOrder = -1; // Ensure it's behind other objects
    }

    void Update()
    {
        
        Frog2Script isSticked = GetComponent<Frog2Script>();
        IsGrounded();
        Vector3 ola = follow.transform.position;
        ola.y -= 1;
        this.gameObject.transform.position = ola;
        //if (isGrounded)
        //{
        //    float horizontal = Input.GetAxis("Horizontal");
        //    float vertical = Input.GetAxis("Vertical");
        //    inputDirection = new Vector2(horizontal, vertical);
        //    Charge();
        //    UpdateJumpIndicator();
        //}
        ///else
        //{
            //IsSticked();
        //}//
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
            lineRenderer.enabled = false; // Hide line after jumping
            jumping = false;
        }
        else
        {
            isGrounded = false;
        }

    }

    private void UpdateJumpIndicator()
    {
        if (inputDirection.magnitude > 0.2f & (-jumpDirection.x > 0 & -jumpDirection.y > 0) ||
            (jumpDirection.x > 0 & -jumpDirection.y > 0 & jumping == false)) // Ensure there's a valid input direction
        {
            Vector2 startPos = rb.position; // More accurate position from Rigidbody
            Vector2 velocity = -jumpDirection * chargeForce; // Initial jump velocity
            float timeStep = 0.05f; // Small time step for smoother curve
            int maxSteps = 60; // More points for a smoother curve
            float gravity = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale; // Ensure positive gravity value

            lineRenderer.positionCount = maxSteps;

            for (int i = 0; i < maxSteps; i++)
            {
                float t = i * timeStep;
                Vector2 point = startPos + velocity * t + 0.5f * new Vector2(0, -gravity * 0.9f) * (t * t);

                lineRenderer.SetPosition(i, point);

                // Stop drawing if the point goes below starting Y too soon
                if (i > 5 && point.y < startPos.y - 1)
                {
                    lineRenderer.positionCount = i + 1;
                    break;
                }
            }

            lineRenderer.enabled = true;
        }

    }

    private void IsSticked(){
        
    }



    public float MaxValue() { return maxJumpForce; }
    public float MinValue() { return minJumpForce; }
    public float NowValue() { return chargeForce; }
}