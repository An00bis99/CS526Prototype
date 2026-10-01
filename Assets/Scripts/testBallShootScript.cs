using UnityEngine;
using UnityEngine.InputSystem;

public class testBallShootScript : MonoBehaviour
{
    //private float rotOffset = -90.0f;
    private Rigidbody2D myPhysics;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        myPhysics = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get mouse position from screen
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

            // Screen to world translation
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            // Direction to look at then get z rotation
            Vector2 lookDir = mouseWorldPos - transform.position;
            float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

            // Right is the local x-axis unit vector, essentially "the forward direction" of the 2D ball
            myPhysics.AddForce(transform.right * 40.0f);
        }
    }
}
