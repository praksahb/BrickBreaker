using UnityEngine;
using UnityEngine.UI;

namespace BallBreaker.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button playButton;

        public UIManager UIManager { get; set; }

        private void OnEnable()
        {
            playButton.onClick.AddListener(PlayGame);
        }
        private void OnDisable()
        {
            playButton.onClick.RemoveListener(PlayGame);
        }

        private void PlayGame()
        {
            UIManager.LaunchPlayMenu?.Invoke();
        }
    }
}
