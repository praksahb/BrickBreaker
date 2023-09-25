using BrickBreaker.Ball;
using System.Collections;
using UnityEngine;

namespace BrickBreaker
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BallView ballPrefab;
        [SerializeField] private int ballSpeed;
        [SerializeField] private int poolSize;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float AimLineLength;
        [SerializeField] private int maxReflections;
        [SerializeField] private float lineOffset;

        public Camera MainCamera
        {
            get
            {
                return mainCamera;
            }
        }

        private BoundaryManager boundaryManager;
        private AimLineController aimLineController;
        private BallServicePool ballServicePool;
        private Coroutine LaunchBallCoroutine;

        private bool isAiming;
        private int count;

        private void Awake()
        {
            boundaryManager = GetComponent<BoundaryManager>();
            aimLineController = firePoint.GetComponent<AimLineController>();
            if (boundaryManager && aimLineController)
            {
                boundaryManager.MainCamera = mainCamera;
                aimLineController.MainCamera = mainCamera;
            }
        }

        private void Start()
        {
            ballServicePool = new BallServicePool(poolSize, ballSpeed, ballPrefab, firePoint.transform);
            SetFirePoint();
            boundaryManager.SetBoundaries();
            aimLineController.SetLineValues(AimLineLength, maxReflections, lineOffset);
            isAiming = true;
        }

        private void Update()
        {
            if (isAiming)
            {
                aimLineController.LineRenderer.enabled = true;
                aimLineController.Aim();
                aimLineController.DrawReflectedTrajectory();
            }

            if (Input.GetMouseButtonDown(0) && isAiming)
            {
                aimLineController.LineRenderer.enabled = false;
                isAiming = false;
                LaunchCoroutine();
            }
        }

        private void SetFirePoint()
        {
            Vector3 point = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
            // manually modifying launch point;
            point.y += 0.15f; point.z = 0;
            firePoint.transform.position = point;
        }


        private void LaunchCoroutine()
        {
            if (LaunchBallCoroutine != null)
            {
                StopCoroutine(LaunchBallCoroutine);
            }
            LaunchBallCoroutine = StartCoroutine(LaunchBalls());
            count = 0;
        }

        private IEnumerator LaunchBalls()
        {
            for (int i = 0; i < poolSize; i++)
            {
                BallController ball = ballServicePool.GetBall();
                if (ball != null)
                {
                    ball.BallView.LaunchBall();
                    ball.ReturnBall += ReturnBall;
                    yield return new WaitForSecondsRealtime(0.05f);
                }
            }
        }

        private void ReturnBall(BallController ball)
        {
            ball.ReturnBall -= ReturnBall;
            ballServicePool.ReturnBall(ball, firePoint.transform);
            count++;
            if (count == poolSize)
            {
                isAiming = true;
            }
        }
    }
}