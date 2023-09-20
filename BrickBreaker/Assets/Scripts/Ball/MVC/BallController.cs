using UnityEngine;

namespace BrickBreaker.Ball
{
    public class BallController
    {
        public BallModel BallModel { get; }
        public BallView BallView { get; }

        public BallController(BallModel ballModel, BallView ballView)
        {
            BallModel = ballModel;
            BallView = Object.Instantiate(ballView);
        }
    }
}
