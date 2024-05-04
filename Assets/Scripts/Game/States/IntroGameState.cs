 
using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Picker.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class IntroGameState : StateMachine
    {
        private IServiceLocator serviceLocator;
        private GamePlayController gamePlayController;
        private UIController uiController;

        public IntroGameState(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            gamePlayController = serviceLocator.Get<GamePlayController>(ServiceKeys.GAME_PLAY_SERVICE);
            uiController = serviceLocator.Get<UIController>(ServiceKeys.UI_SERVICE);
        }

        protected override void OnEnter()
        {
            gamePlayController.OnEnter();

            uiController.EnableView(Picker.View.ViewMenu.IntroGameView);

            Debug.Log("[IntroGameState] OnEnter() called...");
        }

        protected override void OnUpdate()
        {
            // Intro'ya Tap To Play demek için gerek duyduk.
            if (Input.GetMouseButtonDown(0))
            {
                SendTrigger((int)StateTriggers.InGameState);
            }

            Debug.Log("[IntroGameState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            uiController.DisableView();

            Debug.Log("[IntroGameState] OnExit() called...");
        }
    }
}