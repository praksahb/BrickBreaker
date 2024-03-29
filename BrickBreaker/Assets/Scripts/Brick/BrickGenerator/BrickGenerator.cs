using UnityEngine;

namespace BrickBreaker.Bricks
{
    // Generate square bricks with offset - used in level 1
    public class BrickGenerator : MonoBehaviour, IBrickGenerator
    {
        [SerializeField] private float brickOffsetX;
        [SerializeField] private float brickOffsetY;

        private BrickManager brickManager;
        private float brickHeight;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
        }

        public void DefineGrid() // from brick dimension
        {
            brickManager.FindGridArea(out float totalLength, out float totalHeight);

            // get brick size (w * h)
            Vector2 brickSize = brickManager.GetBrickPrefabSize();

            int rows = CalculateRows(totalHeight, brickSize.y, out float leftoverSpaceY);
            int columns = CalculateColumns(totalLength, brickSize.x, out float leftoverSpaceX);

            // get starting point for parent transform
            BrickLayout brickLayout = new BrickLayout(brickSize.x, brickSize.y, brickOffsetX, brickOffsetY);
            brickManager.SetStartPosition(brickLayout, leftoverSpaceX, leftoverSpaceY);

            // setup grid
            brickManager.InitializeBricks(brickLayout, rows, columns);

            // store brick height value for later
            brickHeight = brickSize.y;
        }

        public void ModifyGrid()
        {
            brickManager.MoveBrickParentPosition(brickHeight, brickOffsetY);
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
