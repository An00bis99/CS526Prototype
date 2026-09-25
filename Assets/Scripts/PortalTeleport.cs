using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PortalTeleport : MonoBehaviour
{
    public GameObject playerBall;
    private PlayerMovement playerScript;
    [SerializeField]
    private GameObject TwinPortal;
    [SerializeField]
    private int portalAngle;
    private Collider2D myCollider;

    private PortalTeleport twinScript;
    private int twinAngle;
    private bool exitingPortal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if (TwinPortal == null)
        {
            Debug.Log("Portal is missing a twin!");
        }
        myCollider = GetComponent<Collider2D>();

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
        Vector3 portalLoc = transform.position;
        Quaternion originalAngle = playerBall.transform.rotation;
        Quaternion newAngle; // Z Rotation is all that is used because 2D topdown
        Vector3 eulerOriginal = originalAngle.eulerAngles;
        switch (portalAngle)
        {
            case 0:
                // Straight Up
                portalLoc.y += 5;

                break;
            case 90:
                portalLoc.x += 5;
                break;
            case 180:
                portalLoc.y -= 5;
                break;
            case 270:
                portalLoc.x -= 5;
                break;
        }

        playerBall.transform.position = portalLoc;
        // Transfer ball angle


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
