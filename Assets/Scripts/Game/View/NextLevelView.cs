using Picker.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Picker.View
{
    public class NextLevelView : ViewBase
    {
        public delegate void MessageDelegate();
        public event MessageDelegate OnNextLevelButtonClick;

        public override ViewMenu viewMenu => ViewMenu.NextLevelView;

        public void RequestNextLevel()
        {
            if(OnNextLevelButtonClick != null)
                OnNextLevelButtonClick();
        }
    }
}