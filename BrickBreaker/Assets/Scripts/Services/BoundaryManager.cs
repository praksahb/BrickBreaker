using UnityEngine;

namespace BrickBreaker
{
    public class BoundaryManager : MonoBehaviour
    {
        [SerializeField] private Transform topRightPrefab;
        [SerializeField] private Transform bottomLeftPrefab;

        private Transform topRightBoundary;
        private Transform bottomLeftBoundary;


        public Camera MainCamera { get; set; }

        private void Awake()
        {
            topRightBoundary = Instantiate(topRightPrefab);
            bottomLeftBoundary = Instantiate(bottomLeftPrefab);
        }

        public void SetBoundaries()
        {
            Vector3 point = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.nearClipPlane));

            topRightBoundary.transform.position = point;

            point = MainCamera.ScreenToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));
            bottomLeftBoundary.transform.position = point;
        }
    }
}
