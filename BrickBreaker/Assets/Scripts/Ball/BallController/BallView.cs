using BallBreaker;
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
            ResetBall();
        }

        public void LaunchBall(Vector2 launchPos)
        {
            Vector3 velocity = BallController.BallModel.BallSpeed * launchPos;
            SetVelocity(velocity);
        }

        private void ResetBall()
        {
            transform.localPosition = Vector2.zero;
            if (Rigidbody2D) SetVelocity(Vector3.zero);
        }

        private void SetVelocity(Vector3 velocity)
        {
            Rigidbody2D.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // handle interaction for when the ball will collide with another collider, like bricks or the wall. using Vector reflect
            //alternatively handle collision logic in Bricks monobehaviour which might lead to lesser operations overall

            if (collision.gameObject.TryGetComponent<IBrickBreak>(out IBrickBreak brick))
            {
                brick.Break();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // raise event in game manager to return the ball
            BallController.ReturnBall?.Invoke(BallController);
        }
    }
}
