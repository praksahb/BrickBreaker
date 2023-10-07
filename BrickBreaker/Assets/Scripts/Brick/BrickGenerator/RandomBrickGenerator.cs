using UnityEngine;

namespace BrickBreaker.Bricks

{
    public class RandomBrickGenerator : MonoBehaviour
    {
        [SerializeField] private int desiredRows;
        [SerializeField] private int desiredColumns;
        [SerializeField] private int brickVal;

        private BrickManager brickManager;
        private float brickWidth;
        private float brickHeight;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
        }

        private void Start()
        {
            DefineGridCustom();
            brickManager.CheckGridWorks();
        }

        // Method 2. creating brick of fixed sizes from the desired rows and column values.
        // and feeding it to the brick initialization function to create the bricks of the specific sizes.

        private void DefineGridCustom() // from row, col values.
        {
            // Get brick sizes
            brickManager.FindGridArea(out float totalWidth, out float totalHeight);
            brickWidth = totalWidth / desiredColumns;
            brickHeight = totalHeight / desiredRows;

            //brickManager.SetGridSize(desiredRows, desiredColumns);

            // Get starting point for parent
            BrickSize brick = new BrickSize(brickWidth, brickHeight);
            brickManager.SetParentPosition(brick);

            // setup grid
            brickManager.InitializeBrickGrid(brick, desiredRows, desiredColumns, brickVal);
        }
    }
}
