using BrickBreaker.Services;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickView brickPrefab;
        [SerializeField] private Transform brickPoolParent;

        public GameManager GameManager { get; set; }
        public Camera MainCamera { private get; set; }

        private BrickServicePool brickPool;
        private BrickGrid brickGrid;

        private Vector2 startPosition;

        private IBrickGenerator brickGenerator;


        private void Awake()
        {
            brickGenerator = GetComponentInChildren<IBrickGenerator>();
        }

        // create brick pool of totalBricks size
        private void InitializePool(float brickWidth, float brickHeight, int maxRows, int maxColumns)
        {
            Bricks brick = new Bricks("Custom", brickPrefab, brickWidth, brickHeight);
            int totalBricks = maxRows * maxColumns;
            brickPool = new BrickServicePool(totalBricks, brick, brickPoolParent, GameManager);
        }
        // creates brickGrid level 1
        public void InitializeBricks(BrickLayout brick, int maxRows, int maxColumns, PlayLevel currentLevel)
        {
            InitializePool(brick.brickWidth, brick.brickHeight, maxRows, maxColumns);
            brickGrid = new BrickGrid(this, maxRows, maxColumns, brick, currentLevel);
        }

        // create brickGrid for level 2
        public void InitializeBricks(BrickLayout brick, int maxRows, int maxColumns, PlayLevel currentLevel, float scaleValue, float thresholdValue)
        {
            // create object pool of bricks
            InitializePool(brick.brickWidth, brick.brickHeight, maxRows, maxColumns);
            // create brickGrid, having 2d arrays for bricks, positions 
            brickGrid = new BrickGrid(this, maxRows, maxColumns, brick, currentLevel, scaleValue, thresholdValue);
        }

        // perform function 1 
        public void LoadGrid()
        {
            brickGenerator?.DefineGrid();
        }

        // perform function 2 - after all balls returned
        public void TurnEffect()
        {
            brickGenerator?.ModifyGrid();
        }

        public void ResetBrickGrid()
        {
            brickPoolParent.position = startPosition;

            brickGrid.ResetBrickGrid();

            brickGrid.InitializeGrid();
        }

        // test function for perlin noise generation
        public void ResetGrid(float v1, float v2)
        {
            brickGrid.ResetBrickGrid();
            brickGrid.SetVal(v1, v2);
            brickGrid.InitializeGrid();
        }

        // gets a brick from the pool
        public BrickController GetBrick()
        {
            BrickController brick = brickPool.GetBrick();
            brick.ReturnBrick += ReturnBrick;
            return brick;
        }

        // get brick w * h from prefab
        public Vector2 GetBrickSize()
        {
            return new Vector2(brickPrefab.transform.localScale.x, brickPrefab.transform.localScale.y);
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
            GameManager.RestartGame += ResetBrickGrid;
        }

        //  function is called once level is restarted
        public void UnsubscribeRestartEvent()
        {
            GameManager.RestartGame -= ResetBrickGrid;
        }

        // move parent brick obj -1.25 (brickHeight + offsetY) in y-axis
        public void MoveBrickParentPosition(float brickHeight, float offsetY)
        {
            // storing as member variables currently till i can see how it can be called maybe from brickGenerator script as im not using singletons atm
            float moveValue = brickHeight + offsetY;
            brickPoolParent.transform.Translate(0, -moveValue, 0);

            brickGrid.AddBrickRow(startPosition);
        }

        // test function for game of life simulation
        public void RegenerateBricks()
        {
            brickGrid.RandomizeAfterTurn();

            if (brickGrid.GameOverCondition())
            {
                GameManager.GameOver?.Invoke();
                Debug.Log("chk");
            }
        }

        // Helpers for defining the size of the grid(rows and column values) from the screen size

        // Calculate the length and height of top half of screen space
        public void FindGridArea(out float boxWidth, out float boxHeight)
        {
            boxHeight = MainCamera.orthographicSize;
            boxWidth = boxHeight * 2f * MainCamera.aspect;
        }

        // the parent obj - brickPool will be set at the top left corner
        public void SetStartPosition(BrickLayout brick, float leftoverSpaceX = 0, float leftoverSpaceY = 0)
        {
            // get top left corner point from camera
            Vector2 startPoint = MainCamera.ViewportToWorldPoint(new Vector2(0, 1));
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
