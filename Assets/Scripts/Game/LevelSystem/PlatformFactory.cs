using System.Collections.Generic;
using System.Linq;
using DevelopmentKit.Base.Object;
using DevelopmentKit.Base.Pattern.ObjectPool;
using DevelopmentKit.Base.Services;
using Game.LevelSystem.BallPacks;
using Game.Managers;
using Game.PlatformSystem.Base;
using Picker.Services;
using UnityEngine;

namespace Game.LevelSystem
{
    public class PlatformFactory : IUpdatable, IDestructible
    {
        private List<PlatformBase> activePlatforms = new List<PlatformBase>();
        private List<CheckPoint> activeCheckPoints = new List<CheckPoint>();

        private List<BallPackBase> activeBallPacks = new List<BallPackBase>();

        private const string BALLPACK_PATH = "BallPacks";
        private const string PLATFORM_PATH = "PlatformPrefabs";

        private IServiceLocator serviceLocator;

        private PlatformPool platformPool;
        private BallPackPool ballPackPool;

        public void Initialize(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            platformPool = new PlatformPool(PLATFORM_PATH, serviceLocator, this);
            ballPackPool = new BallPackPool(BALLPACK_PATH);
        }

        public void CallUpdate()
        {
            for (int i = 0; i < activeCheckPoints.Count; i++)
            {
                activeCheckPoints[i].CallUpdate();
            }
        }

        public void AddActiveCheckPoint(CheckPoint checkPoint)
        {
            if (!activeCheckPoints.Contains(checkPoint))
                activeCheckPoints.Add(checkPoint);
        }

        public void RemoveActiveCheckPoint(CheckPoint checkPoint)
        {
            activeCheckPoints.Remove(checkPoint);
        }

        public PlatformBase GetAvailablePlatform(PlatformType platformType)
        {
            var platform = platformPool.Get(platformType);

            activePlatforms.Add(platform);

            return platform;
        }

        public BallPackBase GetAvailableBallPack(BallPackType ballPackType)
        {
            var ballPack = ballPackPool.Get(ballPackType);

            activeBallPacks.Add(ballPack);

            return ballPack;
        }


        public void OnDestruct()
        {
            foreach (var platform in activePlatforms)
            {
                platformPool.Add(platform);
            }

            foreach (var ball in activeBallPacks)
            {
                ballPackPool.Add(ball);
            }

            activePlatforms.Clear();

            activeBallPacks.Clear();

            activeCheckPoints.Clear();
        }

    }
}
