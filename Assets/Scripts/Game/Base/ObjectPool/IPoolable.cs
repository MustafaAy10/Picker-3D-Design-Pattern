using System.Collections;
using System.Collections.Generic;
using Picker.Services;
using UnityEngine;

namespace DevelopmentKit.Base.Pattern.ObjectPool 
{
    public interface IPoolable
    {
        void Activate();
        void Deactivate();
        void Initialize();

    }

}

