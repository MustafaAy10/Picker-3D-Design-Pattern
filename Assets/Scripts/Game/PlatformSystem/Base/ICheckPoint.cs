using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.PlatformSystem.Base
{
    public interface ICheckPoint
    {
        bool IsCounterStateSuccessful();
        void OpenGate(float percentage);
    }
}