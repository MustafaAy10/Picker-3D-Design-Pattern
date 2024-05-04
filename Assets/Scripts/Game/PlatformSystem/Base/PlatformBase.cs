using DevelopmentKit.Base.Pattern.ObjectPool;
using DevelopmentKit.Base.Services;
 
using UnityEngine;

namespace Game.PlatformSystem.Base
{
    public abstract class PlatformBase : MonoBehaviour, IPoolable
    {
        public abstract PlatformType PlatformType { get; }
        public bool IsActive;

        public virtual void Initialize()
        {
            IsActive = false;
        }
        
        public virtual void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }
        
    }

    public enum PlatformType
    {
        NORMAL,
        CHECKPOINT,
        FINAL
    }
}
