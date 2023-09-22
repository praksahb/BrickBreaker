using UnityEngine;

namespace BrickBreaker.Ball
{

    public class BallView : MonoBehaviour
    {
        public BallController BallController { get; set; }

        public Rigidbody2D Rigidbody2D { get; private set; }

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetBallActive(bool val)
        {
            gameObject.SetActive(val);
        }

        public void SetLaunchBall(Transform FirePoint)
        {
            transform.SetPositionAndRotation(FirePoint.position, FirePoint.rotation);
            Vector3 speed = BallController.BallModel.BallSpeed * transform.up;
            Rigidbody2D.velocity = speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // handle interaction for when the ball will collide with another collider, like bricks or the wall.
            //alternatively handle collision logic in Bricks monobehaviour which might lead to lesser operations overall
            Debug.Log("Collision detected.");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Tic");
        }
    }
}
