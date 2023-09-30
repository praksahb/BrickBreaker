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

        private int rows = 1;
        private int columns = 10;

        private Camera mainCamera;
        private BrickServicePool brickPool;

        private BrickController[,] brickGrid; // 2D array stores all the bricks which can populate the grid box 
        private Vector2[,] gridPosition; // 2D array storing position values for the bricks

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            InitializeBrickPool();
            DefineGridSize();
            SetupBrickGrid();
        }

        private void InitializeBrickPool()
        {
            // Currently only one type of brick, and the generic Pooling is also modelled in a way to accomodate only a single type of object.
            Bricks brickType = brickSO.allBricks[0];
            brickPool = new BrickServicePool(totalBricks, brickType, brickPoolParent);
        }

        private void DefineGridSize()
        {
            float totalLength, totalHeight;
            FindGridArea(out totalLength, out totalHeight);

            BrickController temp = brickPool.GetBrick();
            temp.ReturnBrick += ReturnBrick;

            // cal. rows and columns which can be fit into the totalLength and totalHeight of top half of screen
            float brickWidth = temp.BrickModel.BrickWidth;
            float brickHeight = temp.BrickModel.BrickHeight;
            CalculateRows(totalHeight, brickHeight);
            CalculateColumns(totalLength, brickWidth, out float leftoverSpace);
            //Debug.Log("Rows: " + rows);
            //Debug.Log("Cols: " + columns);

            // calculate starting point to set brickPoolParent at
            CalculateStartingPoint(brickHeight, brickWidth, leftoverSpace);

            // plot transform values for the bricks
            SetupGridPositions(temp);

            // return temp brick used for measurement
            temp.ReturnBrick?.Invoke(temp);
        }

        private void SetupGridPositions(BrickController brick)
        {
            gridPosition = new Vector2[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Calculate the position based on row and column indices
                    float xPosition = col * (brick.BrickModel.BrickWidth + brickOffsetX);
                    float yPosition = -row * (brick.BrickModel.BrickHeight + brickOffsetY);

                    gridPosition[row, col] = new Vector2(xPosition, yPosition);
                }
            }
        }

        // Populate the array with bricks in a basic rectangular shape
        private void SetupBrickGrid()
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

                    // Set the brick's position and assign it in brickGrid
                    brick.BrickView.SetPosition(new Vector2(xPosition, yPosition));
                    brickGrid[row, col] = brick;
                }
            }
        }

        // Helpers for defining the size of the grid(rows and column values) from the screen size
        // Calculate the length and height of the box
        private void FindGridArea(out float boxLength, out float boxHeight)
        {
            float screenHeight = mainCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * mainCamera.aspect;
            float halfScreenHeight = screenHeight / 2f;

            Vector3 boxSize = new Vector3(screenWidth, halfScreenHeight, 0f);

            boxLength = boxSize.x;
            boxHeight = boxSize.y;
        }

        // Calculate the number of columns that can fit in the box
        private void CalculateColumns(float boxLength, float brickWidth, out float leftoverSpace)
        {
            float availableLength = boxLength - brickOffsetX;
            columns = Mathf.FloorToInt(availableLength / (brickWidth + brickOffsetX));
            // Calculate the leftover space at the end
            leftoverSpace = boxLength - (columns * (brickWidth + brickOffsetX));
        }

        private void CalculateRows(float boxHeight, float brickHeight)
        {
            float availableHeight = boxHeight - brickOffsetY;
            rows = Mathf.FloorToInt(availableHeight / (brickHeight + brickOffsetY));
        }
        // the parent obj - brickPool will be set at the top left corner
        private void CalculateStartingPoint(float brickHeight, float brickWidth, float leftoverSpace)
        {
            // get top left corner point from camera
            Vector2 startPoint = mainCamera.ViewportToWorldPoint(new Vector2(0, 1));
            // center the grid columns to be equi-distant from both ends
            float adjustedLeftoverSpace = leftoverSpace + brickOffsetX;

            startPoint.x += adjustedLeftoverSpace / 2 + brickWidth / 2;
            startPoint.y -= brickOffsetY + brickHeight / 2;

            brickPoolParent.position = startPoint;
        }

        // brickView invokes ReturnBrick action once its value reaches to 0
        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;
            brickPool.ReturnBrick(brick);
        }
    }
}
