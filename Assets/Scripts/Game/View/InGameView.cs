using System.Collections.Generic;
using System.Linq;
using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Services;
using DG.Tweening;
 
using Game.PickerSystem.Base;
using Game.View.Helpers;
using Picker.Services;
using UnityEngine;
 

namespace Picker.View
{

    public class InGameView : ViewBase, IDestructible
    {
        // private InGameMessageBroadcaster inGameMessageBroadcaster;
        private PickerBase _pickerBase;
        private List<CheckItem> _checkItems;
        private PointClaim _pointClaim;
        private int _point;

        public override ViewMenu viewMenu => ViewMenu.InGameView;

        public override void Initialize(IServiceLocator serviceLocator)
        {
            base.Initialize(serviceLocator);
            Activate();

            _checkItems = GetComponentsInChildren<CheckItem>().ToList();
            _pointClaim = GetComponentInChildren<PointClaim>();
            foreach (var checkItem in _checkItems)
            {
                checkItem.Initialize();
            }
            _pointClaim.Initialize();
        }

        public void ChangePoint(int total)
        {
            var temp = _point;
            _point += total;
            DOVirtual.Float(temp, _point, 2f, value =>
            {
                _pointClaim.ChangePoint(Mathf.RoundToInt(value));
            });
        }

        private void DeActivateCheckPointItem()
        {
            foreach (var item in _checkItems)
            {
                item.Deactive();
            }
        }

        public void ActiveCheckPointItem()
        {
            foreach (var item in _checkItems)
            {
                if (!item.IsActive)
                {
                    item.Active();
                    break;
                }
            }
        }

        public void OnDestruct()
        {
            DeActivateCheckPointItem();
            _pointClaim.ChangePoint(0);
            _point = 0;

        }
    }
}
