using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentKit.Base.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private Dictionary<string, object> services = new Dictionary<string, object>();

        public void Add(string key, object value)
        {
            if (services.ContainsKey(key))
            {
                Debug.LogError("[ServiceLocator] Add() dublicate key error... it is not allowed...");
                return;
            }
            services[key] = value;
        }

        public T Get<T>(string key)
        {
            object value;
            if (services.TryGetValue(key, out value))
            {
                return (T)value;
            }
            else
            {
                Debug.LogError("[ServiceLocator] Get() key not found... There is no key paired with a value in the services...");
                // throw new KeyNotFoundException();
                return default(T);
            }
        }

        public void Remove(string key)
        {
            if (!services.ContainsKey(key))
            {
                Debug.LogError("[ServiceLocator] Remove() key not found in servicelocator... Be careful...");
                return;
            }

            services.Remove(key);
        }
    }
}