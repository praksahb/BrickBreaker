using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BrickBreaker.UI
{
    public class PlayMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button l1Button;
        [SerializeField] private Button l2Button;

        private Coroutine levelLoader;

        public UIManager UIManager { get; set; }

        private void OnEnable()
        {
            l1Button.onClick.AddListener(LoadLevel1);
            l2Button.onClick.AddListener(LoadLevel2);
        }

        private void OnDisable()
        {
            l1Button.onClick.RemoveAllListeners();
            l2Button.onClick.RemoveAllListeners();
        }

        private void LoadLevel1()
        {
            if (levelLoader != null)
            {
                StopCoroutine(levelLoader);
            }
            levelLoader = StartCoroutine(LoadLevelAsync(1));
        }

        private void LoadLevel2()
        {
            StartCoroutine(LoadLevelAsync(2));
        }

        private IEnumerator LoadLevelAsync(int level)
        {
            int levelIdx = SceneManager.GetActiveScene().buildIndex + level;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelIdx, LoadSceneMode.Single);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
