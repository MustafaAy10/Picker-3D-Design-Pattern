using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker.Services
{
    public class CameraController : MonoBehaviour, IService, IInitializable, IUpdatable
    {
        private Vector3 startPosition;

        public void Initialize(IServiceLocator serviceLocator)
        {
            startPosition = transform.position;
        }

        public void Init()
        {
            transform.position = startPosition;
        }

        public void CallUpdate()
        {

        }

       
    }
}