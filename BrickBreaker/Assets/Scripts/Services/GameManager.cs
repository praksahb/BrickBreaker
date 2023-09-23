using BrickBreaker.Ball;
using UnityEngine;

namespace BrickBreaker
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BallView ballPrefab;
        [SerializeField] private int ballSpeed;
        [SerializeField] private int poolSize;
        [SerializeField] private Transform poolBox;

        private GameObject firePoint;
        private BoundaryManager boundaryManager;
        private BallServicePool ballServicePool;

        private void Awake()
        {
            boundaryManager = GetComponent<BoundaryManager>();
            SetFirePoint();
        }

        private void SetFirePoint()
        {
            firePoint = new GameObject("FirePointer");
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
            // manually modifying launch point;
            point.y += 0.15f; point.z = 0;
            firePoint.transform.position = point;
        }

        private void Start()
        {
            ballServicePool = new BallServicePool(poolSize, ballSpeed, ballPrefab, poolBox, firePoint.transform);
            boundaryManager.SetBoundaries();
            LaunchBalls();
        }

        private void LaunchBalls()
        {
            for (int i = 0; i < poolSize; i++)
            {
                BallController ball = ballServicePool.GetBall();
                if (ball != null)
                {
                    ball.BallView.SetLaunchBall(firePoint.transform);
                    ball.ReturnBall += ReturnBall;
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