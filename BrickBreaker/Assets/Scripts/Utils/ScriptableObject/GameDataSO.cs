using UnityEngine;

namespace BrickBreaker.Serv
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameDataSO")]
    public class GameDataSO : ScriptableObject
    {
        public int ballPoolSize;
        public int ballSpeed;
    }
}
