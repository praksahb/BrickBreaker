using System;
using UnityEngine;

namespace BrickBreaker.Ball
{
    public class BallController
    {
        public BallModel BallModel { get; }
        public BallView BallView { get; }

        public Action<BallController> ReturnBall;

        public BallController(BallModel ballModel, BallView ballView, Transform firePoint)
        {
            BallModel = ballModel;
            BallView = UnityEngine.Object.Instantiate(ballView, firePoint, true);
            BallView.BallController = this;
            BallView.FirePoint = firePoint;
            BallView.ResetBall();
            BallView.SetBallActive(false);
        }
    }
}
