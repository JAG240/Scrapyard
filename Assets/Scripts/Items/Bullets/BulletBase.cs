using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons
{
    [CreateAssetMenu(fileName = "BulletBase", menuName = "Items/New Bullet Base", order = 3)]
    public class BulletBase : ScriptableObject
    {
        [field: SerializeField] public AmmoType type { get; private set; }
        [field: SerializeField] public GameObject bullet { get; private set; }
    }
}
