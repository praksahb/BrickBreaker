using BallBreaker.Ball;
using BrickBreaker.Ball;
using System.Collections.Generic;
using UnityEngine;

namespace BallBreaker
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BallView ballPrefab;
        [SerializeField] private int ballSpeed;
        [SerializeField] private int poolSize;
        [SerializeField] private GameObject poolBox;

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
            point.z = 0;
            firePoint.transform.position = point;
        }

        private void Start()
        {
            ballServicePool = new BallServicePool(poolSize, ballSpeed, ballPrefab, poolBox.transform);
            boundaryManager.SetBoundaries();
            LaunchBalls();
        }

        private List<BallController> ballList;

        private void FixedUpdate()
        {

        }

        private void LaunchBalls()
        {
            for (int i = 0; i < poolSize; i++)
            {
                BallController ball = ballServicePool.GetBall();
                if (ball != null)
                {
                    ball.BallView.SetBallActive(true);
                    ball.BallView.SetLaunchBall(firePoint.transform);
                }
            }
        }

        public void AddBalls()
        {
            for (int i = 0; i < poolSize; i++)
            {
                ballList.Add(ballServicePool.GetBall());
            }
        }

        private void RemoveBalls()
        {
            for (int i = 0; i < ballList.Count; i++)
            {
                BallController ball = ballList[i];
                ballServicePool.ReturnBall(ball);
            }
        }
    }
}