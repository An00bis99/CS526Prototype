using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public GameObject playerBall;
    private PlayerMovement playerScript;

    private float portalAngle;
    private bool touchingPortal;
    private Collider2D myCollider;

    [SerializeField]
    private GameObject TwinPortal;
    private PortalTeleport twinScript;
    private float twinAngle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        touchingPortal = false;
        if (TwinPortal == null)
        {
            Debug.Log("Portal is missing a twin!");
        }
        myCollider = GetComponent<Collider2D>();
        portalAngle = transform.rotation.eulerAngles.z;

    }
    private void Start()
    {
        twinScript = TwinPortal.GetComponent<PortalTeleport>();
        twinAngle = twinScript.portalAngle;

        playerScript = playerBall.GetComponent<PlayerMovement>();
    }

    private void Update()
    {

    }

    public void ExitPortal()
    {
        touchingPortal = true;
        // Changes balls position to portal and adjusts angle based on portalAngle
        Vector3 portalLoc = transform.position; // going to be exit offset
        Quaternion originalAngle = playerBall.transform.rotation;
        Vector3 ballRotateToSet = playerBall.transform.rotation.eulerAngles;

        if (ballRotateToSet.z < 0.0f)
        {
            ballRotateToSet.z += 360.0f;
        }

        // Portal Rotation Calc
        if (portalAngle + twinAngle != 360.0f && portalAngle != twinAngle)
        {
            // If angle is opposite, then all this is skipped because angle remains unchanged
            if (portalAngle == twinAngle)
            {
                // Portals are facing the same direction
                ballRotateToSet.z = 180.0f - ballRotateToSet.z;
            }
            else if (twinAngle == portalAngle + 90.0f)
            {
                // Exit rotate counterclockwise
                /*
                float offsetCalc = 270.0f + twinAngle;

                if (offsetCalc >= 360.0f)
                {
                    offsetCalc -= 360.0f;
                }

                if (ballRotateToSet.z > offsetCalc)
                {
                    ballRotateToSet.z = ballRotateToSet.z - 180.0f;
                }
                else
                {
                    ballRotateToSet.z = ballRotateToSet.z - 90.0f;
                }
                */
                if (twinAngle == 90.0f || twinAngle == 270.0f)
                {
                    // 360 - angle works
                    ballRotateToSet.z = (360.0f - ballRotateToSet.z) + 90.0f;
                    if (ballRotateToSet.z >= 360.0f)
                    {
                        ballRotateToSet.z -= 360.0f;
                    }
                }
                else
                {
                    // angle - 360 needed
                    ballRotateToSet.z = Mathf.Abs((ballRotateToSet.z - 360.0f) + 90.0f);
                }

                // Same angle so don't change in greater or equal to
            }
            else if (twinAngle == portalAngle - 90.0f)
            {
                // Exit is rotated clockwise
                /*
                float offsetCalc = 270.0f + twinAngle;

                if (offsetCalc >= 360.0f)
                {
                    offsetCalc -= 360.0f;
                }
                */

                // Pos half is reflected, so original - 180
                // Cutoff for "top half" angle is messed up because portals are oriented
                // differently
                /*
                if (ballRotateToSet.z > offsetCalc)
                {
                    ballRotateToSet.z = ballRotateToSet.z - 180.0f;
                }
                else
                {
                    ballRotateToSet.z = ballRotateToSet.z - 90.0f;
                }
                */

                if (twinAngle == 0.0f || twinAngle == 180.0f)
                {
                    // 360 - angle works
                    ballRotateToSet.z = (360.0f - ballRotateToSet.z) + 90.0f;
                    if (ballRotateToSet.z >= 360.0f)
                    {
                        ballRotateToSet.z -= 360.0f;
                    }
                }
                else
                {
                    // angle - 360 needed
                    ballRotateToSet.z = Mathf.Abs((ballRotateToSet.z - 360.0f) + 90.0f);
                }
                //ballRotateToSet.z = (offsetCalc - ballRotateToSet.z) + 90.0f;
            }
        }
        else if (twinAngle == portalAngle)
        {
            ballRotateToSet.z = (2.0f * twinAngle) - ballRotateToSet.z;
            if (ballRotateToSet.z < 0.0f)
            {
                ballRotateToSet.z += 360.0f;
            }
        }

        // Offset calculations
        // Local y direction accurately shows where portal is facing so use that as it's "normal"
        if (portalAngle == 0.0f)
        {
            // Up Exit
            portalLoc += new Vector3(0.0f, 0.5f, 0.0f);
        }
        else if (portalAngle == 90.0f)
        {
            // Left Exit
            portalLoc += new Vector3(-0.5f, 0.0f, 0.0f);
        }
        else if (portalAngle == 180.0f)
        {
            // Down Exit
            portalLoc += new Vector3(0.0f, -0.5f, 0.0f);
        }
        else
        {
            // Right Exit
            portalLoc += new Vector3(0.5f, 0.0f, 0.0f);
        }

        // TODO Call function to teleport ball, apply rotation, and maintain speed
        // For now, it will be done here but should be relegated to playerMovement
        playerBall.transform.position = portalLoc;

        // Transfer ball angle
        playerBall.transform.rotation = Quaternion.Euler(ballRotateToSet);

        playerBall.GetComponent<Rigidbody2D>().linearVelocity = playerBall.GetComponent<Rigidbody2D>().linearVelocity.magnitude * playerBall.transform.right;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ball" && !touchingPortal)
        {
            touchingPortal = true;
            twinScript.ExitPortal();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            touchingPortal = false;
        }
    }


}
