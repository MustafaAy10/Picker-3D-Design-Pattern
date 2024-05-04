using Game.CollectableSystem;
using Game.PickerSystem.Managers;
using Picker.Services;
using UnityEngine;

namespace Game.PickerSystem.Controllers
{
    public class PickerPhysicsController : MonoBehaviour
    {
        private PickerPhysicsManager _pickerPhysicsManager;
        private PickerMovementController _pickerMovementController;
        private GamePlayController gamePlayController;

        public void Initialize(PickerPhysicsManager pickerPhysicsManager, PickerMovementController pickerMovementController, GamePlayController gamePlayController)
        {
            _pickerPhysicsManager = pickerPhysicsManager;
            _pickerMovementController = pickerMovementController;
            this.gamePlayController = gamePlayController;
        }

        public void PushCollectables()
        {
            
            foreach (var collectable in _pickerPhysicsManager.GetCollectables())
            {
                collectable.Push();
            }
        }

        public int GetCollectablesCount()
        {
            return _pickerPhysicsManager.GetCollectables().Count;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var collectable = other.GetComponent<CollectableBase>();
            if (collectable != null)
            {
                _pickerPhysicsManager.AddCollectable(collectable);
            }

            if (other.CompareTag("Final"))
            {
                gamePlayController.TriggerLevelComplete();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var collectable = other.GetComponent<CollectableBase>();
            if (collectable != null)
            {
                _pickerPhysicsManager.RemoveCollectable(collectable);
            }
        }
    }
}
