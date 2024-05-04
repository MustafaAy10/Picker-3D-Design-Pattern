using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.PlatformSystem.State
{
    public class CheckPointEventContainer
    {
        public delegate void MessageDelegate();

        public event MessageDelegate OnCheckPointExit;
        public event MessageDelegate OnCheckPointFail;
        public event MessageDelegate OnOpenGateStateEnter;
        public event MessageDelegate OnOpenGateStateExit;

        public void TriggerCheckPointExit()
        {
            if (OnCheckPointExit != null)
                OnCheckPointExit();
        }

        public void TriggerCheckPointFail()
        {
            if (OnCheckPointFail != null)
                OnCheckPointFail();
        }

        public void TriggerOpenGateStateEnter()
        {
            if (OnOpenGateStateEnter != null)
                OnOpenGateStateEnter();
        }

        public void TriggerOpenGateStateExit()
        {
            if (OnOpenGateStateExit != null)
                OnOpenGateStateExit();
        }
    }
}
