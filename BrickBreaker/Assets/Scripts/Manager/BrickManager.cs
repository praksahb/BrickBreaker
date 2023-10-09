using BrickBreaker.Services;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickView brickPrefab;
        [SerializeField] private Transform brickPoolParent;
        [SerializeField] private Camera mainCamera;

        private GameManager gameManager;

        private BrickServicePool brickPool;
        private BrickGrid brickGrid;

        private Vector2 startPosition;

        // create brick pool of totalBricks size
        private void InitializePool(float brickWidth, float brickHeight, int maxRows, int maxColumns)
        {
            Bricks brick = new Bricks("Custom", brickPrefab, brickWidth, brickHeight);
            int totalBricks = maxRows * maxColumns;
            brickPool = new BrickServicePool(totalBricks, brick, brickPoolParent, gameManager);
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
        public void InitializeBrickGrid(BrickLayout brick, int maxRows, int maxColumns, GameManager gameManager)
        {
            this.gameManager = gameManager;
            InitializePool(brick.brickWidth, brick.brickHeight, maxRows, maxColumns);
            brickGrid = new BrickGrid(this, maxRows, maxColumns, brick);
            brickGrid.InitializeGrid();


        }

        // function is called when action is invoked from brickController
        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;
            brickPool.ReturnBrick(brick);
        }

        // action is invoked from gameOverPanel when restart button is clicked
        public void SubscribeRestartLevel()
        {
            gameManager.RestartGame += ResetBrickGrid;
        }

        //  function is called once level is restarted
        public void UnsubscribeRestartEvent()
        {
            gameManager.RestartGame -= ResetBrickGrid;
        }

        public void ResetBrickGrid()
        {
            brickPoolParent.position = startPosition;

            brickGrid.ResetBrickGrid();

            brickGrid.InitializeGrid();
        }

        // move parent brick obj -1.25 (brickHeight + offsetY) in y-axis
        public void MoveBrickParentPosition(float brickHeight, float offsetY)
        {
            // storing as member variables currently till i can see how it can be called maybe from brickGenerator script as im not using singletons atm
            float moveValue = brickHeight + offsetY;
            brickPoolParent.transform.Translate(0, -moveValue, 0);

            brickGrid.AddBrickRow(startPosition);

        }

        // test function
        public void CheckGridWorks()
        {
            brickGrid.GridTraversal();
        }

        // Helpers for defining the size of the grid(rows and column values) from the screen size

        // Calculate the length and height of top half of screen space
        public void FindGridArea(out float boxWidth, out float boxHeight)
        {
            boxHeight = mainCamera.orthographicSize;
            boxWidth = boxHeight * 2f * mainCamera.aspect;
        }

        // the parent obj - brickPool will be set at the top left corner
        public void SetStartPosition(BrickLayout brick, float leftoverSpaceX = 0, float leftoverSpaceY = 0)
        {
            // get top left corner point from camera
            Vector2 startPoint = mainCamera.ViewportToWorldPoint(new Vector2(0, 1));
            // center the grid columns to be equi-distant from both ends
            float totalSpaceX = leftoverSpaceX + brick.brickOffsetX;
            float totalSpaceY = leftoverSpaceY + brick.brickOffsetY;

            startPoint.x += totalSpaceX / 2 + brick.brickWidth / 2;
            startPoint.y -= totalSpaceY / 2 + brick.brickHeight / 2;

            //store start point for use later in level 1
            startPosition = startPoint;
            Debug.Log("Start: " + startPosition);

            brickPoolParent.position = startPoint;
        }
    }
}
