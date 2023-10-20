using UnityEngine;

namespace BrickBreaker.Ball
{
    public class BallServicePool
    {
        private GenericPooling<BallController> ballPool;

        private Transform firePoint;
        private BallData ballData;

        public BallServicePool(int amount, BallData ballData, Transform firePoint)
        {
            this.ballData = ballData;
            this.firePoint = firePoint;
            ballPool = new GenericPooling<BallController>(amount, CreateBall);
        }

        //factory func delegate

        public BallController CreateBall()
        {
            BallModel ballModel = new BallModel(ballData.ballSpeed);
            return new BallController(ballModel, ballData.ballPrefab, firePoint, ballData.ballRadius);
        }

        public BallController GetBall()
        {
            BallController ball = ballPool.GetObject();
            ball.BallView.SetBallActive(true);
            return ball;
        }

        public void ReturnBall(BallController ball)
        {
            ball.BallView.SetBallActive(false);
            ballPool.ReturnObject(ball);
        }
    }

    public class BallData
    {
        public BallView ballPrefab;
        public int ballSpeed;
        public float ballRadius;

        public BallData(BallView ballPrefab, int ballSpeed, float ballRadius)
        {
            this.ballPrefab = ballPrefab;
            this.ballSpeed = ballSpeed;
            this.ballRadius = ballRadius;
        }
    }

}
