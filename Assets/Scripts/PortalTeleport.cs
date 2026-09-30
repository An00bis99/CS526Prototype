using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public GameObject playerBall;
    private PlayerMovement playerScript;

    private float portalAngle;
    private bool exitingPortal;
    private Collider2D myCollider;

    [SerializeField]
    private GameObject TwinPortal;
    private PortalTeleport twinScript;
    private float twinAngle;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
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
        // Changes balls position to portal and adjusts angle based on portalAngle
        Vector3 portalLoc = transform.position; // going to be exit offset
        Quaternion originalAngle = playerBall.transform.rotation;
        Vector3 ballRotateToSet = playerBall.transform.rotation.eulerAngles;

        // Portal Rotation Calc
        if (portalAngle + twinAngle != 360.0f)
        {
            // If angle is opposite, then all this is skipped because angle remains unchanged
            if (portalAngle == twinAngle)
            {
                // Portals are facing the same direction
                ballRotateToSet.z = 180.0f - ballRotateToSet.z;
            }
            else if (portalAngle == twinAngle + 90.0f)
            {
                // Exit is rotated clockwise
                if (portalAngle >= 180.0f)
                {
                    ballRotateToSet.z = ballRotateToSet.z - 180.0f;
                }
                // otherwise, keep current angle
            }
            else if (portalAngle == twinAngle - 90.0f)
            {
                // Exit rotate counterclockwise
                if (portalAngle >= 180.0f)
                {
                    ballRotateToSet.z = 180.0f - ballRotateToSet.z;
                }
                else
                {
                    ballRotateToSet.z = ballRotateToSet.z - 180.0f;
                }
            }
        }

        // Offset calculations
        // Local y direction accurately shows where portal is facing so use that as it's "normal"
        if (portalAngle == 0.0f)
        {
            // Up Exit
            portalLoc += new Vector3(0.0f, 5.0f, 0.0f);
        }
        else if (portalAngle == 90.0f)
        {
            // Right Exit
            portalLoc += new Vector3(5.0f, 0.0f, 0.0f);
        }
        else if (portalAngle == 180.0f)
        {
            // Down Exit
            portalLoc += new Vector3(0.0f, -5.0f, 0.0f);
        }
        else
        {
            // Left Exit
            portalLoc += new Vector3(-5.0f, 0.0f, 0.0f);
        }
        playerBall.transform.position = portalLoc;

        // Transfer ball angle
        playerBall.transform.rotation = Quaternion.Euler(ballRotateToSet);

        exitingPortal = !exitingPortal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ball" && !exitingPortal)
        {
            twinScript.ExitPortal();
        }
    }


}
