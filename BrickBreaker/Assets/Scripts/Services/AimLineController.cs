using BrickBreaker;
using UnityEngine;

public class AimLineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 launchDirection;

    private Vector3 targetPosition;

    private GameManager gameManager;

    private int maxReflections = 5;
    public float minReflectionDirectionMagnitude = 0.1f;

    public float lineSegmentLength = 2f;

    private bool isDrawing = true;

    private void Awake()
    {
        gameManager = GetComponentInParent<GameManager>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.positionCount = 1;
        lineRenderer.enabled = true;
    }

    private void Update()
    {
        Aim();
    }

    private void FixedUpdate()
    {
        DrawLine();
    }

    // Method 1 - Render Launch Direction and launch 
    private void RenderLine()
    {
        Vector3 mousePosition = gameManager.MainCamera.ScreenToWorldPoint(Input.mousePosition);

        launchDirection = (mousePosition - transform.position).normalized;
        Vector3 secondPoint = new Vector3(transform.position.x + launchDirection.x, 0f, 0f);

        lineRenderer.SetPosition(1, mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.enabled = false;
            LaunchBall(launchDirection);
        }
    }

    // Method 2 - Using mouse to move transform of firePoint
    // and launch using the transform.up value

    private void Aim()
    {
        targetPosition = gameManager.MainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 difference = targetPosition - transform.position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.rotation = rotation;
    }

    private void DrawLine()
    {
        Vector2 direction = transform.up;
        Vector2 currentPosition = transform.position;

        lineRenderer.positionCount = 1; // Reset LineRenderer to one point

        for (int i = 0; i < maxReflections; i++)
        {
            // Cast a ray in the current direction
            RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction);

            if (hit.collider != null)
            {

                // Hit an object, calculate reflection
                Vector2 reflection = Vector2.Reflect(direction, hit.normal);

                // Extend the LineRenderer to the hit point
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                // Update the current position and direction
                currentPosition = hit.point;
                direction = reflection;
            }
            else
            {
                // No collision, exit the loop
                break;
            }
        }
    }

    Vector2 CalculateReflection(Vector2 incomingDirection, Vector2 hitNormal)
    {
        return incomingDirection - 2 * Vector2.Dot(incomingDirection, hitNormal) * hitNormal;
    }

    private void LaunchBall(Vector3 direction)
    {
        gameManager.StartGame(direction);
    }
}
