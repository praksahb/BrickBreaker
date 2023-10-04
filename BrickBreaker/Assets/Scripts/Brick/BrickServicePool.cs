using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickServicePool
    {
        private GenericPooling<BrickController> brickPool;
        private Bricks baseBrick;
        private Transform parentObj;

        // create brick from Base Brick values
        public BrickServicePool(int amount, Bricks baseBrick, Transform parentObj)
        {
            this.baseBrick = baseBrick;
            this.parentObj = parentObj;
            brickPool = new GenericPooling<BrickController>(amount, CreateBrick);
        }

        public BrickController CreateBrick()
        {
            BrickModel brickModel = new BrickModel(baseBrick.brickBreakValue, baseBrick.brickWidth, baseBrick.brickHeight);
            return new BrickController(brickModel, baseBrick.brickPrefab, parentObj);
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
