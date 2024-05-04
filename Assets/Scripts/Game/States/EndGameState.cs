using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Picker.Services;
using Picker.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class EndGameState : StateMachine
    {
        private IServiceLocator serviceLocator;
        private UIController uiController;
        private EndGameView endGameView; 

        public EndGameState(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            uiController = serviceLocator.Get<UIController>(ServiceKeys.UI_SERVICE);
            endGameView = uiController.GetView<EndGameView>(ViewMenu.EndGameView);
        }

        protected override void OnEnter()
        {
            endGameView.OnRestartButtonClick += OnRestart;
            
            uiController.EnableView(ViewMenu.EndGameView);

            Debug.Log("[EndGameState] OnEnter() called...");
        }

        protected override void OnUpdate()
        {
            Debug.Log("[EndGameState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            endGameView.OnRestartButtonClick -= OnRestart;
            uiController.DisableView();

            Debug.Log("[EndGameState] OnExit() called...");
        }

        private void OnRestart()
        {
            SendTrigger((int)StateTriggers.IntroGameState);
        }
    }
}