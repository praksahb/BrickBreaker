using BrickBreaker.Services;
using UnityEngine;

namespace BrickBreaker
{
    public abstract class IBrickGenerator : MonoBehaviour
    {
        public abstract void DefineGrid(GameManager gameManager);
        public abstract void PerformFunction();
    }
}