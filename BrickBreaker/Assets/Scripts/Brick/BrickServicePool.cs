using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickServicePool
    {
        private GenericPooling<BrickController> brickPool;
        private Bricks brickType;
        private Transform parentObj;
        private int brickVal;
        private float brickWidth;
        private float brickHeight;
        private BrickView brickPrefab;

        // create brick from prefab fixed size
        public BrickServicePool(int amount, Bricks brickType, Transform parentObj)
        {
            this.brickType = brickType;
            this.parentObj = parentObj;
            brickPool = new GenericPooling<BrickController>(amount, CreateBrick);
        }

        // create brick according to size given
        public BrickServicePool(int amount, BrickView brickPrefab, Transform parentObj, int brickVal, float brickWidth, float brickHeight)
        {
            this.brickVal = brickVal;
            this.brickWidth = brickWidth;
            this.brickHeight = brickHeight;
            this.brickPrefab = brickPrefab;
            this.parentObj = parentObj;
            brickPool = new GenericPooling<BrickController>(amount, CreateBrickCustom);
        }

        public BrickController CreateBrickCustom()
        {
            BrickModel brickModel = new BrickModel(brickVal, brickWidth, brickHeight);
            return new BrickController(brickModel, brickPrefab, parentObj);
        }

        public BrickController CreateBrick()
        {
            BrickModel brickModel = new BrickModel(brickType.brickBreakValue, brickType.brickWidth, brickType.brickHeight);
            return new BrickController(brickModel, brickType.brickPrefab, parentObj);
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
