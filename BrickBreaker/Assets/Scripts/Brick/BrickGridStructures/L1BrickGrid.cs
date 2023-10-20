using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class L1BrickGrid : BaseBrickGrid
    {
        private List<BrickController> usedBricks;
        private BrickLayout brickLayout;
        private BrickManager brickManager;
        private int brickValIncrement;

        private void SetupBrick(BrickController brick, int row, int col)
        {
            brick.UpdateBrickValue(base.maxRows - row);
            usedBricks.Add(brick);
        }

        // custom constructor for Level 1 bricks generation
        public L1BrickGrid(BrickManager brickManager, int maxRows, int maxColumns, BrickLayout brickLayout) : base(maxRows, maxColumns)
        {
            this.brickManager = brickManager;
            this.brickLayout = brickLayout;
            usedBricks = new List<BrickController>();

            InitializePositionGrid(brickLayout);
            InitializeBricks(brickManager);
        }

        // initializes the brick grid
        public override void InitializeBricks(BrickManager brickManager)
        {
            brickValIncrement = base.maxRows;
            InitializeBrickGrid(brickManager, SetupBrick);
        }

        // add a row of bricks at the top - working with world space values
        public override void AddBrickRow(Vector2 startPos)
        {
            brickValIncrement++;
            for (int i = 0; i < base.maxColumns; i++)
            {
                float xPos = startPos.x + i * (brickLayout.brickWidth + brickLayout.brickOffsetX);
                float yPos = startPos.y;

                BrickController brick = brickManager.GetBrick();
                brick.UpdateBrickValue(brickValIncrement);
                brick.SetPositionWorld(new Vector2(xPos, yPos));
                usedBricks.Add(brick);
            }
        }

        // reset grid - return brick function for each individual brick
        public override void ResetBrickGrid()
        {
            int length = usedBricks.Count;
            for (int i = 0; i < length; i++)
            {
                BrickController brick = usedBricks[i];
                brick.ReturnBrick?.Invoke(brick);

                //usedBricks[i].ReturnBrick?.Invoke(usedBricks[i]);

            }
            usedBricks.Clear();
        }
    }
}
