using Scrapyard.core;
using Scrapyard.core.character;
using Scrapyard.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons 
{
    public class BulletBehavior : Teamable
    {
        private Rigidbody rigidBody;
        private Vector3 startPos;
        private float range;
        private Vector3 dir;
        private float speed;
        private bool atRange = false;
        private float sharpDamage;
        private float bluntDamage;

        private float maxAccuracy = 10f;
        private float maxSpread = 0.35f;

        [SerializeField] private TrailRenderer trail;

        public virtual void Init(Weapon weapon, Vector3 dir, Team team)
        {
            gameObject.layer = team == Team.player ? LayerMask.NameToLayer("Player Bullet") : LayerMask.NameToLayer("Enemy Bullet");

            range = weapon.range;
            speed = weapon.bulletSpeed;

            sharpDamage = weapon.sharpDamage;
            bluntDamage = weapon.bluntDamage;

            this.team = team;
            this.dir = ApplyBulletSpread(dir, weapon.accuracy);
            LookAtDir();
        }

        private void LookAtDir()
        {
            float angle = CustomFunctions.AngleBetweenPoints(transform.position, dir + transform.position);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        private Vector3 ApplyBulletSpread(Vector3 dir, float accuracy)
        {
            float spread = CustomFunctions.remap(0, maxAccuracy, maxSpread, 0f, accuracy);
            float offsetz = dir.z + Random.Range(-spread, spread);
            float offsetx = dir.x + Random.Range(-spread, spread);

            return new Vector3(offsetx, dir.y, offsetz).normalized;
        }

        protected virtual void Start()
        {
            trail = GetComponentInChildren<TrailRenderer>();
            startPos = transform.position;
            rigidBody = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            if(!atRange)
                rigidBody.velocity = dir * speed;

            if(Vector3.Distance(transform.position, startPos) >= range && !atRange)
            {
                float randDropoff = Random.Range(2f, 3f);
                rigidBody.velocity = dir * (speed / randDropoff);
                StopBullet();
            }
        }

        protected virtual IEnumerator StartDespawn()
        {
            yield return new WaitForSeconds(10f);

            float timer = 0f;
            while(timer < 1.5f)
            {
                timer += Time.deltaTime;
                float t = timer / 1.5f;
                float newX = Mathf.Lerp(transform.localScale.x, 0f, t);
                float newY = Mathf.Lerp(transform.localScale.y, 0f, t);
                float newZ = Mathf.Lerp(transform.localScale.z, 0f, t);
                transform.localScale = new Vector3(newX, newY, newZ);
                yield return null;
            }

            Destroy(gameObject);
        }

        private void StopBullet()
        {
            trail.emitting = false;
            atRange = true;
            StartCoroutine(StartDespawn());
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            if (atRange)
                return;

            StopBullet();

            Character character = collision.gameObject.GetComponent<Character>();

            if (character == null)
                return;

            float damage = character.TakeDamage(sharpDamage, bluntDamage);
            ServiceLocator.Resolve<UIManager>().splashController.NewSplash(character.transform.position, damage);
        }
    }

}
