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
        public int brickBreakValue;
    }
}
