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

        public void SetBrickDimensions()
        {
            float brickWidth = BrickController.BrickModel.BrickWidth;
            float brickHeight = BrickController.BrickModel.BrickHeight;

            transform.localScale = new Vector3(brickWidth, brickHeight, 1f);
        }

        public void SetBrickActive(bool val)
        {
            gameObject.SetActive(val);
        }

        public void Break()
        {
            BrickController.BrickModel.BrickValue--;
            if (BrickController.BrickModel.BrickValue <= 0)
            {
                // set brick inactive  and return to the brick pool
                BrickController.ReturnBrick?.Invoke(BrickController);
            }
            SetBrickValue();
        }

        public void SetPosition(Vector2 position)
        {
            transform.localPosition = new Vector3(position.x, position.y, transform.position.z);
            //transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}
