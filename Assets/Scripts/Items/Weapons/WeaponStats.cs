using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons 
{
    public class WeaponStats : Item
    {
        [Header("Core Stats")]
        [SerializeField] private float damage;
        [SerializeField] private float accuracy;
        [SerializeField] private float reloadSpeed;
        [SerializeField] private int modSlots;
    }
}
