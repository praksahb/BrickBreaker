using BrickBreaker.Services;
using System;
using UnityEngine;

namespace BrickBreaker.Bricks
{
    public class BrickController
    {
        public BrickModel BrickModel { get; set; }
        public BrickView BrickView { get; set; }

        public GameManager GameManager { get; set; }
        public Action<BrickController> ReturnBrick;

        public BrickController(BrickModel brickModel, BrickView brickPrefab, Transform parentObj, GameManager gameManager)
        {
            BrickModel = brickModel;
            BrickView = UnityEngine.Object.Instantiate(brickPrefab, parentObj, false);
            BrickView.BrickController = this;
            BrickView.SetBrickSize(BrickModel.BrickWidth, BrickModel.BrickHeight);
            BrickView.SetBrickActive(false);
            GameManager = gameManager;
        }

        public void ReduceBrickValue()
        {
            BrickModel.BrickValue--;
            if (BrickModel.BrickValue <= 0)
            {
                ReturnBrick?.Invoke(this);
            }
            BrickView.SetBrickValue(BrickModel.BrickValue);
        }
    }
}
