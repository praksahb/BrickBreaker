using System;
using UnityEngine;

namespace BrickBreaker.Ball
{
    public class BallController
    {
        public BallModel BallModel { get; }
        public BallView BallView { get; }

        public Action<BallController> ReturnBall;

        public BallController(BallModel ballModel, BallView ballView, Transform parent, Transform firePoint)
        {
            BallModel = ballModel;
            BallView = UnityEngine.Object.Instantiate(ballView, parent, true);
            BallView.BallController = this;
            BallView.ResetBall(firePoint);
            BallView.SetBallActive(false);
        }
    }
}
