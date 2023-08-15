using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

namespace Scrapyard.items.weapons 
{
    public class WeaponStats : Item
    {
        [field:SerializeField] public float bluntDamage { get; private set; }
        [field:SerializeField] public float sharpDamage { get; private set; }
        [field:SerializeField] public float accuracy { get; private set; }
        [field:SerializeField] public float reloadSpeed { get; private set; }
        [field:SerializeField] public int modSlots { get; private set; }
        [field: SerializeField] public float range { get; private set; }
        [field: SerializeField] public float bulletSpeed { get; private set; }
        [field: SerializeField] public float firerate { get; private set; }
        [field: SerializeField] public int magSize { get; private set; }

        public Dictionary<string, float> GetStats()
        {
            Dictionary<string, float> stats = new Dictionary<string, float>();

            PropertyInfo[] props = GetType().GetProperties();
            foreach(PropertyInfo prop in props)
            {
                if (prop.PropertyType == typeof(float) || prop.PropertyType == typeof(int))
                    stats.Add(prop.Name, (float) Convert.ChangeType(prop.GetValue(this), typeof(float)));
            }

            return stats;
        }
    }
}
