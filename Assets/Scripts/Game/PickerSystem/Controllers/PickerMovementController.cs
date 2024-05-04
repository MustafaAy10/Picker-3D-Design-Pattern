using DevelopmentKit.Base.Object;
using Picker.Services;
using System;
using UnityEngine;

namespace Game.PickerSystem.Controllers
{
    public class PickerMovementController : MonoBehaviour, IUpdatable
    {
        private bool _active;
        private float _forwardSpeed;
        private float _xSpeed;

        private Camera _pickerCamera;
        private Vector3 _mousePos;
        private float _distanceToScreen;
        private InGameInputSystem inGameInputSystem;

        public void Initialize(Camera pickerCamera, InGameInputSystem inGameInputSystem)
        {
            _pickerCamera = pickerCamera;
            _forwardSpeed = 5f;
            _xSpeed = 10f;
            this.inGameInputSystem = inGameInputSystem;
            //Activate();
        }

        public void Activate()
        {
            _active = true;
            inGameInputSystem.OnScreenTouch += OnMouseButton;
        }

        public void Deactivate()
        {
            _active = false;
            inGameInputSystem.OnScreenTouch -= OnMouseButton;

        }

        public void CallUpdate()
        {
            if(!_active)
                return;
            
            transform.Translate(0,0,Time.deltaTime * _forwardSpeed);
        }

        private void OnMouseButton()
        {
            var position = Input.mousePosition;

            _distanceToScreen = _pickerCamera.WorldToScreenPoint(gameObject.transform.position).z;
            _mousePos = _pickerCamera.ScreenToWorldPoint(new Vector3(position.x, position.y, _distanceToScreen));
            float direction = _xSpeed;
            direction = _mousePos.x > transform.position.x ? direction : -direction;

            if (Math.Abs(_mousePos.x - transform.position.x) > 0.5f)
                transform.Translate(Time.deltaTime * direction, 0, 0);
        }
    }
}
