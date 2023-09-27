using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField] private BrickSO brickSO;
        [SerializeField] private int totalBricks;

        private BrickServicePool brickPool;

        private void Start()
        {
            InitializeBricks();
            BrickController brick = brickPool.GetBrick();
            brick.ReturnBrick += ReturnBrick;
        }

        private void InitializeBricks()
        {
            BrickView brickView = brickSO.allBricks[0].brickPrefab;
            int brickValue = brickSO.allBricks[0].brickBreakValue;
            brickPool = new BrickServicePool(totalBricks, brickView, brickValue);
        }

        private void ReturnBrick(BrickController brick)
        {
            brick.ReturnBrick -= ReturnBrick;

            brickPool.ReturnBrick(brick);
        }
    }
}
