using UnityEngine;

public class Frog2Script : MonoBehaviour
{
    private enum TongueType {goofyTongue, moveShootTongue }

    [SerializeField]
    private GameObject Direction;
    [SerializeField]
    private GameObject Tongue;
    [SerializeField]
    private TongueType tongueType;

    private float rightStickX;
    private float rightStickY;
    private Vector2 inputDirection;
    private bool isJumping;
    private Vector2 tongueDirection;
    private bool tongueSticked; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rightStickX = Input.GetAxisRaw("RightStickHorizontal"); // Right joystick X-axis
        float rightStickY = Input.GetAxisRaw("RightStickVertical");   // Right joystick Y-axis
        Vector2 inputDirection = new Vector2(rightStickX, rightStickY);

        if (tongueType == TongueType.goofyTongue){
            GoofyTongue();
        }else{
            MoveTongue();
        }
    }

    private void GoofyTongue(){
        if (inputDirection.magnitude > 1f) // Only rotate if there's input
        {
            Tongue.SetActive(true);
            float angle = Mathf.Atan2(rightStickY, rightStickX) * Mathf.Rad2Deg; // Convert to degrees
            Tongue.transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation to Z-axis
        }else{
            Tongue.SetActive(false);
        }
    }

    private void MoveTongue(){
        bool isPointing = false;
        float angle;
        if (inputDirection.magnitude > 1f && !tongueSticked) // Only rotate if there's input
        {
            Direction.SetActive(true);
            isPointing = true;
            tongueDirection = inputDirection.normalized;
            angle = Mathf.Atan2(tongueDirection.x, tongueDirection.y) * Mathf.Rad2Deg; 
            Direction.transform.rotation = Quaternion.Euler(0, 0, angle);
        }else if (isPointing && !tongueSticked){
            isPointing = false;
            Direction.SetActive(false);
            angle = Mathf.Atan2(tongueDirection.x, tongueDirection.y) * Mathf.Rad2Deg; 
            StickTongue(angle);

        }
    }

    private void StickTongue(float tongueAngle){
        Tongue.SetActive(true);
        Tongue.transform.rotation = Quaternion.Euler(0, 0, tongueAngle);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 7){
            tongueSticked = true;
        }
    }

    public bool StickedState(){return tongueSticked;}
}
