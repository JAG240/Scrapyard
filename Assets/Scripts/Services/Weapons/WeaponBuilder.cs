using System;
using System.Collections.Generic;
using Scrapyard.items.weapons;
using Scrapyard;
using Scrapyard.items;
using UnityEngine;
using System.Reflection;
using Scrapyard.services.modelbuilders;
using System.Linq;

namespace Scrapyard.services 
{
    public class WeaponBuilder : Service
    {
        [field:SerializeField] public GameObject defaultGunBase {get; private set;}
        [field:SerializeField] public GameObject defaultGrip {get; private set;}
        [field:SerializeField] public GameObject defaultBarrel {get; private set;}

        private List<ModelBuilder> modelBuilders = new List<ModelBuilder>();

        protected override void Register()
        {
            modelBuilders = CustomFunctions.CreateInstances<ModelBuilder>();
            ServiceLocator.Register<WeaponBuilder>(this);
            Registered = true;
        }

        public GameObject BuildWeaponModel(Weapon weapon)
        {
            foreach(ModelBuilder builder in modelBuilders)
            {
                if(weapon.weaponBase.type == builder.type)
                {
                    return builder.Build(this, weapon);
                }
            }

            return null;
        }

        public void DestroyModel(Weapon weapon)
        {
            if(weapon != null && weapon.model != null)
                Destroy(weapon.model);
        }


        public Weapon BuildWeapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            if (!Validate(weaponBase, weaponParts))
                return null;

            Assembly asm = typeof(Weapon).Assembly;
            Type type = asm.GetType("Scrapyard.items.weapons." + weaponBase.type.ToString());
            Weapon weapon = (Weapon)Activator.CreateInstance(type, weaponBase, weaponParts);
            weapon.bullet = ServiceLocator.Resolve<ItemIndex>().Get<BulletBase>(weaponBase.ammoType.ToString().ToLower()).bullet;
            return weapon;
        }

        public bool Validate(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            if (weaponBase == null)
                return false;

            List<WeaponPartType> partTypes = new List<WeaponPartType>();

            foreach(WeaponPart part in weaponParts)
            {
                if (!part.baseTypes.Contains(weaponBase.baseType) || partTypes.Contains(part.type))
                    return false;

                partTypes.Add(part.type);
            }

            return true;
        }
    }
}

