using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services.modelbuilders
{
    [Serializable]
    public abstract class ModelBuilder
    {
        public WeaponType type { get; protected set; }

        public abstract GameObject Build(WeaponBuilder weaponBuilder, Weapon weapon);
        public ModelBuilder()
        {

        }
    }
}
