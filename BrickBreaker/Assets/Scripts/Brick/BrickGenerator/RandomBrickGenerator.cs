using UnityEngine;

namespace BrickBreaker.Bricks

{
    public class RandomBrickGenerator : MonoBehaviour
    {
        [SerializeField] private int desiredRows;
        [SerializeField] private int desiredColumns;
        [SerializeField] private int brickVal;

        private BrickManager brickManager;
        float brickWidth;
        float brickHeight;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
        }

        private void Start()
        {
            DefineGridCustom();
            brickManager.InitializeCustomPool(brickVal, brickWidth, brickHeight);
            brickManager.InitializeGrid();
        }

        // Method 2. creating brick of fixed sizes from the desired rows and column values.
        // and feeding it to the brick initialization function to create the bricks of the specific sizes.

        private void DefineGridCustom() // from row, col values.
        {
            // Get brick sizes
            brickManager.FindGridArea(out float totalWidth, out float totalHeight);
            brickWidth = totalWidth / desiredColumns;
            brickHeight = totalHeight / desiredRows;
            Debug.Log("brickWidth: " + brickWidth);
            Debug.Log("brickHeight: " + brickHeight);

            brickManager.SetGridSize(desiredRows, desiredColumns);

            // Get starting point for parent
            BrickSize brick = new BrickSize(brickWidth, brickHeight);
            brickManager.CalculateStartingPoint(brick);

            // plot grid
            brickManager.SetupGridPositions(brickWidth, brickHeight);
        }
    }
}
