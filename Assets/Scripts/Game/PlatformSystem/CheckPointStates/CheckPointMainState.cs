using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Game.PlatformSystem.Base;
using Picker.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlatformSystem.State
{
    public class CheckPointMainState : StateMachine
    {
        
        private OpenGateState openGateState;
        private CounterState counterState;
        private ICheckPoint checkPoint;
        private CheckPointEventContainer checkPointEventContainer;

        public CheckPointMainState(ICheckPoint checkPoint, CheckPointEventContainer checkPointEventContainer)
        {
            
            this.checkPoint = checkPoint;
            this.checkPointEventContainer = checkPointEventContainer;

            counterState = new CounterState( checkPoint, checkPointEventContainer);
            openGateState = new OpenGateState( checkPoint, checkPointEventContainer);

            AddSubState(counterState);
            AddSubState(openGateState);

            AddTransition(counterState, openGateState, (int) CheckPointStateTriggers.OPEN_GATE_STATE);
            
        }

        protected override void OnEnter()
        {
            Debug.Log("[CheckPointMainState] OnEnter() called...");
        }

        protected override void OnUpdate()
        {
            Debug.Log("[CheckPointMainState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            SetDefaultState();

            Debug.Log("[CheckPointMainState] OnExit() called...");
        }
    }

    public enum CheckPointStateTriggers
    {
        COUNTER_STATE = 0,
        OPEN_GATE_STATE = 1
    }
}