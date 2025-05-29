using UnityEngine;
using UnityEngine.Audio;

public class Frog2ScriptNew : MonoBehaviour
{
    [SerializeField]
    private GameObject Direction;
    [SerializeField]
    private Transform tongueTransform;
    [SerializeField]
    private AudioSource tongueOutSound;

    private float angle;
    private Vector2 inputDirection;
    private Vector2 tongueDirection;
    private bool isJumping;
    private bool canMoveStick = true;
    private Animator _anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMoveStick){
            float rightStickX = Input.GetAxisRaw("RightStickHorizontal"); // Right joystick X-axis
            float rightStickY = Input.GetAxisRaw("RightStickVertical");   // Right joystick Y-axis
            inputDirection = new Vector2(rightStickX, rightStickY);
        }
        MoveTongue();
    }


    private void MoveTongue() {
        if (inputDirection.magnitude > 1f || Input.GetMouseButton(1)) // Only rotate if there's input
        {
            Direction.SetActive(true);
            tongueDirection = GetTongueDirection();
            angle = Mathf.Atan2(tongueDirection.y, tongueDirection.x) * Mathf.Rad2Deg;
            Direction.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            tongueOutSound.Play();
            tongueTransform.rotation = Quaternion.Euler(0, 0, angle);
            _anim.SetTrigger("tongueOut");
        }
    }

    private Vector2 GetTongueDirection()
    {
        if (inputDirection.magnitude > 1f) return inputDirection.normalized;

        else if (Input.GetMouseButton(1)) return GetMouseDirection();

        else return Vector2.zero;
    }

    private Vector2 GetMouseDirection()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        return direction;
    }

}
