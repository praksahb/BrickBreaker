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

        private List<BrickController> usedBrickList;
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

            usedBrickList = new List<BrickController>();

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

            usedBrickList = new List<BrickController>();

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
            int length = usedBrickList.Count;
            for (int i = 0; i < length; i++)
            {
                BrickController brick = usedBrickList[i];
                brick.ReturnBrick?.Invoke(brick);
            }
            usedBrickList.Clear();
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
                    brick.BrickModel.BrickValue = currentBrickVal;
                    brick.BrickView.SetBrickValue(brick.BrickModel.BrickValue);

                    // Get position from gridPosition 
                    Vector2 brickPos = gridPosition[row, col];
                    // changes local positions inside brick pool parent obj
                    brick.BrickView.SetPosition(brickPos);
                    brickGrid[row, col] = brick;
                    usedBrickList.Add(brick);
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
                brick.BrickModel.BrickValue = currentBrickVal;
                brick.BrickView.SetBrickValue(brick.BrickModel.BrickValue);
                brick.BrickView.SetWorldPosition(xPos, yPos);
                usedBrickList.Add(brick);
            }
        }

        // Level 2

        // generate random initial states of brick in grid

        private void RandomArrangement()
        {
            RandomizeInitialState();

            UpdateBrickState();

            UpdateBrickValue();
        }

        private void RandomizeInitialState()
        {
            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxColumns; col++)
                {
                    BrickController brick = brickManager.GetBrick();

                    // Get position from gridPosition 
                    Vector2 brickPos = gridPosition[row, col];
                    // Set position of brick
                    brick.BrickView.SetPosition(brickPos);
                    brickGrid[row, col] = brick; // Add brick to grid
                    usedBrickList.Add(brick);   // Add brick to usedBricks

                    // set nextState value and then update all bricks
                    brick.BrickModel.NextState = CalculateBrickState(row, col);
                }
            }
        }

        // Conway's game of life - 3 Rules
        public void ImplementGOL()
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

                    //// Rule 2 & 3
                    //if (brick.BrickModel.CurrentState == BrickState.Active)
                    //{
                    //    if (brick.BrickModel.BrickValue < 2 || brick.BrickModel.BrickValue > 3)
                    //    {
                    //        brick.BrickModel.NextState = BrickState.Inactive;
                    //    }
                    //    else
                    //    {
                    //        brick.BrickModel.NextState = BrickState.Active;
                    //    }
                    //}

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

            UpdateBrickState();

            UpdateBrickValue();
        }

        private void UpdateBrickState()
        {
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

        private int RandomValue()
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand > 50)
            {
                return 2;
            }
            return 1;
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
                    // Cell - brick
                    BrickController brick = brickGrid[row, col];

                    // updating brickValue for each cell/brick
                    // 2. can check active status of neighbours
                    //Get count of neighbours
                    int count = NeighbourCount(row, col);
                    brick.BrickModel.BrickValue = count;
                    brick.BrickView.SetBrickValue(count);

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
                    // BrickController neighbourBrick = brickGrid[neighbour_row, neighbour_col];
                    // do something with neighbour brick

                    if (brickGrid[neighbour_row, neighbour_col].BrickModel.CurrentState == BrickState.Active)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        // activate or deactivate a brick at a specific row and column
        private void SetBrickActive(int row, int col)
        {
            if (IsValidCell(row, col))
            {
                BrickController brick = brickGrid[row, col];
                if (brick != null)
                {
                    brick.BrickModel.UpdateCurrentState();
                    bool isActive = brick.BrickModel.CurrentState == BrickState.Active ? true : false;
                    brick.BrickView.SetBrickActive(isActive);
                }
            }
        }
    }
}