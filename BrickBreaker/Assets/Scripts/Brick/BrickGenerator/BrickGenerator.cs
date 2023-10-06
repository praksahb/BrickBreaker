using UnityEngine;

namespace BrickBreaker.Bricks
{
    // Generate square bricks with offset - used in level 1
    public class BrickGenerator : MonoBehaviour
    {
        [SerializeField] private float brickOffsetX;
        [SerializeField] private float brickOffsetY;
        [SerializeField] private int brickValue;

        private BrickManager brickManager;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
        }

        private void Start()
        {
            DefineGrid();
        }

        private void DefineGrid() // from brick dimension
        {
            brickManager.FindGridArea(out float totalLength, out float totalHeight);

            // get brick size (w * h)
            Vector2 brickSize = brickManager.GetBrickSize();

            int rows = CalculateRows(totalHeight, brickSize.y, out float leftoverSpaceY);
            int columns = CalculateColumns(totalLength, brickSize.x, out float leftoverSpaceX);

            // get starting point for parent transform
            BrickSize brick = new BrickSize(brickSize.x, brickSize.y, brickOffsetX, brickOffsetY);
            brickManager.SetParentPosition(brick, leftoverSpaceX, leftoverSpaceY);

            // setup grid
            brickManager.InitializeBrickGrid(brick, rows, columns, brickValue);
        }

        // Calculate the number of columns that can fit in the box
        private int CalculateColumns(float boxLength, float brickWidth, out float leftoverSpaceX)
        {
            float availableLength = boxLength - brickOffsetX;
            int maxColumns = Mathf.FloorToInt(availableLength / (brickWidth + brickOffsetX));
            // Calculate the leftover space at the end
            leftoverSpaceX = boxLength - (maxColumns * (brickWidth + brickOffsetX));
            return maxColumns;
        }
        // Calculates the number of rows which can fit the box
        private int CalculateRows(float boxHeight, float brickHeight, out float leftoverSpaceY)
        {
            float availableHeight = boxHeight - brickOffsetY;
            int maxRows = Mathf.FloorToInt(availableHeight / (brickHeight + brickOffsetY));

            leftoverSpaceY = boxHeight - (maxRows * (brickHeight + brickOffsetY));
            return maxRows;
        }
    }
}
