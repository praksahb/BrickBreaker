using BrickBreaker.Services;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BrickBreaker
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button quitBtn;
        [SerializeField] private TextMeshProUGUI scoreText;

        private GameManager gameManager;

        private Coroutine quitRoutine;

        private void OnEnable()
        {
            restartBtn.onClick.AddListener(RestartLevel);
            quitBtn.onClick.AddListener(QuitLevel);
        }

        private void OnDisable()
        {
            restartBtn.onClick.RemoveAllListeners();
            quitBtn.onClick.RemoveAllListeners();
        }

        public void SetGameManager(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void SetScoreValue(int value)
        {
            scoreText.text = "Score: " + value.ToString();
        }

        private void RestartLevel()
        {
            gameManager.RestartGame?.Invoke();
        }

        private void QuitLevel()
        {
            if (quitRoutine != null)
            {
                StopCoroutine(quitRoutine);
            }
            quitRoutine = StartCoroutine(LoadLevelAsync("MainMenu"));
        }

        private IEnumerator LoadLevelAsync(string name)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
