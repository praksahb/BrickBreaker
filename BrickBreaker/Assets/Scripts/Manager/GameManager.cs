﻿using BrickBreaker.Ball;
using BrickBreaker.Services.Aiming;
using System.Collections;
using UnityEngine;

namespace BrickBreaker.Services
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BallView ballPrefab;
        [SerializeField] private int ballSpeed;
        [SerializeField] private int ballPoolSize;
        [SerializeField] private Transform firePoint;
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

        private BoundaryManager boundaryManager;
        private AimLineController aimLineController;
        private BallServicePool ballServicePool;
        private Coroutine LaunchBallCoroutine;

        private bool isAiming;
        private int count;
        private Vector2 newFirePosition;

        private void Awake()
        {
            boundaryManager = GetComponent<BoundaryManager>();
            aimLineController = firePoint.GetComponent<AimLineController>();
            if (boundaryManager && aimLineController)
            {
                boundaryManager.MainCamera = mainCamera;
                aimLineController.MainCamera = mainCamera;
            }
        }

        private void Start()
        {
            ballServicePool = new BallServicePool(ballPoolSize, ballSpeed, ballPrefab, firePoint.transform);
            SetFirePoint();
            boundaryManager.SetBoundaries();
            aimLineController.SetLineValues(AimLineLength, maxReflections, lineOffset);
        }

        private void Update()
        {
            if (isAiming)
            {
                aimLineController.LineRenderer.enabled = true;
                aimLineController.Aim();
                aimLineController.DrawReflectedTrajectory();
            }

            if (Input.GetMouseButtonDown(0) && isAiming)
            {
                aimLineController.LineRenderer.enabled = false;
                isAiming = false;
                Vector2 launchPosition = firePoint.transform.up;

                LaunchCoroutine(new Vector2(launchPosition.x, launchPosition.y));
            }
        }

        public void StartGame()
        {
            isAiming = true;
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
            LaunchBallCoroutine = StartCoroutine(LaunchBalls(launchPos));
            count = 0;
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
            if (count == 0)
            {
                //update x value of transform.position of firePoint, update firePos after all balls have been launched or after all balls returned
                newFirePosition = firePoint.transform.position;
                newFirePosition.x = ball.BallView.transform.position.x;
            }
            ballServicePool.ReturnBall(ball);
            count++;
            if (count == ballPoolSize)
            {
                firePoint.transform.position = newFirePosition;
                isAiming = true;
            }

        }
    }
}