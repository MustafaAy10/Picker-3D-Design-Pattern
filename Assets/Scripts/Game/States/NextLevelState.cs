using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Picker.Services;
using Picker.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class NextLevelState : StateMachine
    {
        private IServiceLocator serviceLocator;
        private UIController uiController;
        private NextLevelView nextLevelView;
        
        public NextLevelState(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            uiController = serviceLocator.Get<UIController>(ServiceKeys.UI_SERVICE);
            nextLevelView = uiController.GetView<NextLevelView>(ViewMenu.NextLevelView);
        }

        protected override void OnEnter()
        {
            nextLevelView.OnNextLevelButtonClick += OnPlayNextLevel;

            uiController.EnableView(ViewMenu.NextLevelView);

            Debug.Log("[NextLevelState] OnEnter() called...");
        }

        protected override void OnUpdate()
        {
            Debug.Log("[NextLevelState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            nextLevelView.OnNextLevelButtonClick -= OnPlayNextLevel;
           
            uiController.DisableView();

            Debug.Log("[NextLevelState] OnExit() called...");
        }

        private void OnPlayNextLevel()
        {
            SendTrigger((int)StateTriggers.IntroGameState);
        }
    }
}