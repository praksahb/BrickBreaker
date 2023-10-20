using BrickBreaker.Bricks;
using UnityEngine;

namespace BrickBreaker
{
    public class L2BrickGrid : BaseBrickGrid
    {
        private float scaleValue;
        private float threshold;
        private float offsetX;
        private float offsetY;

        private int brickValue; // keeping default of 1 from brickModel's constructor

        private void RandomSeedStart()
        {
            offsetX = Random.Range(0f, 99999f);
            offsetY = Random.Range(0f, 99999f);
        }

        // function passed as delegate to be called for each brick in brickGrid
        private void SetupBricks(BrickController brick, int row, int col)
        {
            brick.BrickModel.NextState = CalculateBrickState(row, col);
        }

        // uses perlin noise for generating random values
        // from brackey's video on procedural generation
        private BrickState CalculateBrickState(int row, int col)
        {
            float ModifiedRow = (float)row / maxRows * scaleValue + offsetX;
            float modifiedCol = (float)col / maxColumns * scaleValue + offsetY;

            float perlinValue = Mathf.PerlinNoise(ModifiedRow, modifiedCol);

            return perlinValue > threshold ? BrickState.Active : BrickState.Inactive;
        }

        // activate or deactivate a brick at a specific row and column
        private void SetBrickActive(BrickController brick, int row, int col)
        {
            if (IsValidCell(row, col))
            {
                if (brick.UpdateBrick()) // sets the brick active or inactive 
                {
                    base.activeBricks++;
                }
            }
        }

        // check if valid row, col value
        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < maxRows && col >= 0 && col < maxColumns;
        }

        // tester functino 
        public override void SetVal(float v1, float v2)
        {
            scaleValue = v1;
            threshold = v2;
        }

        // custom constructor for level 2
        public L2BrickGrid(BrickManager brickManager, int maxRows, int maxColumns, BrickLayout brickLayout, float scaleValue, float threshold) : base(maxRows, maxColumns)
        {
            this.scaleValue = scaleValue;
            this.threshold = threshold;

            InitializePositionGrid(brickLayout);
            InitializeBricks(brickManager);
        }

        public override void InitializeBricks(BrickManager brickManager)
        {
            // set random values for offsetX and offsetY
            RandomSeedStart();
            // intialize brickGrid
            InitializeBrickGrid(brickManager, SetupBricks);
            // update state values for bricks
            BrickGridTraversal(SetBrickActive);
        }

        public override bool GameOverCondition()
        {
            return activeBricks == 0;
        }
    }
}
