using BallBreaker;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickView : MonoBehaviour, IBrickBreak
    {
        // renderer refs
        [SerializeField] private SpriteRenderer brickRenderer;
        [SerializeField] private BoxCollider2D boxCollider;
        private MeshRenderer brickValRenderer;


        public BrickController BrickController { get; set; }

        private BrickValueCounter brickValueCounter; // ui script for changing value of brick


        private void Awake()
        {
            brickValueCounter = GetComponentInChildren<BrickValueCounter>();
            if (brickValueCounter != null)
            {
                brickValRenderer = brickValueCounter.GetComponent<MeshRenderer>();
            }
        }

        public void SetBrickValue(int value)
        {
            if (brickValueCounter != null)
            {
                brickValueCounter.SetValue(value);
            }
        }

        public void SetBrickSize(float brickWidth, float brickHeight)
        {
            transform.localScale = new Vector3(brickWidth, brickHeight, 1f);
        }

        public void SetBrickActive(bool val)
        {
            brickRenderer.enabled = val;
            if (brickValueCounter != null)
            {
                brickValRenderer.enabled = val;
            }
            boxCollider.enabled = val;

            //gameObject.SetActive(val);
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BrickController.GameManager.GameOver?.Invoke();
        }
    }
}
