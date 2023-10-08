using UnityEngine;

namespace BrickBreaker.Bricks
{
    [CreateAssetMenu(fileName = "BrickTypeSO", menuName = "ScriptableObjects/BrickSO")]
    public class BrickSO : ScriptableObject
    {
        public Bricks[] allBricks;
    }

    [System.Serializable]
    public struct Bricks
    {
        public BrickView brickPrefab;
        public string brickName;
        public float brickWidth;
        public float brickHeight;

        // ctor used in brickManager for custom brick sizes
        public Bricks(string brickName, BrickView brickPrefab, float brickWidth, float brickHeight)
        {
            this.brickName = brickName;
            this.brickPrefab = brickPrefab;
            this.brickWidth = brickWidth;
            this.brickHeight = brickHeight;
        }
    }
}
