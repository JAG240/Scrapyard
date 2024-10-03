using Codice.Client.BaseCommands.CheckIn.Progress;
using Scrapyard.core;
using Scrapyard.core.character;
using Scrapyard.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons
{
    public class MeleeBehavior : BulletBehavior
    {


        public override void Init(Weapon weapon, Vector3 dir, Team team)
        {
            this.team = team;

            sharpDamage = weapon.sharpDamage;
            bluntDamage = weapon.bluntDamage;

            transform.parent = weapon.model.transform;
        }

        protected override void Start()
        {

        }

        protected override void Update()
        {

        }

        protected override void OnCollisionEnter(Collision collision)
        {
            //Melee is trigger based
        }

        private void OnTriggerEnter(Collider other)
        {
            Character character = other.gameObject.GetComponent<Character>();

            if (character == null || character.team == team)
                return;

            float damage = character.TakeDamage(sharpDamage, bluntDamage);
            ServiceLocator.Resolve<UIManager>().splashController.NewSplash(character.transform.position, damage);

            Destroy(this.gameObject);
        }
    }
}
