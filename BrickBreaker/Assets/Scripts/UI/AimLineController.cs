using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker.Services
{
    public class AimLineController : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayerMask;
        [SerializeField] private float totalLength;
        [SerializeField] private int maxReflections;
        [SerializeField] private float lineOffset;

        private LineRenderer LineRenderer { get; set; }

        private RaycastHit2D hit;
        private Vector2 mousePosition;
        private Vector2 touchPos;
        private List<Vector3> trajectoryPoints = new List<Vector3>();

        public Camera MainCamera { get; set; }
        public bool IsAiming { get; set; }
        public Action<Vector2> FireBalls;

        private void Awake()
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Debug.Log("switch");
                AimUsingMouse();
            }
            // else
            AimUsingTouch();

        }

        private void AimUsingTouch()
        {
            if (IsAiming)
            {
                if (Input.touchCount > 0)
                {
                    LineRenderer.enabled = true;

                    Touch touch = Input.GetTouch(0);

                    TouchAim(touch.position);
                    DrawReflectedTrajectory();

                    if (touch.phase == TouchPhase.Ended)
                    {
                        LineRenderer.enabled = false;
                        IsAiming = false;
                        FireBalls?.Invoke((Vector2)transform.up);
                    }
                }
            }
        }

        private void AimUsingMouse()
        {
            if (IsAiming)
            {
                LineRenderer.enabled = true;
                MouseAim();
                DrawReflectedTrajectory();
            }

            if (Input.GetMouseButtonDown(0) && IsAiming)
            {
                LineRenderer.enabled = false;
                IsAiming = false;
                Vector2 launchPosition = transform.up;

                FireBalls?.Invoke(launchPosition);
            }
        }

        private void TouchAim(Vector2 touchPos)
        {
            touchPos = MainCamera.ScreenToWorldPoint(touchPos);

            Vector2 difference = touchPos - (Vector2)transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = rotation;
        }

        public void MouseAim()
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