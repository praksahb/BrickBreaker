using BrickBreaker.Serv;
using UnityEngine;
using UnityEngine.UI;

namespace BrickBreaker.UI
{
    public class OptionsMenuPanel : MonoBehaviour
    {
        [SerializeField] private GameDataSO gameData;

        [SerializeField] private Slider ballNumberSlider;
        [SerializeField] private Slider ballSpeedSlider;

        [SerializeField] private Button saveBtn;
        [SerializeField] private Button backBtn;

        public UIManager UIManager { get; set; }


        private void OnEnable()
        {
            DefaultValues();
            saveBtn.onClick.AddListener(SaveValues);
            backBtn.onClick.AddListener(ReturnMainMenu);
        }

        private void DefaultValues()
        {
            gameData.ballPoolSize = (int)ballNumberSlider.value;
            gameData.ballSpeed = (int)ballSpeedSlider.value;
        }

        private void SaveValues()
        {
            gameData.ballPoolSize = (int)ballNumberSlider.value;
            gameData.ballSpeed = (int)ballSpeedSlider.value;
            UIManager.SelectMenu?.Invoke(UI.MenuType.MainMenu);

        }

        private void ReturnMainMenu()
        {
            UIManager.SelectMenu?.Invoke(UI.MenuType.MainMenu);
        }
    }
}
