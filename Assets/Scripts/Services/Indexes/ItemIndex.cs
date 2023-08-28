using System;
using Scrapyard.items.weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items;

namespace Scrapyard.services
{
    public class ItemIndex : Service
    {
        private Dictionary<string, Item> Items = new Dictionary<string, Item>();
        private Dictionary<string, WeaponBase> WeaponBases = new Dictionary<string, WeaponBase>();
        private Dictionary<string, WeaponPart> WeaponParts = new Dictionary<string, WeaponPart>();
        private Dictionary<string, BulletBase> Bullets = new Dictionary<string, BulletBase>();


        private void Index()
        {
            object[] items = Resources.LoadAll("Items");
            LoadItemDictionary<Item>(items, Items);

            object[] bases = Resources.LoadAll("WeaponBases");
            LoadItemDictionary<WeaponBase>(bases, WeaponBases);

            object[] parts = Resources.LoadAll("WeaponParts");
            LoadItemDictionary<WeaponPart>(parts, WeaponParts);

            object[] bullets = Resources.LoadAll("Bullets");
            LoadItemDictionary<BulletBase>(bullets, Bullets);
        }

        private void LoadItemDictionary<T>(object[] objs, Dictionary<string, T> list)
        {
            foreach (object obj in objs)
            {
                T o = (T)obj;
                ScriptableObject scrO = (ScriptableObject)obj;
                list.Add(scrO.name, o);
            }
        }

        public T Get<T>(string name)
        {
            if (typeof(T) == typeof(Item))
            {
                Item obj;
                Items.TryGetValue(name, out obj);
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            else if (typeof(T) == typeof(WeaponBase))
            {
                WeaponBase obj;
                WeaponBases.TryGetValue(name, out obj);
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            else if (typeof(T) == typeof(WeaponPart))
            {
                WeaponPart obj;
                WeaponParts.TryGetValue(name, out obj);
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            else if(typeof(T) == typeof(BulletBase))
            {
                BulletBase obj;
                Bullets.TryGetValue(name, out obj);
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            return (T) Convert.ChangeType(null, typeof(T));
        }

        protected override void Register()
        {
            Index();
            ServiceLocator.Register<ItemIndex>(this);
            Registered = true;
        }
    }
}
