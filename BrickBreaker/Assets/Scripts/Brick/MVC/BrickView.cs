using BallBreaker;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickView : MonoBehaviour, IBrickBreak
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

        public void Break()
        {
            BrickController.BrickModel.BrickValue--;
            if (BrickController.BrickModel.BrickValue == 0)
            {
                // set brick inactive  and return to the brick pool
                BrickController.ReturnBrick?.Invoke(BrickController);
            }
            SetBrickValue();
        }
    }
}
