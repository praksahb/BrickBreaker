using BrickBreaker.UI;
using System;
using UnityEngine;

namespace BallBreaker.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuPanel mainMenuPanel;
        [SerializeField] private PlayMenuPanel playMenuPanel;

        public Action LaunchPlayMenu;

        private void Awake()
        {
            InitializeMenu();
        }

        private void OnEnable()
        {
            LaunchPlayMenu += EnablePlayMenu;
        }

        private void OnDisable()
        {
            LaunchPlayMenu -= EnablePlayMenu;
        }

        private void InitializeMenu()
        {
            mainMenuPanel.UIManager = this;
            playMenuPanel.UIManager = this;
            mainMenuPanel.gameObject.SetActive(true);
            playMenuPanel.gameObject.SetActive(false);
        }

        private void EnablePlayMenu()
        {
            mainMenuPanel.gameObject.SetActive(false);
            playMenuPanel.gameObject.SetActive(true);
        }
    }
}
