namespace BrickBreaker.Bricks
{
    public enum BrickState
    {
        None = 0,
        Active,
        Inactive
    }
}

namespace BrickBreaker
{
    public enum PlayLevel
    {
        Menu = 0,
        Classic = 1,
        NewType = 2,
    }
}

namespace BrickBreaker.UI
{
    public enum MenuType
    {
        MainMenu,
        PlayLevel,
        SelectOptions,
        Exit,
    }
}
