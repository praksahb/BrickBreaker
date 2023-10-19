using UnityEngine;

namespace BrickBreaker.Bricks

{
    public class RandomBrickGenerator : MonoBehaviour, IBrickGenerator
    {
        [SerializeField] private int desiredRows;
        [SerializeField] private int desiredColumns;
        [Range(0.0f, 50.0f)]
        [SerializeField] private float scaleValue;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float threshold;
        [SerializeField] private bool activateBrianBrain;
        private BrickManager brickManager;
        private float brickWidth;
        private float brickHeight;

        [SerializeField] private float timer;
        private float timerVal;

        private void Awake()
        {
            brickManager = GetComponentInParent<BrickManager>();
            timerVal = timer;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                brickManager.ResetGrid(scaleValue, threshold);
            }

            if (activateBrianBrain)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    ModifyGrid();
                    timer = timerVal;
                }

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
            BrickLayout brickLayout = new BrickLayout(brickWidth, brickHeight);
            brickManager.SetStartPosition(brickLayout);

            // setup grid
            brickManager.InitializeBricks(brickLayout, desiredRows, desiredColumns, scaleValue, threshold);
        }

        public void ModifyGrid()
        {
            // To be created, randomize bricks after all balls have returned..
            //brickManager.RegenerateBricks();

            brickManager.CheckWinCondition();
        }
    }
}
