using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.core.character
{
    public abstract class Character : Teamable
    {
        [SerializeField] private CharacterAttributes characterAttributes;

        public float maxHealth { get; private set; }
        public float maxStamina { get; private set; }
        public float bluntDefense { get; private set; }
        public float sharpDefense { get; private set; }
        public float speed { get; private set; }

        public float health { get; private set; } = 1;
        public float stamina { get; private set; } = 1;
        public float dodgeTime { get; private set; }
        public CharacterInventory inventory { get; private set; }
        [SerializeField] protected float dodgeSpeed = 30f;
        [SerializeField] private Transform handPos;
        [SerializeField] private Transform holsterPos;

        protected virtual void Start()
        {
            inventory = new CharacterInventory(12, handPos, holsterPos);
            UpdateStats();
            Restore();
        }

        private void UpdateStats()
        {
            maxHealth = characterAttributes.maxHealth;
            maxStamina = characterAttributes.maxStamina;
            bluntDefense = characterAttributes.bluntDefense;
            sharpDefense = characterAttributes.sharpDefense;
            speed = characterAttributes.speed;
            dodgeTime = characterAttributes.dodgeTime;
        }

        private void Restore()
        {
            health = maxHealth;
            stamina = maxStamina;
        }
    }
}
