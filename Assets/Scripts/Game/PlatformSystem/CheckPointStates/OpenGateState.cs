using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Game.PlatformSystem.Base;
using Picker.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlatformSystem.State
{
    public class OpenGateState : StateMachine
    {
        private ICheckPoint checkPoint;
        private CheckPointEventContainer checkPointEventContainer;
        private float elapsedTime;
        private const float DURATION_FOR_OPEN_GATE = 1;
        private bool isTimeUp = false;

        public OpenGateState( ICheckPoint checkPoint, CheckPointEventContainer checkPointEventContainer)
        {
            this.checkPoint = checkPoint;
            this.checkPointEventContainer = checkPointEventContainer;
        }

        protected override void OnEnter()
        {
            elapsedTime = 0;
            isTimeUp = false;

            checkPointEventContainer.TriggerOpenGateStateEnter();

            Debug.Log("[OpenGateState] OnEnter() called...");
        }

        protected override void OnUpdate()
        {
            if (!isTimeUp)
            {
                checkPoint.OpenGate(elapsedTime / DURATION_FOR_OPEN_GATE);

                if (elapsedTime > DURATION_FOR_OPEN_GATE)
                {
                    isTimeUp = true;

                    checkPointEventContainer.TriggerCheckPointExit();
                }
                elapsedTime += Time.deltaTime;
            }

            Debug.Log("[OpenGateState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            checkPointEventContainer.TriggerOpenGateStateExit();

            Debug.Log("[OpenGateState] OnExit() called...");
        }
    }
}