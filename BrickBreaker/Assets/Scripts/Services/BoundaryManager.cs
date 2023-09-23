using UnityEngine;

namespace BrickBreaker
{
    public class BoundaryManager : MonoBehaviour
    {
        [SerializeField] private Transform topRightPrefab;
        [SerializeField] private Transform bottomLeftPrefab;

        private Transform topRightBoundary;
        private Transform bottomLeftBoundary;


        private void Awake()
        {
            topRightBoundary = Instantiate(topRightPrefab);
            bottomLeftBoundary = Instantiate(bottomLeftPrefab);
        }

        public void SetBoundaries()
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

            topRightBoundary.transform.position = point;

            point = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            bottomLeftBoundary.transform.position = point;
        }
    }
}
