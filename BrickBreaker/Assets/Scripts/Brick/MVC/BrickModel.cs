
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

        public BrickModel(int brickVal)
        {
            BrickValue = brickVal;
        }
    }
}
