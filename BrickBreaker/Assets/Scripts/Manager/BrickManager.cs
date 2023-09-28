using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickSO brickSO;
        [SerializeField] private int totalBricks;

        [SerializeField] private int rows = 5;
        [SerializeField] private int columns = 10;

        private BrickServicePool brickPool;

        private BrickController[,] brickGrid;

        private void Start()
        {
            InitializeBrickPool();
            SetupBrickGrid();
        }

        private void InitializeBrickPool()
        {
            // Currently only one type of brick, and the generic Pooling is also modelled in a way to accomodate only a single type of object.
            Bricks brickType = brickSO.allBricks[0];
            brickPool = new BrickServicePool(totalBricks, brickType);
        }

        void SetupBrickGrid()
        {
            Vector3 topLeftCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)); // Calculate the top-left corner position
            int offset = 1;

            // Initialize the 2D array
            brickGrid = new BrickController[rows, columns];

            // Populate the array with bricks in a basic rectangular shape
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Create a new brick
                    BrickController brick = brickPool.GetBrick();
                    brick.ReturnBrick += ReturnBrick;

                    // Calculate the position based on row and column indices
                    float xPosition = topLeftCorner.x + 2 * offset + col * brick.BrickModel.BrickWidth;
                    float yPosition = (topLeftCorner.y - offset) - row * brick.BrickModel.BrickHeight;

                    // Set the brick's position and assign it to the array
                    brick.BrickView.SetPosition(new Vector2(xPosition, yPosition));
                    brickGrid[row, col] = brick;
                }
            }
        }

        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;
            brickPool.ReturnBrick(brick);
        }
    }
}
