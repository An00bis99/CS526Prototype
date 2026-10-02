using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    private ControlsClass playerControls;
    [Header("Shot Parameters (tweak speeds here)")]

    [SerializeField] private float minimumLaunchSpeed = 5f;
    [SerializeField] private float maximumLaunchSpeed = 25f;
    [SerializeField] private float SPEED_MULT = 15.0f;

    private float potentialSpeed;
    private Vector2 lastVel;

    private Rigidbody2D ballPhysics;
    public bool isRolling;
    private Vector3 originalPosition;
    private Quaternion originalDirection;
    private LineRenderer myLine;

    private void Awake()
    {
        playerControls = new ControlsClass();
        ballPhysics = GetComponent<Rigidbody2D>();
        myLine = GetComponent<LineRenderer>();
        potentialSpeed = 0.0f;
        originalPosition = transform.position;
        originalDirection = transform.rotation;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void FixedUpdate()
    {
        lastVel = ballPhysics.linearVelocity;
        if (lastVel.magnitude <= 1.25f && lastVel.magnitude > 0.0f)
        {
            if (!isRolling && lastVel.magnitude != 0.0f)
            {
                ballPhysics.linearVelocity = Vector2.zero;
                transform.position = originalPosition;
                transform.rotation = originalDirection;
                myLine.enabled = true;
            }
            else
            {
                isRolling = false;
            }
        }
    }

    void Update()
    {
        ReadRotationInput();
        ReadShootInput();
    }

    private void ReadRotationInput()
    {

        if (!isRolling)
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

    }

    private void ReadShootInput()
    {

        if (!isRolling)
        {
            if (playerControls.Player.Shoot.IsPressed())
            {
                if (potentialSpeed < maximumLaunchSpeed)
                {
                    potentialSpeed += 25.0f * Time.deltaTime;
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
                    myLine.enabled = false;
                    ballPhysics.AddForce(transform.right * potentialSpeed * SPEED_MULT);
                }
                potentialSpeed = 0.0f;
            }
        }
        else
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Goal")
        {
            // Load next scene
        }
        else if (collision.collider.tag == "Wall")
        {
            /*
            // Bounce according to equation
            float speed = lastVel.magnitude;
            float wallAngle = collision.transform.rotation.z;
            float angleToSet = (2.0f * wallAngle) - transform.rotation.z;
            Vector3 vecToSet = transform.rotation.eulerAngles;
            vecToSet.z = angleToSet;
            transform.rotation = Quaternion.Euler(vecToSet);
            ballPhysics.linearVelocity = transform.right * speed;
            */
            Vector2 wallNormal = collision.GetContact(0).normal;
            Vector2 bounceVelocity = Vector2.Reflect(lastVel, wallNormal);

            ballPhysics.linearVelocity = bounceVelocity;

            if (bounceVelocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(
                    bounceVelocity.y,
                    bounceVelocity.x
                ) * Mathf.Rad2Deg;

                ballPhysics.SetRotation(angle);
            }
        }
    }
}
