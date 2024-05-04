using System;
using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
 
using Game.PickerSystem.Controllers;
using Game.PickerSystem.Managers;
using Picker.Services;
using UnityEngine;

namespace Game.PickerSystem.Base
{
    public class PickerBase : MonoBehaviour, IInitializable, IUpdatable
    {
        public Action<int> OnPointGained;
        
        private CameraController _pickerCamera;
        private Vector3 _cameraOffset;
        
        private PickerPhysicsManager _pickerPhysicsManager;

        private PickerPhysicsController _pickerPhysicsController;
        private PickerMovementController _pickerMovementController;

        private Vector3 _pickerStartPosition = new Vector3(0,0.6f,2.5f);

        public void Init()
        {
            transform.position = _pickerStartPosition;
        }

        public void Initialize(IServiceLocator serviceLocator)
        {
            _pickerCamera = serviceLocator.Get<CameraController>(ServiceKeys.GAME_CAMERA_SERVICE);
            _cameraOffset = _pickerCamera.transform.position - transform.position;
            
            _pickerPhysicsManager = new PickerPhysicsManager();

            _pickerMovementController = GetComponent<PickerMovementController>();
            _pickerPhysicsController = GetComponent<PickerPhysicsController>();
            
            _pickerMovementController.Initialize(_pickerCamera.GetComponent<Camera>(), serviceLocator.Get<InGameInputSystem>(ServiceKeys.IN_GAME_INPUT_SYSTEM_SERVICE));
            _pickerPhysicsController.Initialize(_pickerPhysicsManager,_pickerMovementController, serviceLocator.Get<GamePlayController>(ServiceKeys.GAME_PLAY_SERVICE));
            
            
        }

        public void CallUpdate()
        {
            if(_pickerCamera == null)
                return;
            
            _pickerCamera.transform.position = new Vector3(_pickerCamera.transform.position.x,_pickerCamera.transform.position.y,
                transform.position.z + _cameraOffset.z);
        }
    }
}
