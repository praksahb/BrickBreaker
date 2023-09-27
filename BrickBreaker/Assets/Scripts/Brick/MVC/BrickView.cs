using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickView : MonoBehaviour
    {
        public BrickController BrickController { get; set; }

        private BrickValueCounter brickValueCounter;

        private void Awake()
        {
            brickValueCounter = GetComponentInChildren<BrickValueCounter>();
        }

        public void SetBrickValue()
        {
            brickValueCounter.SetValue(BrickController.BrickModel.BrickValue);
        }

        public void SetBrickActive(bool val)
        {
            gameObject.SetActive(val);
        }
    }
}
