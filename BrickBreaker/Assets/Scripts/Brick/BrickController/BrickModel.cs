namespace BrickBreaker.Bricks
{
    public class BrickModel
    {
        private int brickValue;
        public int BrickValue
        {
            get
            {
                return brickValue;
            }
            set
            {
                brickValue = value;
            }
        }

        private float brickWidth;
        public float BrickWidth
        {
            get
            {
                return brickWidth;
            }
            set
            {
                brickWidth = value;
            }
        }

        private float brickHeight;
        public float BrickHeight
        {
            get
            {
                return brickHeight;
            }
            set
            {
                brickHeight = value;
            }
        }

        public BrickState CurrentState { get; private set; }

        public BrickState NextState { get; set; }

        public void UpdateCurrentState()
        {
            CurrentState = NextState;
            NextState = BrickState.None;
        }

        public BrickModel(int brickVal, float brickWidth, float brickHeight)
        {
            BrickValue = brickVal;
            BrickWidth = brickWidth;
            BrickHeight = brickHeight;
            CurrentState = BrickState.None;
            NextState = BrickState.None;
        }
    }
}
