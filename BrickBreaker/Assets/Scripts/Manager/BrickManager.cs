using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickSO brickSO;
        [SerializeField] private Transform brickPoolParent;
        [SerializeField] private int totalBricks;
        [SerializeField] private float brickOffsetX;
        [SerializeField] private float brickOffsetY;

        [SerializeField] private int desiredRows;
        [SerializeField] private int desiredColumns;
        [SerializeField] private int brickVal;

        private int rows;
        private int columns;

        private Camera mainCamera;
        private BrickServicePool brickPool;

        private BrickController[,] brickGrid; // 2D array stores all the bricks which can populate the grid box 
        private Vector2[,] gridPosition; // 2D array storing position values for the bricks

        private float brickWidth;
        private float brickHeight;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            //InitializeBrickPool();
            //DefineGrid();

            // creating custom block of bricks
            DefineGridCustom();
            InitializeCustomPool();

            InitializeGrid();
        }

        private void InitializeBrickPool()
        {
            // Currently only one type of brick, and the generic Pooling is also modelled in a way to accomodate only a single type of object.
            Bricks brickType = brickSO.allBricks[0];
            brickPool = new BrickServicePool(totalBricks, brickType, brickPoolParent);
        }

        // gets the height and width of the grid, and the starting point to place the parent of the bricks at top left corner
        private void DefineGrid() // from brick dimension
        {
            float totalLength, totalHeight;
            FindGridArea(out totalLength, out totalHeight);

            BrickController temp = brickPool.GetBrick();
            temp.ReturnBrick += ReturnBrick;
            float brickWidth = temp.BrickModel.BrickWidth;
            float brickHeight = temp.BrickModel.BrickHeight;
            temp.ReturnBrick?.Invoke(temp);

            CalculateRows(totalHeight, brickHeight, out float leftoverSpaceY);
            CalculateColumns(totalLength, brickWidth, out float leftoverSpaceX);
            CalculateStartingPoint(brickHeight, brickWidth, leftoverSpaceX, leftoverSpaceY);

            // plot transform values for the bricks
            SetupGridPositions(brickWidth, brickHeight);
        }

        private void SetupGridPositions(float brickWidth, float brickHeight)
        {
            gridPosition = new Vector2[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Calculate the position based on row and column indices
                    float xPosition = col * (brickWidth + brickOffsetX);
                    float yPosition = -row * (brickHeight + brickOffsetY);
                    gridPosition[row, col] = new Vector2(xPosition, yPosition);
                }
            }
        }

        // Populate the array with bricks in a basic rectangular shape
        private void InitializeGrid()
        {
            brickGrid = new BrickController[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
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

        // Calculate the length and height of the box
        // grid box is being defined as upper half of the screen space where bricks can be placed
        private void FindGridArea(out float boxWidth, out float boxHeight)
        {
            boxHeight = mainCamera.orthographicSize;
            boxWidth = boxHeight * 2f * mainCamera.aspect;

            Debug.Log("tot Height: " + boxHeight);
            Debug.Log("tot width: " + boxWidth);
        }

        // Calculate the number of columns that can fit in the box
        private void CalculateColumns(float boxLength, float brickWidth, out float leftoverSpaceX)
        {
            float availableLength = boxLength - brickOffsetX;
            columns = Mathf.FloorToInt(availableLength / (brickWidth + brickOffsetX));
            // Calculate the leftover space at the end
            leftoverSpaceX = boxLength - (columns * (brickWidth + brickOffsetX));
        }
        // Calculates the number of rows which can fit the box
        private void CalculateRows(float boxHeight, float brickHeight, out float leftoverSpaceY)
        {
            float availableHeight = boxHeight - brickOffsetY;
            rows = Mathf.FloorToInt(availableHeight / (brickHeight + brickOffsetY));

            leftoverSpaceY = boxHeight - (rows * (brickHeight + brickOffsetY));
        }

        // the parent obj - brickPool will be set at the top left corner
        private void CalculateStartingPoint(float brickHeight, float brickWidth, float leftoverSpaceX = 0, float leftoverSpaceY = 0)
        {
            // get top left corner point from camera
            Vector2 startPoint = mainCamera.ViewportToWorldPoint(new Vector2(0, 1));
            // center the grid columns to be equi-distant from both ends
            float totalSpaceX = leftoverSpaceX + brickOffsetX;
            float totalSpaceY = leftoverSpaceY + brickOffsetY;

            startPoint.x += totalSpaceX / 2 + brickWidth / 2;
            startPoint.y -= totalSpaceY / 2 + brickHeight / 2;

            brickPoolParent.position = startPoint;
        }

        // brickView invokes ReturnBrick action once its value reaches to 0
        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;
            brickPool.ReturnBrick(brick);
        }


        // Method 2. creating brick of fixed sizes from the desired rows and column values.
        // and feeding it to the brick initialization function to create the bricks of the specific sizes.

        private void DefineGridCustom() // from row, col values.
        {
            // Get brick sizes
            FindGridArea(out float totalWidth, out float totalHeight);
            brickWidth = totalWidth / desiredColumns;
            brickHeight = totalHeight / desiredRows;

            rows = desiredRows;
            columns = desiredColumns;

            // Get starting point for parent
            CalculateStartingPoint(brickHeight, brickWidth);

            // plot grid
            SetupGridPositions(brickWidth, brickHeight);
        }

        private void InitializeCustomPool()
        {
            BrickView brickPrefab = brickSO.allBricks[0].brickPrefab;

            Bricks brick = new Bricks("Custom", brickPrefab, brickVal, brickWidth, brickHeight);

            brickPool = new BrickServicePool(totalBricks, brick, brickPoolParent);
        }

    }
}
