using UnityEngine;

public class PlayerControls_Uma : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float minimumLaunchSpeed = 5f;
    [SerializeField] private float maximumLaunchSpeed = 20f;
    [SerializeField] private float chargeTime = 2f;

    private Rigidbody2D ballRigidbody;
    private Vector2 launchDirection = Vector2.up;

    private float currentChargeTime;
    private bool isCharging;

    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ReadDirectionInput();
        ReadLaunchInput();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BallDestroyer"))
        {
            Destroy(gameObject);
        }
    }

    private void ReadDirectionInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector2 inputDirection = new Vector2(
            horizontalInput,
            verticalInput
        );

        // Retain the previous direction when no direction key is held.
        if (inputDirection.sqrMagnitude > 0f)
        {
            launchDirection = inputDirection.normalized;
        }
    }

    private void ReadLaunchInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            currentChargeTime = 0f;
        }

        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            currentChargeTime += Time.deltaTime;
            currentChargeTime = Mathf.Min(
                currentChargeTime,
                chargeTime
            );
        }

        if (isCharging && Input.GetKeyUp(KeyCode.Space))
        {
            LaunchBall();

            isCharging = false;
            currentChargeTime = 0f;
        }
    }

    private void LaunchBall()
    {
        float chargePercentage = currentChargeTime / chargeTime;

        float launchSpeed = Mathf.Lerp(
            minimumLaunchSpeed,
            maximumLaunchSpeed,
            chargePercentage
        );

        ballRigidbody.AddForce(
            launchDirection * launchSpeed,
            ForceMode2D.Impulse
        );
    }
}