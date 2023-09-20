using BrickBreaker.Ball;

namespace BallBreaker.Ball
{
    public class BallServicePool
    {
        public int amount, speed;
        private GenericPooling<BallController> ballPool;

        private BallView ballPrefab;

        public BallServicePool(int amount, int speed, BallView ballPrefab)
        {
            this.amount = amount;
            this.ballPrefab = ballPrefab;
            this.speed = speed;
            ballPool = new GenericPooling<BallController>(amount, CreateBall);
        }

        public BallController CreateBall()
        {
            BallModel ballModel = new BallModel(speed);
            return new BallController(ballModel, ballPrefab);
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
