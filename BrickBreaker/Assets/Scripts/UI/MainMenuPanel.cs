using UnityEngine;
using UnityEngine.UI;

namespace BallBreaker
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button startGame;

        public UIManager UIManager { get; set; }


        private void OnEnable()
        {
            startGame.onClick.AddListener(LaunchGame);
        }
        private void OnDisable()
        {
            startGame.onClick.RemoveListener(LaunchGame);
        }

        private void LaunchGame()
        {
            UIManager.StartGame?.Invoke();
        }
    }
}
