using UnityEngine;
using UnityEngine.UI;

namespace BrickBreaker.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsBtn;

        public UIManager UIManager { get; set; }

        private void OnEnable()
        {
            playButton.onClick.AddListener(PlayGame);
            optionsBtn.onClick.AddListener(SelectOptions);
        }
        private void OnDisable()
        {
            playButton.onClick.RemoveListener(PlayGame);
            optionsBtn.onClick.RemoveAllListeners();
        }

        private void PlayGame()
        {
            UIManager.SelectMenu?.Invoke(MenuType.PlayLevel);
        }

        private void SelectOptions()
        {
            UIManager.SelectMenu?.Invoke(MenuType.SelectOptions);
        }
    }
}
