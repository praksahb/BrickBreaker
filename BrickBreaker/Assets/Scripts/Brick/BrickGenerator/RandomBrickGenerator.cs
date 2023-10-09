using UnityEngine;

namespace BrickBreaker.Bricks

{
    public class RandomBrickGenerator : MonoBehaviour, IBrickGenerator
    {
        [SerializeField] private int desiredRows;
        [SerializeField] private int desiredColumns;

        private BrickManager brickManager;
        private float brickWidth;
        private float brickHeight;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
        }


        // Method 2. creating brick of fixed sizes from the desired rows and column values.
        // and feeding it to the brick initialization function to create the bricks of the specific sizes.

        public void DefineGrid() // from row, col values.
        {
            // Get brick sizes
            brickManager.FindGridArea(out float totalWidth, out float totalHeight);
            brickWidth = totalWidth / desiredColumns;
            brickHeight = totalHeight / desiredRows;

            //brickManager.SetGridSize(desiredRows, desiredColumns);

            // Get starting point for parent
            BrickLayout brick = new BrickLayout(brickWidth, brickHeight);
            brickManager.SetStartPosition(brick);

            // setup grid
            brickManager.InitializeBrickGrid(brick, desiredRows, desiredColumns);
        }

        public void PerformFunction()
        {
            // To be created, randomize bricks after all balls have returned..
        }
    }
}
