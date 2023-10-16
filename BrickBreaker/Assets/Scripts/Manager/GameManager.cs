using BrickBreaker.Ball;
using BrickBreaker.Bricks;
using System;
using System.Collections;
using UnityEngine;

namespace BrickBreaker.Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BallView ballPrefab;
        [SerializeField] private BrickManager brickManager;
        [SerializeField] private GameOverPanel gameOverPanel;
        [SerializeField] private int ballSpeed;
        [SerializeField] private int ballPoolSize;
        [SerializeField] private float AimLineLength;
        [SerializeField] private int maxReflections;
        [SerializeField] private float lineOffset;

        public Camera MainCamera
        {
            get
            {
                return mainCamera;
            }
        }

        public Action GameOver;
        public Action RestartGame;

        private AimLineController firePoint;
        private BoundaryManager boundaryManager;
        private BallServicePool ballServicePool;
        private Coroutine LaunchBallCoroutine;

        private bool isAiming;
        private int ballCount;
        private int scoreCount;
        private Vector2 newFirePosition;

        public void LoadValues(int ballSpeed, int ballPoolSize, int maxReflections)
        {
            this.ballSpeed = ballSpeed;
            this.ballPoolSize = ballPoolSize;
            this.maxReflections = maxReflections;
        }

        private void Awake()
        {
            InitializeReference();
        }

        private void OnEnable()
        {
            GameOver += StopGame;
            RestartGame += ReinitializeLevel;
        }

        private void Start()
        {
            InitializeLevel();
        }

        private void OnDisable()
        {
            GameOver -= StopGame;
            RestartGame -= ReinitializeLevel;
        }

        private void Update()
        {
            if (isAiming)
            {
                firePoint.LineRenderer.enabled = true;
                firePoint.Aim();
                firePoint.DrawReflectedTrajectory();
            }

            if (Input.GetMouseButtonDown(0) && isAiming)
            {
                firePoint.LineRenderer.enabled = false;
                isAiming = false;
                Vector2 launchPosition = firePoint.transform.up;

                LaunchCoroutine(new Vector2(launchPosition.x, launchPosition.y));
            }
        }

        private void InitializeReference()
        {
            boundaryManager = GetComponent<BoundaryManager>();
            firePoint = GetComponentInChildren<AimLineController>();
            if (boundaryManager && firePoint)
            {
                boundaryManager.MainCamera = mainCamera;
                firePoint.MainCamera = mainCamera;
            }
        }

        // On level loaded first time, initializes the level - aim trigger, bricks, boundaries
        private void InitializeLevel()
        {
            gameOverPanel.gameObject.SetActive(false);
            ballServicePool = new BallServicePool(ballPoolSize, ballSpeed, ballPrefab, firePoint.transform);
            boundaryManager.SetBoundaries();
            // fire point/ launch position of balls is set at middle bottom of screen
            SetFirePoint();
            // pass on values for aim trajectory
            firePoint.SetLineValues(AimLineLength, maxReflections, lineOffset);
            // pass game manager reference
            gameOverPanel.SetGameManager(this);
            brickManager.GameManager = this;
            brickManager.MainCamera = MainCamera;
            //load brick grids
            brickManager.LoadGrid();
            isAiming = true;
            scoreCount = 0;
        }

        // called if bricks reach bottom of screen
        private void StopGame()
        {
            Debug.Log("chk2");
            isAiming = false;
            // load game over screen panel
            gameOverPanel.SetScoreValue(scoreCount);
            gameOverPanel.gameObject.SetActive(true);
            brickManager.SubscribeRestartLevel();
        }

        // on restart of same level
        private void ReinitializeLevel()
        {
            scoreCount = 0;
            isAiming = true;
            gameOverPanel.gameObject.SetActive(false);
            brickManager.UnsubscribeRestartEvent();
            SetFirePoint();
        }

        private void SetFirePoint()
        {
            Vector3 point = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
            // manually modifying launch point;
            point.y += 0.15f; point.z = 0;
            firePoint.transform.position = point;
        }

        private void LaunchCoroutine(Vector2 launchPos)
        {
            if (LaunchBallCoroutine != null)
            {
                StopCoroutine(LaunchBallCoroutine);
            }
            ballCount = 0;
            LaunchBallCoroutine = StartCoroutine(LaunchBalls(launchPos));
        }

        private IEnumerator LaunchBalls(Vector2 launchPos)
        {
            for (int i = 0; i < ballPoolSize; i++)
            {
                BallController ball = ballServicePool.GetBall();
                if (ball != null)
                {
                    ball.BallView.LaunchBall(launchPos);
                    ball.ReturnBall += ReturnBall;
                    yield return new WaitForSecondsRealtime(0.05f);
                }
            }
        }

        private void ReturnBall(BallController ball)
        {
            ball.ReturnBall -= ReturnBall;
            if (ballCount == 0)
            {
                //update x value of transform.position of firePoint, update firePos after all balls have been launched or after all balls returned
                newFirePosition = firePoint.transform.position;
                newFirePosition.x = ball.BallView.transform.position.x;
            }
            ballServicePool.ReturnBall(ball);
            ballCount++;
            if (ballCount == ballPoolSize)
            {
                firePoint.transform.position = newFirePosition;
                isAiming = true;
                scoreCount++;
                brickManager.TurnEffect();
            }

        }
    }
}