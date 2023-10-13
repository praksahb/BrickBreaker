using System.Collections.Generic;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickGrid
    {
        private BrickController[,] brickGrid; // 2D array stores all the bricks which can populate the grid box
        private Vector2[,] gridPosition; // 2D array storing position values for the bricks

        private BrickManager brickManager;

        private int maxRows;
        private int maxColumns;
        private BrickLayout brickLayout; // brick dimensions / brick configuration

        private int totalBricks;
        private int activeBricks;
        private List<BrickController> usedBricks;
        private PlayLevel currentLevel;
        private int currentBrickVal;

        private float scaleValue;
        private float threshold;


        private readonly int[] neighbourX = { 0, -1, -1, -1, 0, 1, 1, 1 };
        private readonly int[] neighbourY = { -1, -1, 0, 1, 1, 1, 0, -1 };

        // tester function
        public void SetVal(float v1, float v2)
        {
            scaleValue = v1;
            threshold = v2;
        }

        // ctor for level 1
        public BrickGrid(BrickManager brickManager, int maxRows, int maxColumns, BrickLayout brick, PlayLevel currentLevel)
        {
            this.brickManager = brickManager;
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
            this.brickLayout = brick;
            this.currentLevel = currentLevel;

            usedBricks = new List<BrickController>();

            SetupGridPositions();

            InitializeGrid();
        }

        // ctor for level 2
        public BrickGrid(BrickManager brickManager, int maxRows, int maxColumns, BrickLayout brick, PlayLevel currentLevel, float scaleValue, float threshold)
        {
            this.brickManager = brickManager;
            this.maxRows = maxRows;
            this.maxColumns = maxColumns;
            this.brickLayout = brick;
            this.currentLevel = currentLevel;
            this.scaleValue = scaleValue;
            this.threshold = threshold;
            totalBricks = maxRows * maxColumns;
            activeBricks = 0;
            usedBricks = new List<BrickController>();

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
                    float xPosition = col * (brickLayout.brickWidth + brickLayout.brickOffsetX);
                    float yPosition = -row * (brickLayout.brickHeight + brickLayout.brickOffsetY);
                    gridPosition[row, col] = new Vector2(xPosition, yPosition);
                }
            }
        }

        // Populate the array with bricks in a basic rectangular shape
        public void InitializeGrid()
        {
            brickGrid = new BrickController[maxRows, maxColumns];

            if (currentLevel == PlayLevel.Classic)
            {
                NormalArrangement();
            }
            if (currentLevel == PlayLevel.NewType)
            {
                // randomize brick arrangement
                RandomArrangement();
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
            }
            usedBricks.Clear();
        }

        // Level 1

        private void NormalArrangement()
        {
            currentBrickVal = maxRows;
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickManager.GetBrick();

                    // set brickVal to row no.
                    brick.UpdateBrickValue(currentBrickVal);

                    // Get position from gridPosition and set it to the brick
                    brick.SetPositionLocal(gridPosition[row, col]);
                    // add brick to brickGrid and usedBrickList
                    brickGrid[row, col] = brick;
                    usedBricks.Add(brick);
                }
                currentBrickVal--;
            }
            currentBrickVal = maxRows;
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

        // Level 2

        // generate random initial states of brick in grid

        private void RandomArrangement()
        {
            RandomizedStart();
            // first update currentState = nextState, and set brickView active based on currentState
            UpdateBrickState();
            // update value of bricks according to neighbour's active
            UpdateBrickValue();
            Debug.Log("active_bricks: " + activeBricks);
            Debug.Log("totalbricks: " + totalBricks);
        }

        public void RandomizeAfterTurn()
        {
            RegenerateBricks();
            UpdateBrickState();
            UpdateBrickValue();
            Debug.Log("active_bricks: " + activeBricks);
            Debug.Log("totalbricks: " + totalBricks);
        }

        // game over condition
        public bool GameOverCondition()
        {
            return activeBricks >= totalBricks;
        }

        private void RandomizedStart()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickManager.GetBrick();

                    // Set position of brick
                    brick.SetPositionLocal(gridPosition[row, col]);
                    brickGrid[row, col] = brick; // Add brick to grid
                    usedBricks.Add(brick);   // Add brick to usedBricks

                    // set nextState value and then update all bricks
                    brick.BrickModel.NextState = CalculateBrickState(row, col);
                }
            }
        }


        // too rapid growth
        private void RegenerateBricks()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController curr_brick = brickGrid[row, col];

                    if (curr_brick.BrickModel.CurrentState == BrickState.Active)
                    {
                        ActivateNeighbours(row, col);
                    }
                }
            }
        }

        private void RegenerateBricks2()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController curr_brick = brickGrid[row, col];

                    if (curr_brick.BrickModel.CurrentState == BrickState.Active)
                    {
                        activeBricks++;
                        curr_brick.BrickModel.NextState = BrickState.Active;
                    }
                    else if (curr_brick.BrickModel.BrickValue > 2)
                    {
                        curr_brick.BrickModel.NextState = BrickState.Active;
                    }
                }
            }
        }

        // cant use this in game
        // Conway's game of life - 3 Rules
        private void ImplementGOL()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickGrid[row, col];

                    //// Rule 1
                    if (brick.BrickModel.CurrentState != BrickState.Active && brick.BrickModel.BrickValue == 3)
                    {
                        brick.BrickModel.NextState = BrickState.Active;
                    }

                    // Rule 2
                    else if (brick.BrickModel.CurrentState == BrickState.Active)
                    {
                        if (brick.BrickModel.BrickValue == 2 || brick.BrickModel.BrickValue == 3)
                        {
                            brick.BrickModel.NextState = BrickState.Active;
                        }
                        else
                        {
                            brick.BrickModel.NextState = BrickState.Inactive;
                        }
                    }

                    // Rule 3
                    else
                    {
                        brick.BrickModel.NextState = BrickState.Inactive;
                    }
                }
            }
        }

        private void UpdateBrickState()
        {
            activeBricks = 0;
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    SetBrickActive(row, col);
                }
            }
        }

        // uses perlin noise for generating random values
        private BrickState CalculateBrickState(int row, int col)
        {
            float perlinValue = Mathf.PerlinNoise(row * scaleValue, col * scaleValue);

            return perlinValue > threshold ? BrickState.Active : BrickState.Inactive;
        }

        // check if valid row, col value
        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < maxRows && col >= 0 && col < maxColumns;
        }

        // traverse grid each cells  -
        // - 1. Initialize BrickValues according to count of neighbours
        private void UpdateBrickValue()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    int count = NeighbourCount(row, col);
                    //if (count == 0) count++;
                    brickGrid[row, col].UpdateBrickValue(count);

                }
            }
        }

        private void ActivateNeighbours(int row, int col)
        {
            int len = neighbourX.Length;
            for (int i = 0; i < len; i++)
            {
                int n_row = row + neighbourX[i];
                int n_col = col + neighbourY[i];
                if (IsValidCell(n_row, n_col))
                {
                    brickGrid[n_row, n_col].BrickModel.NextState = BrickState.Active;
                }
            }
        }

        private int NeighbourCount(int row, int col)
        {
            int len = neighbourX.Length;
            int count = 0;
            for (int i = 0; i < len; i++)
            {
                // get neighbour cell index
                int neighbour_row = row + neighbourX[i];
                int neighbour_col = col + neighbourY[i];
                if (IsValidCell(neighbour_row, neighbour_col))
                {
                    if (brickGrid[neighbour_row, neighbour_col].BrickModel.CurrentState == BrickState.Active)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        // updates currentState to be nextState in brickModel and 
        // according to currentState value of brick
        // activate or deactivate a brick at a specific row and column
        private void SetBrickActive(int row, int col)
        {
            if (IsValidCell(row, col))
            {
                brickGrid[row, col].ActiveByCurrState();
                if (brickGrid[row, col].BrickModel.CurrentState == BrickState.Active)
                {
                    activeBricks++;
                }
            }
        }
    }
}