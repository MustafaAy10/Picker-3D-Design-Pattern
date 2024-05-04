using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
using DG.Tweening;
 
using Game.LevelSystem;
using Game.PickerSystem.Base;
using Game.PickerSystem.Controllers;
using Game.PlatformSystem.CheckPointControllers;
using Game.PlatformSystem.State;
using Picker.Services;
using Picker.View;
using UnityEngine;
 

namespace Game.PlatformSystem.Base
{
    public class CheckPoint : PlatformBase, IUpdatable, ICheckPoint
    {
        public override PlatformType PlatformType => PlatformType.CHECKPOINT;
        private int _target;

        private IServiceLocator serviceLocator;
        private InGameView inGameView;
        private GamePlayController gamePlayController;
        private PlatformFactory platformFactory;
        private CheckPointEventContainer checkPointEventContainer;

        private PickerMovementController pickerMovementController;
        private CheckPointCounterPlatform _checkPointCounterPlatform;
        private Transform _gate1;
        private Transform _gate2;

        private Quaternion gate1firstRotation, gate2firstRotation;

        private CheckPointMainState mainState;
        private int counter;

        public override void Initialize()
        {
            base.Initialize();
            
            _checkPointCounterPlatform = GetComponentInChildren<CheckPointCounterPlatform>(true);
            _checkPointCounterPlatform.Initialize();

            checkPointEventContainer = new CheckPointEventContainer();

            _gate1 = transform.Find("Gate1");
            _gate2 = transform.Find("Gate2");
            
            gate1firstRotation = _gate1.rotation;
            gate2firstRotation = _gate2.rotation;
        }

        public void Inject(IServiceLocator serviceLocator, PlatformFactory platformFactory)
        {
            this.serviceLocator = serviceLocator;
            this.platformFactory = platformFactory;

            inGameView = serviceLocator.Get<UIController>(ServiceKeys.UI_SERVICE).GetView<InGameView>(ViewMenu.InGameView);
            gamePlayController = serviceLocator.Get<GamePlayController>(ServiceKeys.GAME_PLAY_SERVICE);

            mainState = new CheckPointMainState(this, checkPointEventContainer);
        }

        public void OnEnter()
        {
            AddEvents();
            mainState.Enter();
            platformFactory.AddActiveCheckPoint(this);
            pickerMovementController.Deactivate();
        }

        public void OnExit()
        {
            platformFactory.RemoveActiveCheckPoint(this);
            pickerMovementController.Activate();
            mainState.Exit();
            RemoveEvents();
        }

        private void AddEvents()
        {
            checkPointEventContainer.OnCheckPointExit += OnExit;
            checkPointEventContainer.OnCheckPointFail += OnFail;
            checkPointEventContainer.OnOpenGateStateEnter += OnOpenGateStateEnter;
            checkPointEventContainer.OnOpenGateStateExit += OnOpenGateStateExit;
        }

        private void RemoveEvents()
        {
            checkPointEventContainer.OnCheckPointExit -= OnExit;
            checkPointEventContainer.OnCheckPointFail -= OnFail;
            checkPointEventContainer.OnOpenGateStateEnter -= OnOpenGateStateEnter;
            checkPointEventContainer.OnOpenGateStateExit -= OnOpenGateStateExit;
        }

        public void CallUpdate()
        {
            mainState.Update();
        }

        public void OnOpenGateStateEnter()
        {
            var counter = _checkPointCounterPlatform.GetCounter();
            inGameView.ChangePoint(counter);
        }

        public void OnOpenGateStateExit()
        {
            _checkPointCounterPlatform.SuccesfulAction();
            inGameView.ActiveCheckPointItem();
        }

        public void OnFail()
        {
            gamePlayController.TriggerGameOver();
        }

        public bool IsCounterStateSuccessful()
        {
            var counter = _checkPointCounterPlatform.GetCounter();
            return counter >= _target;
        }

        public void OpenGate(float percentage)
        {
            _gate1.rotation = Quaternion.Lerp(gate1firstRotation, Quaternion.Euler(new Vector3(-80, 90, 90)), percentage);
            _gate2.rotation = Quaternion.Lerp(gate2firstRotation, Quaternion.Euler(new Vector3(80, 90, 90)), percentage);
            _checkPointCounterPlatform.DoMove(percentage);
        }

        public void SetTarget(int aim)
        {
            _target = aim;
            _checkPointCounterPlatform.SetTarget(_target);
            SetDefault();
        }

        private void SetDefault()
        {
            _gate1.rotation = gate1firstRotation;
            _gate2.rotation = gate2firstRotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            var pickerPhysicsController = other.GetComponent<PickerPhysicsController>();
            if (pickerPhysicsController != null)
            {
                pickerMovementController = pickerPhysicsController.GetComponent<PickerMovementController>();
                var collectableCount = pickerPhysicsController.GetCollectablesCount();
                _checkPointCounterPlatform.SetCollectablesCount(collectableCount);
                pickerPhysicsController.PushCollectables();
                OnEnter();
            }
        }

        
    }
}
