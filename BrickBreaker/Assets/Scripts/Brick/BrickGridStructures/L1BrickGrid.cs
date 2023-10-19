using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class L1BrickGrid : BaseBrickGrid
    {
        private List<BrickController> usedBricks;
        private BrickLayout brickLayout;
        private BrickManager brickManager;
        private int currentBrickVal;

        private int maxRows;
        private int maxColumns;

        private void SetupBrick(BrickController brick, int row, int col)
        {
            brick.UpdateBrickValue(currentBrickVal - row);
            usedBricks.Add(brick);
        }

        // custom constructor for Level 1 bricks generation
        public L1BrickGrid(BrickManager brickManager, int maxRows, int maxColumns, BrickLayout brickLayout) : base(maxRows, maxColumns)
        {
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
            this.brickManager = brickManager;
            this.brickLayout = brickLayout;
            usedBricks = new List<BrickController>();

            InitializePositionGrid(brickLayout);
            InitializeBricks();
        }

        // initializes the brick grid
        public void InitializeBricks()
        {
            currentBrickVal = maxRows;
            InitializeBrickGrid(brickManager, SetupBrick);
        }

        // add a row of bricks at the top - working with world space values
        public void AddBrickRow(Vector2 startPos)
        {
            currentBrickVal++;
            for (int i = 0; i < maxColumns; i++)
            {
                float xPos = startPos.x + i * (brickLayout.brickWidth + brickLayout.brickOffsetX);
                float yPos = startPos.y;

                BrickController brick = brickManager.GetBrick();
                brick.UpdateBrickValue(currentBrickVal);
                brick.SetPositionWorld(new Vector2(xPos, yPos));
                usedBricks.Add(brick);
            }
        }

        // reset grid - return brick function for each individual brick
        public void ResetBrickGrid()
        {
            int length = usedBricks.Count;
            for (int i = 0; i < length; i++)
            {
                BrickController brick = usedBricks[i];
                brick.ReturnBrick?.Invoke(brick);
                brick.BrickView.SetBrickActive(false);

                //usedBricks[i].ReturnBrick?.Invoke(usedBricks[i]);

            }
            usedBricks.Clear();
        }
    }
}
