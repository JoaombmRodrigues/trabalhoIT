using UnityEngine;
using UnityEngine.UI;

public class CloudsController : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private float movementSpeed = 1;

    public void SetMovementSpeed(float ms)
    {
        movementSpeed = ms;
    }
    private void FixedUpdate()
    {
        Vector3 NewPosition = transform.position;
        NewPosition.x -= movementSpeed;
        transform.position = NewPosition;
        if (NewPosition.x <= -70) {
            Destroy(this.gameObject);
        }
    }
}
