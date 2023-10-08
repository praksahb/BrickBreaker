namespace BrickBreaker.Bricks
{
    public class BrickLayout
    {
        public float brickWidth;
        public float brickHeight;
        public float brickOffsetX;
        public float brickOffsetY;

        public BrickLayout(float brickWidth, float brickHeight, float brickOffsetX = 0, float brickOffsetY = 0)
        {
            this.brickWidth = brickWidth;
            this.brickHeight = brickHeight;
            this.brickOffsetX = brickOffsetX;
            this.brickOffsetY = brickOffsetY;
        }
    }

}
