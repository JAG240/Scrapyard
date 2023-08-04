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

        protected virtual void Start()
        {
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
        }

        private void Restore()
        {
            health = maxHealth;
            stamina = maxStamina;
        }
    }
}
