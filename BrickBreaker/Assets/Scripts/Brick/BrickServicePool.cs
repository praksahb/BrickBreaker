using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickServicePool
    {
        private GenericPooling<BrickController> brickPool;
        private Bricks brickType;
        private Transform parentObj;

        public BrickServicePool(int amount, Bricks brickType, Transform parentObj)
        {
            this.brickType = brickType;
            this.parentObj = parentObj;
            brickPool = new GenericPooling<BrickController>(amount, CreateBrick);
        }

        public BrickController CreateBrick()
        {
            // assign values or directly pass into params

            //int brickVal = brickType.brickBreakValue;
            //float brickWidth = brickType.brickWidth;
            //float brickHeight = brickType.brickHeight;
            //BrickView brickPrefab = brickType.brickPrefab;

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
