using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons 
{
    public class WeaponStats : Item
    {
        [field:SerializeField] public float damage { get; private set; }
        [field:SerializeField] public float accuracy { get; private set; }
        [field:SerializeField] public float reloadSpeed { get; private set; }
        [field:SerializeField] public int modSlots { get; private set; }
    }
}
