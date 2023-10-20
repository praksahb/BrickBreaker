using BrickBreaker.Ball;
using BrickBreaker.Bricks;
using BrickBreaker.Serv;
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
        [SerializeField] private GameDataSO gameData;

        public Camera MainCamera
        {
            get
            {
                return mainCamera;
            }
        }

        public Action GameOver;
        public Action RestartGame;

        private int ballSpeed;
        private int ballPoolSize;

        private AimLineController firePoint;
        private BoundaryManager boundaryManager;
        private BallServicePool ballServicePool;
        private Coroutine LaunchBallCoroutine;

        //private bool isAiming;
        private int ballCount;
        private int scoreCount;
        private Vector2 newFirePosition;



        private void Awake()
        {
            InitializeReference();
            LoadBallValues();
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
        private void LoadBallValues()
        {
            ballSpeed = gameData.ballSpeed;
            ballPoolSize = gameData.ballPoolSize;
        }

        // On level loaded first time, initializes the level - aim trigger, bricks, boundaries
        private void InitializeLevel()
        {
            // setup boundary walls
            boundaryManager.SetBoundaries();

            // setup bricks
            brickManager.GameManager = this;
            brickManager.MainCamera = MainCamera;
            brickManager.LoadGrid();
            // Get size of brick and pass its lower value as radius value for balls
            Vector2 brickSize = brickManager.GetCurrentBrickSize();

            float ballRadius = Mathf.Min(brickSize.x, brickSize.y) / 2;
            ballRadius = Mathf.Min(0.2f, ballRadius);
            // setup balls
            BallData ballData = new BallData(ballPrefab, ballSpeed, ballRadius);
            ballServicePool = new BallServicePool(ballPoolSize, ballData, firePoint.transform);
            // fire point/ launch position of balls is set at middle bottom of screen
            SetFirePoint();
            // subscribe to fire action
            firePoint.FireBalls += LaunchCoroutine;
            firePoint.IsAiming = true;

            // pass game manager reference
            gameOverPanel.SetGameManager(this);
            gameOverPanel.gameObject.SetActive(false);

            scoreCount = 0;
        }

        // called if bricks reach bottom of screen
        private void StopGame()
        {
            firePoint.IsAiming = false;
            firePoint.FireBalls -= LaunchCoroutine;
            // load game over screen panel
            gameOverPanel.SetScoreValue(scoreCount);
            gameOverPanel.gameObject.SetActive(true);
            brickManager.SubscribeRestartLevel();
        }

        // on restart of same level
        private void ReinitializeLevel()
        {
            scoreCount = 0;
            firePoint.IsAiming = true;
            firePoint.FireBalls += LaunchCoroutine;
            gameOverPanel.gameObject.SetActive(false);
            brickManager.UnsubscribeRestartEvent();
            SetFirePoint();
        }

        private void SetFirePoint()
        {
            Vector3 point = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
            // manually modifying launch point;
            point.y += 0.2f; point.z = 0;
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
                firePoint.IsAiming = true;
                scoreCount++;
                brickManager.TurnEffect();
            }

        }
    }
}