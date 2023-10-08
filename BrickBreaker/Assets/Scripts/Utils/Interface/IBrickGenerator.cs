using UnityEngine;

namespace BrickBreaker
{
    public abstract class IBrickGenerator : MonoBehaviour
    {
        public abstract void DefineGrid();
        public abstract void PerformFunction();
    }
}