using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickView brickPrefab;
        [SerializeField] private Transform brickPoolParent;
        [SerializeField] private Camera mainCamera;

        private int maxRows;
        private int maxColumns;

        private BrickServicePool brickPool;


        private BrickGrid brickGrid;

        // function is called when action is invoked from brickController
        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;
            brickPool.ReturnBrick(brick);
        }

        // create brick pool of totalBricks size
        private void InitializeCustomPool(int brickVal, float brickWidth, float brickHeight)
        {
            Bricks brick = new Bricks("Custom", brickPrefab, brickVal, brickWidth, brickHeight);
            int totalBricks = maxRows * maxColumns;
            brickPool = new BrickServicePool(totalBricks, brick, brickPoolParent);
        }

        // gets a brick from the pool
        public BrickController GetBrick()
        {
            BrickController brick = brickPool.GetBrick();
            brick.ReturnBrick += ReturnBrick;
            return brick;
        }

        public Vector2 GetBrickSize()
        {
            return new Vector2(brickPrefab.transform.localScale.x, brickPrefab.transform.localScale.y);
        }

        // creates brickGrid
        public void InitializeBrickGrid(BrickSize brick, int maxRows, int maxColumns, int brickValue)
        {
            InitializeCustomPool(brickValue, brick.brickWidth, brick.brickHeight);

            brickGrid = new BrickGrid(this, maxRows, maxColumns, brick);
        }

        // Helpers for defining the size of the grid(rows and column values) from the screen size

        // Calculate the length and height of the box
        // grid box is being defined as upper half of the screen space where bricks can be placed
        public void FindGridArea(out float boxWidth, out float boxHeight)
        {
            boxHeight = mainCamera.orthographicSize;
            boxWidth = boxHeight * 2f * mainCamera.aspect;
        }

        // the parent obj - brickPool will be set at the top left corner
        public void SetParentPosition(BrickSize brick, float leftoverSpaceX = 0, float leftoverSpaceY = 0)
        {
            // get top left corner point from camera
            Vector2 startPoint = mainCamera.ViewportToWorldPoint(new Vector2(0, 1));
            // center the grid columns to be equi-distant from both ends
            float totalSpaceX = leftoverSpaceX + brick.brickOffsetX;
            float totalSpaceY = leftoverSpaceY + brick.brickOffsetY;

            startPoint.x += totalSpaceX / 2 + brick.brickWidth / 2;
            startPoint.y -= totalSpaceY / 2 + brick.brickHeight / 2;

            brickPoolParent.position = startPoint;
        }
    }

    public class BrickSize
    {
        public float brickWidth;
        public float brickHeight;
        public float brickOffsetX;
        public float brickOffsetY;

        public BrickSize(float brickWidth, float brickHeight, float brickOffsetX = 0, float brickOffsetY = 0)
        {
            this.brickWidth = brickWidth;
            this.brickHeight = brickHeight;
            this.brickOffsetX = brickOffsetX;
            this.brickOffsetY = brickOffsetY;
        }
    }
}
