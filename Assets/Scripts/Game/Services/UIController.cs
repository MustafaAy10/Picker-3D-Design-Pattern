using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
using Picker.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Picker.Services
{
    public class UIController : MonoBehaviour, IService, IDestructible
    {

        private List<ViewBase> views;  
        private ViewBase activeView;

        public void Initialize(IServiceLocator serviceLocator)
        {
            views = GetComponentsInChildren<ViewBase>(true).ToList();
            views.ForEach(view => view.Initialize(serviceLocator));
        }

        public T GetView<T>(ViewMenu viewMenu)  where T : ViewBase
        {
            return (T)views.FirstOrDefault(view => view.viewMenu == viewMenu);
        }

        public void EnableView(ViewMenu viewMenu)
        {
            DeactivateView(activeView);
            ActivateView(viewMenu);
        }

        public void DisableView()
        {
            DeactivateView(activeView);
        }

        private void DeactivateView(ViewBase viewBase)
        {
            if (viewBase != null)
            {
                viewBase.Deactivate();
            }
        }

        private void ActivateView(ViewMenu viewMenu)
        {
            activeView = views.FirstOrDefault(view => view.viewMenu == viewMenu);

            if (activeView != null)
            {
                activeView.Activate();
            }
        }

        public void OnDestruct()
        {

        }
    }
}