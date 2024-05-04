using DG.Tweening;
using Game.CollectableSystem;
 
using Game.Managers;
using TMPro;
using UnityEngine;

namespace Game.PlatformSystem.CheckPointControllers
{
    public class CheckPointCounterPlatform : MonoBehaviour
    {
        private int _targetCounter;
        private int _counter;
        private int pickerCollectablesCount = 1;
        private TextMeshPro _textMesh;
        private MeshRenderer _meshRenderer;
        private Vector3 firstLocalPosition, lastLocalPosition;

        public void Initialize()
        {
            _textMesh = GetComponentInChildren<TextMeshPro>(true);
            _meshRenderer = GetComponent<MeshRenderer>();
            firstLocalPosition = new Vector3(transform.localPosition.x,-3.43f,transform.localPosition.z);
            lastLocalPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }

        public void SetTarget(int target)
        {
            _counter = 0;
            _targetCounter = target;
            SetDefault();
        }

        public void SuccesfulAction()
        {
            _textMesh.enabled = false;
            _meshRenderer.material = AssetManager.Instance.GroundMaterial;
        }

        public void DoMove(float percentage)
        {
            transform.localPosition = Vector3.Lerp(firstLocalPosition, lastLocalPosition, percentage);
        }
        
        public int GetCounter()
        {
            return _counter;
        }

        public void SetDefault()
        {
            transform.localPosition = firstLocalPosition;
            _textMesh.enabled = true;
            _textMesh.text = " 0 / " + _targetCounter.ToString();
            // _textMesh.ForceMeshUpdate();
            Debug.Log(gameObject.name + "  : "+ _textMesh.text);
            _meshRenderer.material = AssetManager.Instance.PickerMaterial;
        }
        
        public void SetCollectablesCount(int count)
        {
            pickerCollectablesCount = count;
        }

        private void OnCollisionEnter(Collision other)
        {
            var picker = other.gameObject.GetComponent<CollectableBase>();

            if (picker != null)
            {
                picker.Deactivate();
                float time = 1.5f * _counter / pickerCollectablesCount;
                DOVirtual.Float(_counter, ++_counter, time, value =>
                 {
                     _textMesh.text = Mathf.RoundToInt(value) + "/" + _targetCounter;
                 });
                // _textMesh.text = (++_counter).ToString() + "/" + _targetCounter;
                Debug.Log(gameObject.name + _textMesh.text);
            }
        }
    }
}
