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

        public void SetLaunchBall(Vector3 dir)
        {

            Vector3 velocity = BallController.BallModel.BallSpeed * dir;
            SetVelocity(velocity);
        }

        public void ResetBall(Transform FirePoint)
        {
            transform.SetPositionAndRotation(FirePoint.position, FirePoint.rotation);

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
            Debug.Log("Collision detected.");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Tic");

            // raise event in game manager to return the ball
            BallController.ReturnBall?.Invoke(BallController);
        }
    }
}
