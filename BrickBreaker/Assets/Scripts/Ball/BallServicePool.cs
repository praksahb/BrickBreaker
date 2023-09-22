using BrickBreaker.Ball;
using UnityEngine;

namespace BallBreaker.Ball
{
    public class BallServicePool
    {
        private int speed;
        private GenericPooling<BallController> ballPool;
        private Transform parent;
        private BallView ballPrefab;

        public BallServicePool(int amount, int speed, BallView ballPrefab, Transform parentObj)
        {
            this.ballPrefab = ballPrefab;
            this.speed = speed;
            parent = parentObj;
            ballPool = new GenericPooling<BallController>(amount, CreateBall);
        }

        public BallController CreateBall()
        {
            BallModel ballModel = new BallModel(speed);
            return new BallController(ballModel, ballPrefab, parent);
        }

        public BallController GetBall()
        {
            return ballPool.GetObject();
        }

        public void ReturnBall(BallController ball)
        {
            ballPool.ReturnObject(ball);
        }
    }
}
