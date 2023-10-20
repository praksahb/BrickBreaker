using System;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BaseBrickGrid
    {
        // required 2d arrays for storing bricks at specific position
        private Vector2[,] gridPosition;
        private BrickController[,] brickGrid;

        protected int maxRows;
        protected int maxColumns;
        protected int activeBricks;

        // 1. traverse position grid
        protected void InitializePositionGrid(BrickLayout brickLayout)
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    SetBrickPosition(brickLayout, row, col);
                }
            }
        }
        private void SetBrickPosition(BrickLayout brickLayout, int row, int col)
        {
            // Calculate the position based on row and column indices
            float xPosition = col * (brickLayout.brickWidth + brickLayout.brickOffsetX);
            float yPosition = -row * (brickLayout.brickHeight + brickLayout.brickOffsetY);
            gridPosition[row, col] = new Vector2(xPosition, yPosition);
        }

        // 2.1 Brick Grid initialization using Grid Traversal
        protected void InitializeBrickGrid(BrickManager brickManager, Action<BrickController, int, int> initializationFunc)
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickManager.GetBrick();
                    SetBrick(brick, row, col);

                    initializationFunc(brick, row, col);
                }
            }
        }

        private void SetBrick(BrickController brick, int row, int col)
        {
            // get a brick, set its position, add to brickGrid
            brick.SetPositionLocal(gridPosition[row, col]);
            brickGrid[row, col] = brick;
        }

        // 2.2 Grid Traversal (brickGrid)
        protected void BrickGridTraversal(Action<BrickController, int, int> BrickOperation)
        {
            // reset active bricks
            activeBricks = 0;

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickGrid[row, col];
                    BrickOperation(brick, row, col);
                }
            }
        }

        // 3 Reset Brick Grid
        // public as it will be called from outside of the class

        public virtual void ResetBrickGrid()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickGrid[row, col];
                    brick.ReturnBrick?.Invoke(brick);
                }
            }
        }


        // base class constructor
        public BaseBrickGrid(int maxRows, int maxColumns)
        {
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
            gridPosition = new Vector2[maxRows, maxColumns];
            brickGrid = new BrickController[maxRows, maxColumns];
        }

        // 2 brickGrid initialization
        public virtual void InitializeBricks(BrickManager brickManager)
        {
            // functionality to be  added in derived class
        }

        // cant make BaseBrickGrid abstract because of these two differences
        // used in Level 1 only
        public virtual void AddBrickRow(Vector2 startPosition) { }

        // used in Level 2 only
        public virtual bool GameOverCondition()
        {
            return false;
        }

        // tester function used in level 2
        public virtual void SetVal(float v1, float v2) { }

        public void DecrementActiveBricks()
        {
            activeBricks--;
        }
    }
}
