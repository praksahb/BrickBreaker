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
        [SerializeField] private Transform poolBox;
        [SerializeField] private Transform firePoint;

        public Camera MainCamera
        {
            get
            {
                return mainCamera;
            }
        }

        private BoundaryManager boundaryManager;
        private BallServicePool ballServicePool;

        private Coroutine StartGameCoroutine;

        private void Awake()
        {
            boundaryManager = GetComponent<BoundaryManager>();
            if (boundaryManager)
            {
                boundaryManager.MainCamera = mainCamera;
            }
        }

        private void Start()
        {
            ballServicePool = new BallServicePool(poolSize, ballSpeed, ballPrefab, poolBox, firePoint.transform);
            SetFirePoint();
            boundaryManager.SetBoundaries();
        }

        private void SetFirePoint()
        {
            Vector3 point = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
            // manually modifying launch point;
            point.y += 0.15f; point.z = 0;
            firePoint.transform.position = point;
        }

        public void StartGame(Vector3 direction)
        {
            StartCoroutine(LaunchBalls(direction));
        }

        private IEnumerator LaunchBalls(Vector3 dir)
        {
            for (int i = 0; i < poolSize; i++)
            {
                BallController ball = ballServicePool.GetBall();
                if (ball != null)
                {
                    ball.BallView.SetLaunchBall(dir);
                    ball.ReturnBall += ReturnBall;
                    yield return new WaitForSecondsRealtime(0.05f);
                }
            }
        }

        private void ReturnBall(BallController ball)
        {
            ball.ReturnBall -= ReturnBall;
            ballServicePool.ReturnBall(ball, firePoint.transform);
        }
    }
}