using System.Threading;
using UnityEngine;

public class Frog2Script : MonoBehaviour
{
    private enum TongueType {goofyTongue, moveShootTongue }

    [SerializeField]
    private GameObject Direction;
    [SerializeField]
    private GameObject Tongue;
    [SerializeField]
    private GameObject TongueSticked;
    [SerializeField]
    private TongueType tongueType;

    private float rightStickX;
    private float rightStickY;
    private Vector2 inputDirection;
    private bool isJumping;
    private Vector2 tongueDirection;
    private bool tongueSticked; 
    private bool isPointing = false;
    private bool canMoveStick = true;
    private float timer;
    [SerializeField]
    private float tongueOutTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canMoveStick){
            timer = 0;
            float rightStickX = Input.GetAxisRaw("RightStickHorizontal"); // Right joystick X-axis
            float rightStickY = Input.GetAxisRaw("RightStickVertical");   // Right joystick Y-axis
            inputDirection = new Vector2(rightStickX, rightStickY);
        }else{
            timer += Time.deltaTime;
            TongueTimer();
        }


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
            float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg; // Convert to degrees
            Tongue.transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation to Z-axis
        }else{
            Tongue.SetActive(false);
        }
    }

    private void MoveTongue(){
        float angle;
        if (inputDirection.magnitude > 1f && !tongueSticked) // Only rotate if there's input
        {
            Tongue.SetActive(false);
            Direction.SetActive(true);
            isPointing = true;
            tongueDirection = inputDirection.normalized;
            angle = Mathf.Atan2(tongueDirection.y, tongueDirection.x) * Mathf.Rad2Deg; 
            Direction.transform.rotation = Quaternion.Euler(0, 0, angle);
        }else if (isPointing && !tongueSticked){
            canMoveStick = false;
            isPointing = false;
            Direction.SetActive(false);
            angle = Mathf.Atan2(tongueDirection.y, tongueDirection.x) * Mathf.Rad2Deg; 
            StickTongue(angle);
        }
    }

    private void StickTongue(float tongueAngle){
        Tongue.SetActive(true);
        Tongue.transform.rotation = Quaternion.Euler(0, 0, tongueAngle);

    }
    private void TongueTimer(){
        if (timer > tongueOutTime){
            canMoveStick = true;
            Tongue.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 7){
            tongueSticked = true;
        }
    }

    public bool StickedState(){return tongueSticked;}
}
