using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
using Game.LevelSystem;
using Game.PickerSystem.Base;
using Game.PickerSystem.Controllers;
using Picker.Services;
using Picker.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker.Services
{
    public class GamePlayController : MonoBehaviour, IService, IUpdatable, IDestructible
    {
        private IServiceLocator serviceLocator;

        public delegate void MessageDelegate();
        public event MessageDelegate OnGameOver;
        public event MessageDelegate OnLevelComplete;
        private PickerMovementController pickerMovementController;
        private PickerBase pickerBase;
        public PickerBase Player => pickerBase;

        private LevelGenerator levelGenerator;
        private PlatformFactory platformFactory;
        private InGameInputSystem inputSystem;
        private CameraController cameraController;
        private InGameView inGameView;

        public void Initialize(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            inputSystem = serviceLocator.Get<InGameInputSystem>(ServiceKeys.IN_GAME_INPUT_SYSTEM_SERVICE);
            cameraController = serviceLocator.Get<CameraController>(ServiceKeys.GAME_CAMERA_SERVICE);
            inGameView = serviceLocator.Get<UIController>(ServiceKeys.UI_SERVICE).GetView<InGameView>(ViewMenu.InGameView);

            CreatePlayer();
            CreateInstance();

        }

        private void CreatePlayer()
        {
            pickerBase = FindObjectOfType<PickerBase>();
            pickerBase.Initialize(serviceLocator);
            pickerMovementController = pickerBase.GetComponent<PickerMovementController>();
        }

        private void CreateInstance()
        {
            levelGenerator = new LevelGenerator();
            platformFactory = new PlatformFactory();

            levelGenerator.Initialize(pickerBase, platformFactory);
            platformFactory.Initialize(serviceLocator);

        }

        public void OnEnter()
        {
            OnDestruct();
            LoadLevel();
            Init();
        }

        public void ActivatePlayer()
        {
            pickerMovementController.Activate();
        }

        public void Init()
        {
            pickerBase.Init();
            cameraController.Init();
        }

        public void OnExit()
        {

        }

        public void CallUpdate()
        {
            inputSystem.CallUpdate();
            platformFactory.CallUpdate();
            pickerBase.CallUpdate();
            pickerMovementController.CallUpdate();

        }

        public void OnDestruct()
        {
            platformFactory.OnDestruct();
            inGameView.OnDestruct();
        }

        private void LoadLevel()
        {
            levelGenerator.GenerateLevel();
        }

        public void TriggerGameOver()
        {
            OnGameOver?.Invoke();
        }

        public void TriggerLevelComplete()
        {
            levelGenerator.IncreaseLevel();

            pickerMovementController.Deactivate();

            OnLevelComplete?.Invoke();
        }
    }
}