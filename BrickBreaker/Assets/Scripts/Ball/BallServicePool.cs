using UnityEngine;

namespace BrickBreaker.Ball
{
    public class BallServicePool
    {
        private int speed;
        private GenericPooling<BallController> ballPool;
        private Transform firePoint;
        private BallView ballPrefab;

        public BallServicePool(int amount, int speed, BallView ballPrefab, Transform firePoint)
        {
            this.ballPrefab = ballPrefab;
            this.speed = speed;
            this.firePoint = firePoint;
            ballPool = new GenericPooling<BallController>(amount, CreateBall);
        }

        //factory func delegate

        public BallController CreateBall()
        {
            BallModel ballModel = new BallModel(speed);
            return new BallController(ballModel, ballPrefab, firePoint);
        }

        public BallController GetBall()
        {
            BallController ball = ballPool.GetObject();
            ball.BallView.SetBallActive(true);
            return ball;
        }

        public void ReturnBall(BallController ball, Transform firePoint)
        {
            ball.BallView.SetBallActive(false);
            ball.BallView.ResetBall(firePoint);
            ballPool.ReturnObject(ball);
        }
    }
}
