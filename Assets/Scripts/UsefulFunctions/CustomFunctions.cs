using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public static class CustomFunctions
{
    public static readonly float Tau = 2 * Mathf.PI;

    public static float Lerp(float a, float b, float t)
    {
        return (1.0f - t) * a + b * t;
    }

    public static float InvLerp(float a, float b, float v)
    {
        return (v - a) / (b - a);
    }

    public static float remap(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = InvLerp(iMin, iMax, v);
        return Lerp(oMin, oMax, t);
    }

    public static float AngleBetweenPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.x - b.x, a.z - b.z) * Mathf.Rad2Deg;
    }

    public static List<T> CreateInstances<T>()
    {
        List<T> objs = new List<T>();

        foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes().Where(targetType => targetType.IsClass && !targetType.IsAbstract && targetType.IsSubclassOf(typeof(T))))
        {
            objs.Add((T)Activator.CreateInstance(type));
        }

        return objs;
    }
}
