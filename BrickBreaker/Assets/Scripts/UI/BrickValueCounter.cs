using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickValueCounter : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshPro brickValue;

        public void SetValue(int value)
        {
            brickValue.text = value.ToString();
        }
    }
}
