using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Scrapyard.services 
{
    public static class ServiceLocator 
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        public static void Register<T>(object serviceInstance)
        {
            if (Services.ContainsKey(typeof(T)))
                Debug.LogError($"Attempting to register more than one service type: {typeof(T)}");

            Services[typeof(T)] = serviceInstance;
        }

        public static T Resolve<T>()
        {
            return (T)Services[typeof(T)];
        }

        public static void Clear()
        {
            Services.Clear();
        }
    }
}
