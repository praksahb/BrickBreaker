using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BrickBreaker
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button quitBtn;


        private Coroutine quitRoutine;

        private void OnEnable()
        {
            restartBtn.onClick.AddListener(RestartLevel);
            quitBtn.onClick.AddListener(QuitLevel);
        }

        private void RestartLevel()
        {

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
