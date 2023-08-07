using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons
{
    [CreateAssetMenu(fileName = "WeaponBase", menuName = "Items/New Weapon Base", order = 2)]
    public class WeaponBase: WeaponStats
    {
        [Header("Base Stats")]
        [SerializeField] private WeaponBaseType type;

    }
}
