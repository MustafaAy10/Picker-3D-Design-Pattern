namespace DevelopmentKit.Base.Pattern.ObjectPool
{
    using Game.LevelSystem.BallPacks;
    using Game.PlatformSystem.Base;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class BallPackPool
    {
        protected List<BallPackBase> pool;
        private List<BallPackBase> sourcePrefabs;
        private string sourceObjectPath;

        private BallPackPool() { }

        public BallPackPool(string sourceObjectPath)
        {
            this.sourceObjectPath = sourceObjectPath;
            sourcePrefabs = Resources.LoadAll<BallPackBase>(sourceObjectPath).ToList();
            pool = new List<BallPackBase>();
        }

        public void Add(BallPackBase poolable)
        {
            poolable.Deactivate();
            pool.Add(poolable);
        }

        public BallPackBase Get(BallPackType type)
        {
            var pooledObject = pool.FirstOrDefault(x => x.BallPackType == type);
            if (pooledObject == null)
            {
                pooledObject = GenerateObject(type);
            }

            pooledObject.Activate();
            pool.Remove(pooledObject);

            return pooledObject;
        }

        private BallPackBase GenerateObject(BallPackType type)
        {

            var sourceObject = sourcePrefabs.FirstOrDefault(x => x.BallPackType == type);
            var poolableObject = GameObject.Instantiate(sourceObject);

            if (poolableObject == null)
            {
                Debug.LogError("Source prefab does not contain any IPoolable");
                return null;
            }

            poolableObject.Initialize();
            poolableObject.Deactivate();

            pool.Add(poolableObject);

            return poolableObject;
        }

    }
}


