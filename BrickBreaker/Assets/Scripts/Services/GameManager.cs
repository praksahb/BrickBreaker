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

        private BallServicePool ballServicePool;

        private void Start()
        {
            ballServicePool = new BallServicePool(poolSize, ballSpeed, ballPrefab);
        }

        private List<BallController> ballList;

        private void FixedUpdate()
        {
            AddBalls();

            RemoveBalls();
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