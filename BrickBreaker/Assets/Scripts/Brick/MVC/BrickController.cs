using System;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickController
    {
        public BrickModel BrickModel { get; set; }
        public BrickView BrickView { get; set; }

        public Action<BrickController> ReturnBrick;

        public BrickController(BrickModel brickModel, BrickView brickPrefab, Transform parentObj)
        {
            BrickModel = brickModel;
            BrickModel.BreakBrick += RemoveBrick;
            BrickView = UnityEngine.Object.Instantiate(brickPrefab, parentObj, false);
            BrickView.BrickController = this;
            BrickView.SetBrickSize(BrickModel.BrickWidth, BrickModel.BrickHeight);
            BrickView.SetBrickActive(false);
        }

        private void RemoveBrick()
        {
            ReturnBrick?.Invoke(this);
        }

        public void ReduceBrickValue()
        {
            BrickModel.BrickValue--;
            BrickView.SetBrickValue(BrickModel.BrickValue);
        }
    }
}
