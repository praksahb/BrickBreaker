using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker.Services
{
    public class AimLineController : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayerMask;
        public LineRenderer LineRenderer { get; set; }

        private RaycastHit2D hit;
        private Vector2 mousePosition;


        private float totalLength;
        private int maxReflections;
        private float lineOffset;

        private List<Vector3> trajectoryPoints = new List<Vector3>();

        public Camera MainCamera { get; set; }

        private void Awake()
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        public void SetLineValues(float aimLineLength, int maxReflections, float lineOffset)
        {
            totalLength = aimLineLength;
            this.maxReflections = maxReflections;
            this.lineOffset = lineOffset;
        }

        public void Aim()
        {
            mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 difference = mousePosition - (Vector2)transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = rotation;
        }

        // using RayCast collision is at a point
        public void DrawReflectedTrajectory()
        {
            Vector2 direction = transform.up;
            Vector2 origin = (Vector2)transform.position + lineOffset * direction;

            LineRenderer.positionCount = 1;
            LineRenderer.SetPosition(0, origin);

            float currentLength = 0f;

            for (int i = 1; i <= maxReflections; i++)
            {
                // create ray in direction from origin
                hit = Physics2D.Raycast(origin, direction);

                //hit = Physics2D.CircleCast(origin, 0.1f, direction);

                float length = Vector2.Distance(origin, hit.point);

                currentLength += length;

                if (currentLength > totalLength)
                {
                    float remainingLength = totalLength - (currentLength - length);

                    // Calculate the final position for the current segment
                    Vector2 finalPosition = origin + direction.normalized * remainingLength;

                    // Set the final position on the LineRenderer
                    LineRenderer.positionCount++;
                    LineRenderer.SetPosition(i, finalPosition);
                    break;
                }

                // add point/vertice of collision in line renderer
                LineRenderer.positionCount++;
                LineRenderer.SetPosition(i, hit.point);

                // reinitialize direction and origin from new point of collision
                direction = Vector2.Reflect(direction.normalized, hit.normal);
                origin = hit.point + lineOffset * direction;
            }
        }

        // using circleCast with radius of ball size. 
        // doesnt reflect the lines properly.
        public void DrawReflectedTrajectory(float ballRadius)
        {
            Vector2 direction = transform.up;
            Vector2 origin = (Vector2)transform.position + lineOffset * direction;

            LineRenderer.positionCount = 1;
            LineRenderer.SetPosition(0, origin);

            float currentLength = 0f;

            for (int i = 1; i <= maxReflections; i++)
            {
                // Perform a CircleCast to find the next reflection point
                RaycastHit2D hit = Physics2D.CircleCast(origin, ballRadius, direction, totalLength, collisionLayerMask);

                if (hit.collider == null)
                {
                    break;
                }

                float length = Vector2.Distance(origin, hit.point);

                currentLength += length;

                if (currentLength > totalLength)
                {
                    float remainingLength = totalLength - (currentLength - length);

                    // Calculate the final position for the current segment
                    Vector2 finalPosition = origin + direction.normalized * remainingLength;

                    // Set the final position on the LineRenderer
                    LineRenderer.positionCount++;
                    LineRenderer.SetPosition(i, finalPosition);
                    break;
                }

                // Add point/vertice of collision to the LineRenderer
                LineRenderer.positionCount++;
                LineRenderer.SetPosition(i, hit.point);

                // Reinitialize direction and origin from the new point of collision
                direction = Vector2.Reflect(direction.normalized, hit.normal);
                origin = hit.point + lineOffset * direction;
            }
        }
    }

}