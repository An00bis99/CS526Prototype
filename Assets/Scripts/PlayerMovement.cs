using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private ControlsClass playerControls;
    [Header("Launch Settings")]

    [SerializeField] private float minimumLaunchSpeed = 5f;
    [SerializeField] private float maximumLaunchSpeed = 20f;
    [SerializeField] private float SPEED_MULT = 15.0f;

    private float potentialSpeed;

    private Rigidbody2D ballPhysics;
    private bool isRolling;

    private void Awake()
    {
        playerControls = new ControlsClass();
        ballPhysics = GetComponent<Rigidbody2D>();
        potentialSpeed = 0.0f;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        ReadDirectionInput();
        ReadLaunchInput();
        if (ballPhysics.linearVelocity == Vector2.zero)
        {
            isRolling = false;
        }
    }
    private void ReadDirectionInput()
    {


        if (playerControls.Player.RotateLeft.IsPressed())
        {
            // Apply rotation
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, 0.0f, 50.0f * Time.deltaTime));

        }
        else if (playerControls.Player.RotateRight.IsPressed())
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - new Vector3(0.0f, 0.0f, 50.0f * Time.deltaTime));
        }

    }

    private void ReadLaunchInput()
    {

        if (!isRolling)
        {
            if (playerControls.Player.Shoot.IsPressed())
            {
                if (potentialSpeed < maximumLaunchSpeed)
                {
                    potentialSpeed += 5.0f * Time.deltaTime;
                }
                else
                {
                    potentialSpeed = maximumLaunchSpeed;
                }
            }
            else
            {
                if (potentialSpeed >= minimumLaunchSpeed)
                {
                    // Launch ball
                    isRolling = true;
                    ballPhysics.AddForce(transform.right * potentialSpeed * SPEED_MULT);
                }
                potentialSpeed = 0.0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Goal")
        {
            // Load next scene
        }
    }
}
