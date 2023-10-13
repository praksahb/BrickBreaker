using BrickBreaker.Services;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickServicePool
    {
        private GenericPooling<BrickController> brickPool;
        private Bricks baseBrick;
        private Transform parentObj;

        private GameManager gameManager;

        // create brick from Base Brick values
        public BrickServicePool(int amount, Bricks baseBrick, Transform parentObj, GameManager gameManager)
        {
            this.baseBrick = baseBrick;
            this.parentObj = parentObj;
            this.gameManager = gameManager;
            brickPool = new GenericPooling<BrickController>(amount, CreateBrick);
        }

        public BrickController CreateBrick()
        {
            BrickModel brickModel = new BrickModel(baseBrick.brickWidth, baseBrick.brickHeight);
            return new BrickController(brickModel, baseBrick.brickPrefab, parentObj, gameManager);
        }

        public BrickController GetBrick()
        {
            BrickController brick = brickPool.GetObject();
            brick.DefaultActivate();
            return brick;
        }

        public void ReturnBrick(BrickController brick)
        {
            brickPool.ReturnObject(brick);
        }
    }
}
