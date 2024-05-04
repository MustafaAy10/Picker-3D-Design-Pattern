namespace DevelopmentKit.Base.Pattern.ObjectPool
{
    using DevelopmentKit.Base.Services;
    using Game.LevelSystem;
    using Game.PlatformSystem.Base;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class PlatformPool
    {
        protected List<PlatformBase> pool;
        private List<PlatformBase> sourcePrefabs;
        private string sourceObjectPath;

        private IServiceLocator serviceLocator;
        private PlatformFactory platformFactory;

        private PlatformPool() { }

        public PlatformPool(string sourceObjectPath, IServiceLocator serviceLocator, PlatformFactory platformFactory)
        {
            this.serviceLocator = serviceLocator;
            this.platformFactory = platformFactory;
            this.sourceObjectPath = sourceObjectPath;
            sourcePrefabs = Resources.LoadAll<PlatformBase>(sourceObjectPath).ToList();
            pool = new List<PlatformBase>();
        }

        public void Add(PlatformBase poolable)
        {
            poolable.Deactivate();
            pool.Add(poolable);
        }

        public PlatformBase Get(PlatformType platformType)
        {
            var pooledObject = pool.FirstOrDefault(p => p.PlatformType == platformType);
            if (pooledObject == null)
            {
                pooledObject = GenerateObject(platformType);
            }
            
            pooledObject.Activate();
            pool.Remove(pooledObject);

            return pooledObject;
        }

        private PlatformBase GenerateObject(PlatformType platformType)
        {
            
            var sourceObject = sourcePrefabs.FirstOrDefault(x => x.PlatformType == platformType);
            var poolableObject = GameObject.Instantiate(sourceObject);

            if (poolableObject == null)
            {
                Debug.LogError("Source prefab does not contain any IPoolable");
                return null;
            }

            poolableObject.Initialize();

            if (platformType == PlatformType.CHECKPOINT)
            {
                (poolableObject as CheckPoint).Inject(serviceLocator, platformFactory);
            }

            poolableObject.Deactivate();

            pool.Add(poolableObject);

            return poolableObject;
        }

    }
}


