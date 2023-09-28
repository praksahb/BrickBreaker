using System;

namespace BrickBreaker.Bricks
{
    public class BrickController
    {
        public BrickModel BrickModel { get; set; }
        public BrickView BrickView { get; set; }

        public Action<BrickController> ReturnBrick;

        public BrickController(BrickModel brickModel, BrickView brickPrefab)
        {
            BrickModel = brickModel;
            BrickView = UnityEngine.Object.Instantiate(brickPrefab);
            BrickView.BrickController = this;
            BrickView.SetBrickDimensions();
            BrickView.SetBrickActive(false);
        }
    }
}
