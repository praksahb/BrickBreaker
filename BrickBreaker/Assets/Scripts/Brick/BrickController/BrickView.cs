using BallBreaker;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickView : MonoBehaviour, IBrickBreak
    {
        public BrickController BrickController { get; set; }

        private BrickValueCounter brickValueCounter; // ui script for changing value of brick

        private void Awake()
        {
            brickValueCounter = GetComponentInChildren<BrickValueCounter>();
        }

        public void SetBrickValue(int value)
        {
            brickValueCounter.SetValue(value);
        }

        public void SetBrickSize(float brickWidth, float brickHeight)
        {
            transform.localScale = new Vector3(brickWidth, brickHeight, 1f);
        }

        public void SetBrickActive(bool val)
        {
            gameObject.SetActive(val);
        }

        public void Break()
        {
            BrickController.ReduceBrickValue();
        }

        public void SetPosition(Vector2 position)
        {
            transform.localPosition = new Vector3(position.x, position.y, transform.position.z);
        }

        public void SetWorldPosition(float positionX, float positionY)
        {
            transform.position = new Vector3(positionX, positionY, transform.position.z);
        }
    }
}
