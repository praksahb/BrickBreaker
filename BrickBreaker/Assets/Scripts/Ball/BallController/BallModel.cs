namespace BrickBreaker.Ball
{
    public class BallModel
    {
        // ctor
        public BallModel(int speed)
        {
            ballSpeed = speed;
        }

        private int ballSpeed;
        public int BallSpeed
        {
            get
            {
                return ballSpeed;
            }
            set
            {
                ballSpeed = value;
            }
        }
    }
}
