using DevelopmentKit.Base.Services;
using Game.States;
using Picker.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker.Services
{
    public class MainServiceController : MonoBehaviour
    {
        private AppState appState;

        private CameraController cameraController;
        private GamePlayController gamePlayController;
        private InGameInputSystem inGameInputSystem;
        private UIController uiController;
        private AccountController accountController;

        private IServiceLocator serviceLocator;

        void Awake()
        {
            serviceLocator = new ServiceLocator();

            CreateUIController();
            CreateGamePlayController();
            CreateCameraController();
            CreateInGameInputSystem();
            CreateAccountController();

            InitializeAllServices();

            CreateAppState();

            appState.Enter();

        }

        private void CreateUIController()
        {
            uiController = FindObjectOfType<UIController>();
            serviceLocator.Add(ServiceKeys.UI_SERVICE, uiController);
        }

        private void CreateGamePlayController()
        {
            gamePlayController = FindObjectOfType<GamePlayController>();
            serviceLocator.Add(ServiceKeys.GAME_PLAY_SERVICE, gamePlayController);
        }

        private void CreateInGameInputSystem()
        {
            inGameInputSystem = new InGameInputSystem();
            serviceLocator.Add(ServiceKeys.IN_GAME_INPUT_SYSTEM_SERVICE, inGameInputSystem);
        }

        private void CreateCameraController()
        {
            cameraController = FindObjectOfType<CameraController>();
            serviceLocator.Add(ServiceKeys.GAME_CAMERA_SERVICE, cameraController);
        }

        private void CreateAccountController()
        {
            accountController = new AccountController();
            serviceLocator.Add(ServiceKeys.ACCOUNT_SERVICE, accountController);
        }

        private void InitializeAllServices()
        {
            cameraController.Initialize(serviceLocator);
            inGameInputSystem.Initialize(serviceLocator);
            uiController.Initialize(serviceLocator);
            accountController.Initialize(serviceLocator);
            gamePlayController.Initialize(serviceLocator);
        }

        private void CreateAppState()
        {
            appState = new AppState(serviceLocator);
        }

        private void Update()
        {
            appState.Update();
        }
    }
}