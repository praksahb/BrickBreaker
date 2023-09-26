using BrickBreaker.Services;
using System;
using UnityEngine;

namespace BallBreaker
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private MainMenuPanel mainMenuPanel;
        [SerializeField] private GameManager gameManager;


        public Action StartGame;

        private void Awake()
        {
            mainMenuPanel.UIManager = this;
        }

        private void OnEnable()
        {
            StartGame += StartGameFunction;
        }

        private void OnDisable()
        {
            StartGame -= StartGameFunction;
        }

        private void StartGameFunction()
        {
            mainMenuPanel.gameObject.SetActive(false);
            gameManager.StartGame();
        }
    }
}
