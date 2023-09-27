namespace BrickBreaker.Bricks
{
    public class BrickController
    {
        public BrickModel BrickModel { get; set; }
        public BrickView BrickView { get; set; }

        public BrickController(BrickModel brickModel, BrickView brickPrefab)
        {
            BrickModel = brickModel;
            BrickView = UnityEngine.Object.Instantiate(brickPrefab);
            BrickView.BrickController = this;
            BrickView.SetBrickActive(false);
        }
    }
}