using System;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BaseBrickGrid
    {
        // required 2d arrays for storing bricks at specific position
        private Vector2[,] gridPosition;
        private BrickController[,] brickGrid;

        private int maxRows;
        private int maxColumns;

        public BaseBrickGrid(int maxRows, int maxColumns)
        {
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
            gridPosition = new Vector2[maxRows, maxColumns];
            brickGrid = new BrickController[maxRows, maxColumns];
        }

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

        // 2.1 brickTraversals
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
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickGrid[row, col];
                    BrickOperation(brick, row, col);
                }
            }
        }
    }
}
