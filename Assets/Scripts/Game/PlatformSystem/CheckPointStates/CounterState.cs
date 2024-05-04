 
using DevelopmentKit.Base.Services;
using DevelopmentKit.HSM;
using Game.PlatformSystem.Base;
using Picker.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PlatformSystem.State
{
    public class CounterState : StateMachine
    {
        private float timer;
        private const float TIME_TO_WAIT_ON_COUNTER_PLATFORM = 2;
        private bool isTimeUp = false;
        private ICheckPoint checkPoint;
        private CheckPointEventContainer checkPointEventContainer;

        public CounterState( ICheckPoint checkPoint, CheckPointEventContainer checkPointEventContainer)
        {
            this.checkPoint = checkPoint;
            this.checkPointEventContainer = checkPointEventContainer;
        }

        protected override void OnEnter()
        {
            timer = 0;
            isTimeUp = false;
            Debug.Log("[CounterState] OnEnter() called...");
        }

        protected override void OnUpdate()
        {
            if (!isTimeUp)
            {
                if (timer > TIME_TO_WAIT_ON_COUNTER_PLATFORM)
                {
                    isTimeUp = true;
                    if (checkPoint.IsCounterStateSuccessful())
                    {
                        SendTrigger((int)CheckPointStateTriggers.OPEN_GATE_STATE);
                    }
                    else
                    {
                        checkPointEventContainer.TriggerCheckPointFail();
                        checkPointEventContainer.TriggerCheckPointExit();
                    }
                }
                timer += Time.deltaTime;
            }

            Debug.Log("[CounterState] OnUpdate() called...");
        }

        protected override void OnExit()
        {
            Debug.Log("[CounterState] OnExit() called...");
        }
    }
}