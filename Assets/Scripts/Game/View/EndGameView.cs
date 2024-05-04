using Picker.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picker.View
{
    public class EndGameView : ViewBase
    {
        public delegate void MessageDelegate();

        public event MessageDelegate OnRestartButtonClick;

        public override ViewMenu viewMenu => ViewMenu.EndGameView;

        public void RequestRestartLevel()
        {
            if(OnRestartButtonClick != null)
                OnRestartButtonClick();
        }
    }
}