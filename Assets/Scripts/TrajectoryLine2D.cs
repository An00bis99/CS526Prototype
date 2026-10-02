using UnityEngine;

public class TrajectoryLine2D : MonoBehaviour
{
    [SerializeField] private float maxDist = 20.0f;
    [SerializeField] private LayerMask collisionLayers;

    private LineRenderer myLineRenderer;

    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        // Start the line at the object's current position
        Vector2 startPos = transform.position;

        // Calculate the default end position based on transform.right
        Vector2 direction = transform.right;
        Vector2 endPos = startPos + (direction * maxDist);

        // Cast a ray to check if the line hits an obstacle
        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, maxDist, collisionLayers);

        if (hit.collider != null)
        {
            // If the ray hits something, stop the line at the collision point
            endPos = hit.point;
        }

        // Update the Line Renderer positions
        myLineRenderer.SetPosition(0, startPos);
        myLineRenderer.SetPosition(1, endPos);
    }
}
