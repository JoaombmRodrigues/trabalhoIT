using UnityEngine;

public class Frog2Script : MonoBehaviour
{
    [SerializeField]
    private GameObject pivot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rightStickX = Input.GetAxisRaw("RightStickHorizontal"); // Right joystick X-axis
        float rightStickY = Input.GetAxisRaw("RightStickVertical");   // Right joystick Y-axis

        Vector2 rightStickInput = new Vector2(rightStickX, rightStickY);

        if (rightStickInput.magnitude > 1.5f) // Only rotate if there's input
        {
            pivot.SetActive(true);
            float angle = Mathf.Atan2(rightStickY, rightStickX) * Mathf.Rad2Deg; // Convert to degrees
            pivot.transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation to Z-axis
        }else{
            pivot.SetActive(false);
        }
    }
}
