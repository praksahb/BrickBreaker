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
            BrickView.SetBrickValue(BrickModel.BrickValue);
            BrickView.SetBrickSize(BrickModel.BrickWidth, BrickModel.BrickHeight);
            GameManager = gameManager;
        }

        public void UpdateBrickValue(int value)
        {
            BrickModel.BrickValue = value;
            BrickView.SetBrickValue(BrickModel.BrickValue);
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

        public bool UpdateBrick()
        {
            BrickModel.UpdateCurrentState();
            bool isActive = BrickModel.CurrentState == BrickState.Active ? true : false;
            BrickView.SetBrickActive(isActive);
            return isActive;
        }

        public void SetPositionLocal(Vector2 pos)
        {
            BrickView.SetPosition(pos);
        }

        public void SetPositionWorld(Vector2 pos)
        {
            BrickView.SetWorldPosition(pos.x, pos.y);
        }
    }
}
