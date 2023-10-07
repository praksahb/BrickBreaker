﻿using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickGrid
    {
        private BrickController[,] brickGrid; // 2D array stores all the bricks which can populate the grid box
        private Vector2[,] gridPosition; // 2D array storing position values for the bricks

        private BrickManager brickManager;

        private int maxRows;
        private int maxColumns;
        private BrickSize brick;

        public BrickGrid(BrickManager brickManager, int maxRows, int maxColumns, BrickSize brick)
        {
            this.brickManager = brickManager;
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
            this.brick = brick;

            SetupGridPositions();
            InitializeGrid();
        }

        private void SetupGridPositions()
        {
            gridPosition = new Vector2[maxRows, maxColumns];

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    // Calculate the position based on row and column indices
                    float xPosition = col * (brick.brickWidth + brick.brickOffsetX);
                    float yPosition = -row * (brick.brickHeight + brick.brickOffsetY);
                    gridPosition[row, col] = new Vector2(xPosition, yPosition);
                }
            }
        }

        // Populate the array with bricks in a basic rectangular shape
        private void InitializeGrid()
        {
            brickGrid = new BrickController[maxRows, maxColumns];

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickManager.GetBrick();

                    // Get position from gridPosition 
                    float xPosition = gridPosition[row, col].x;
                    float yPosition = gridPosition[row, col].y;

                    brick.BrickView.SetPosition(new Vector2(xPosition, yPosition));
                    brickGrid[row, col] = brick;
                }
            }
        }

        // check if valid row, col value
        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < maxRows && col >= 0 && col < maxColumns;
        }

        // traverse grid each cells
        public void GridTraversal()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    // Cell - brick
                    BrickController brick = brickGrid[row, col];

                    // 1. can check if brick is active or not, by storing bool value in its model

                    // 2. can check active status of neighbours


                }
            }
        }

        private int[] neighbourX = { 0, -1, -1, -1, 0, 1, 1, 1 };
        private int[] neighbourY = { -1, -1, 0, 1, 1, 1, 0, -1 };

        public void CheckNeighbours(int row, int col)
        {
            int len = neighbourX.Length;
            for (int i = 0; i < len; i++)
            {
                // get neighbour cell index
                int neighbour_row = row + neighbourX[i];
                int neighbour_col = col + neighbourY[i];
                if (IsValidCell(neighbour_row, neighbour_col))
                {
                    BrickController neighbourBrick = brickGrid[neighbour_row, neighbour_col];
                    // do something with neighbour brick

                }
            }
        }

        // activate or deactivate a brick at a specific row and column
        public void SetBrickActive(int row, int col, bool isActive)
        {
            if (IsValidCell(row, col))
            {
                BrickController brick = brickGrid[row, col];
                if (brick != null)
                {
                    brick.BrickView.SetBrickActive(isActive);
                }
            }
        }
    }
}