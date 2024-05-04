 
using Game.Managers;
using Game.PickerSystem.Base;
using Game.PlatformSystem.Base;
using UnityEngine;
 
using Zenject;

namespace Game.LevelSystem
{
    public class LevelGenerator 
    {
        private PlatformFactory _platformFactory;
        private PickerBase _pickerBase;
        private int _levelIndex;
        private Vector3 _pickerStartPosition;
        
        
        public void Initialize(PickerBase pickerBase, PlatformFactory platformFactory)
        {
            _platformFactory = platformFactory;
            _pickerBase = pickerBase;
            _pickerStartPosition = new Vector3(0,0.6f,2.5f);
            _levelIndex = 1;
            
        }

        public void IncreaseLevel()
        {
            _levelIndex++;
        }

        public void GenerateLevel()
        {
            var levelData = AssetManager.Instance.LoadLevel(_levelIndex);
            if (levelData == null)
            {
                _levelIndex = 1;
                levelData = AssetManager.Instance.LoadLevel(_levelIndex);
            }
            
            Debug.Log("[LevelGenerator] levelIndex: " + _levelIndex);

            var platformList = levelData.PlatformDatas;
            foreach (var platformData in platformList)
            {
                var platform = _platformFactory.GetAvailablePlatform(platformData.PlatformType);
                platform.transform.position = platformData.Position;
                

                if (platformData.PlatformType == PlatformType.CHECKPOINT)
                    (platform as CheckPoint).SetTarget(platformData.CheckPointCount);
                
            }

            var ballPacks = levelData.BallPackDatas;
            foreach (var ballPack in ballPacks)
            {
                var ball = _platformFactory.GetAvailableBallPack(ballPack.BallPackType);
                ball.transform.position = ballPack.Position;
            }
            
            _pickerBase.transform.position = _pickerStartPosition;
        }
    }
}
