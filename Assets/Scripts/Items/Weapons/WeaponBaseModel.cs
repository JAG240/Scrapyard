using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Scrapyard.items.weapons
{
    public class WeaponBaseModel : MonoBehaviour
    {
        [SerializeField] private List<WeaponModelPositions> partPositions = new List<WeaponModelPositions>();

        public void EquipPart(GameObject obj, WeaponPartType type)
        {
            foreach(WeaponModelPositions position in partPositions)
            {
                if(position.type == type)
                {
                    obj.transform.position = position.position.position;
                    break;
                }
            }

            obj.transform.parent = transform;
        }
    }

    [Serializable]
    public class WeaponModelPositions
    {
        [field: SerializeField] public WeaponPartType type { get; private set; }
        [field: SerializeField] public Transform position { get; private set; }
    }
}
