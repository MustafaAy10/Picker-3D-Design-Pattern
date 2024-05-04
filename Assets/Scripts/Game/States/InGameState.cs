 
using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Picker.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class InGameState : StateMachine
    {
        private IServiceLocator serviceLocator;
        private GamePlayController gamePlayController;

        public InGameState(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            gamePlayController = serviceLocator.Get<GamePlayController>(ServiceKeys.GAME_PLAY_SERVICE);
        }

        protected override void OnEnter()
        {
            Debug.Log("[InGameState] OnEnter() called...");

            gamePlayController.OnGameOver += OnGameOver;
            gamePlayController.OnLevelComplete += OnLevelComplete;

            gamePlayController.ActivatePlayer();

        }

        protected override void OnUpdate()
        {
            gamePlayController.CallUpdate();

            Debug.Log("[InGameState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            gamePlayController.OnExit();

            gamePlayController.OnGameOver -= OnGameOver;
            gamePlayController.OnLevelComplete -= OnLevelComplete;

            Debug.Log("[InGameState] OnExit() called...");
        }

        private void OnGameOver()
        {
            SendTrigger((int)StateTriggers.EndGameState);
        }

        private void OnLevelComplete()
        {
            SendTrigger((int)StateTriggers.NextLevelState);
        }
    }
}