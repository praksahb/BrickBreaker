using BrickBreaker;
using UnityEngine;

public class AimLineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 launchDirection = Vector3.right;
    private float lineLength = 3.0f;

    private GameManager gameManager;

    private void Start()
    {
        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        gameManager = GetComponentInParent<GameManager>();
        lineRenderer.positionCount = 2; // Two points to create a line
        lineRenderer.SetPosition(0, transform.position); // Start the line at the paddle's position
        lineRenderer.enabled = true;
    }

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        launchDirection = (mousePosition - transform.position).normalized;
        Vector3 secondPoint = new Vector3(transform.position.x + launchDirection.x, 0f, 0f);

        lineRenderer.SetPosition(1, mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.enabled = false;
            LaunchBall(mousePosition);
        }
    }

    private void LaunchBall(Vector3 direction)
    {
        gameManager.StartGame(direction);
    }
}
