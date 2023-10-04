
using System;

namespace BrickBreaker.Bricks
{
    public class BrickModel
    {
        public Action BreakBrick;

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
                if (brickValue == 0)
                {
                    BreakBrick?.Invoke();
                }
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

        public BrickModel(int brickVal, float brickWidth, float brickHeight)
        {
            BrickValue = brickVal;
            BrickWidth = brickWidth;
            BrickHeight = brickHeight;
        }
    }
}
