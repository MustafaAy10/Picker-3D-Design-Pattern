using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Picker.Services
{
    public class InGameInputSystem : IUpdatable, IService, IDestructible, IInitializable
    {
        public delegate void TouchMessageDelegate();
        public event TouchMessageDelegate OnScreenTouch;

        private bool isInputAvailable = false;

        public void Initialize(IServiceLocator serviceLocator)
        {
            
        }

        public void CallUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                // Debug.Log("OnScreenTouch");
                if (OnScreenTouch != null)
                {
                    OnScreenTouch();
                }
            }
        }

        public void CallFixedUpdate()
        {

        }

        public void CallLateUpdate()
        {

        }

       

        public void OnDestruct()
        {
            isInputAvailable = false;
        }

        public void Init()
        {
            isInputAvailable = true;
        }

       
    }
}
