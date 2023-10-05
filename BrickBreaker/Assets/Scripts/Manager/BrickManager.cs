using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickSO brickSO;
        [SerializeField] private Transform brickPoolParent;

        private int maxRows;
        private int maxColumns;

        private Camera mainCamera;
        private BrickServicePool brickPool;

        private BrickController[,] brickGrid; // 2D array stores all the bricks which can populate the grid box 
        private Vector2[,] gridPosition; // 2D array storing position values for the bricks

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        // brickView invokes ReturnBrick action once its value reaches to 0
        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;
            brickPool.ReturnBrick(brick);
        }

        public void InitializeBrickPool()
        {
            // Currently only one type of brick, and the generic Pooling is also modelled in a way to accomodate only a single type of object.
            Bricks brickType = brickSO.allBricks[0];
            int totalBricks = maxRows * maxColumns;
            brickPool = new BrickServicePool(totalBricks, brickType, brickPoolParent);
        }

        public void InitializeCustomPool(int brickVal, float brickWidth, float brickHeight)
        {
            BrickView brickPrefab = brickSO.allBricks[0].brickPrefab;

            Bricks brick = new Bricks("Custom", brickPrefab, brickVal, brickWidth, brickHeight);
            int totalBricks = maxRows * maxColumns;
            brickPool = new BrickServicePool(totalBricks, brick, brickPoolParent);
        }

        public Vector2 GetBrickSize()
        {
            Bricks brick = brickSO.allBricks[0];
            return new Vector2(brick.brickWidth, brick.brickHeight);
        }

        public void SetupGridPositions(float brickWidth, float brickHeight, float brickOffsetX = 0, float brickOffsetY = 0)
        {
            gridPosition = new Vector2[maxRows, maxColumns];

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    // Calculate the position based on row and column indices
                    float xPosition = col * (brickWidth + brickOffsetX);
                    float yPosition = -row * (brickHeight + brickOffsetY);
                    gridPosition[row, col] = new Vector2(xPosition, yPosition);
                }
            }
        }

        // Populate the array with bricks in a basic rectangular shape
        public void InitializeGrid()
        {
            brickGrid = new BrickController[maxRows, maxColumns];

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickPool.GetBrick();
                    brick.ReturnBrick += ReturnBrick;

                    // Get position from gridPosition 
                    float xPosition = gridPosition[row, col].x;
                    float yPosition = gridPosition[row, col].y;

                    brick.BrickView.SetPosition(new Vector2(xPosition, yPosition));
                    brickGrid[row, col] = brick;
                }
            }
        }

        // Helpers for defining the size of the grid(rows and column values) from the screen size

        public void SetGridSize(int rows, int columns)
        {
            maxRows = rows;
            maxColumns = columns;
        }

        // Calculate the length and height of the box
        // grid box is being defined as upper half of the screen space where bricks can be placed
        public void FindGridArea(out float boxWidth, out float boxHeight)
        {
            boxHeight = mainCamera.orthographicSize;
            boxWidth = boxHeight * 2f * mainCamera.aspect;
        }

        // the parent obj - brickPool will be set at the top left corner
        public void CalculateStartingPoint(BrickSize brick, float leftoverSpaceX = 0, float leftoverSpaceY = 0)
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
