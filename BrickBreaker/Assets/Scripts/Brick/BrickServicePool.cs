namespace BrickBreaker.Bricks
{
    public class BrickServicePool
    {
        private GenericPooling<BrickController> brickPool;
        private int brickValue;
        private BrickView brickPrefab;

        public BrickServicePool(int amount, BrickView brickPrefab, int brickValue)
        {
            this.brickPrefab = brickPrefab;
            this.brickValue = brickValue;
            brickPool = new GenericPooling<BrickController>(amount, CreateBrick);
        }

        public BrickController CreateBrick()
        {
            BrickModel brickModel = new BrickModel(brickValue);
            return new BrickController(brickModel, brickPrefab);
        }

        public BrickController GetBrick()
        {
            BrickController brick = brickPool.GetObject();
            brick.BrickView.SetBrickActive(true);
            brick.BrickView.SetBrickValue();
            return brick;
        }

        public void ReturnBrick(BrickController brick)
        {
            brick.BrickView.SetBrickActive(false);
            brickPool.ReturnObject(brick);
        }
    }
}
