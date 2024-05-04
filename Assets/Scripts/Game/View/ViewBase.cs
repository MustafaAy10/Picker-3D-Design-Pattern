using DevelopmentKit.Base.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker.View
{
    public abstract class ViewBase : MonoBehaviour, IService
    {
        protected IServiceLocator ServiceLocator;

        public abstract ViewMenu viewMenu { get; }

        public virtual void Initialize(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;

        }

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }



    }

   
}