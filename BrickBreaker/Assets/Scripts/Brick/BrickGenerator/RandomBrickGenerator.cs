using UnityEngine;

namespace BrickBreaker.Bricks

{
    public class RandomBrickGenerator : MonoBehaviour, IBrickGenerator
    {
        [SerializeField] private int desiredRows;
        [SerializeField] private int desiredColumns;
        [SerializeField] private PlayLevel currentLevel;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float scaleValue;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float threshold;

        private BrickManager brickManager;
        private float brickWidth;
        private float brickHeight;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                brickManager.ResetGrid(scaleValue, threshold);
            }
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
            brickManager.InitializeBricks(brick, desiredRows, desiredColumns, currentLevel, scaleValue, threshold);
        }

        public void PerformFunction()
        {
            // To be created, randomize bricks after all balls have returned..
        }
    }
}
