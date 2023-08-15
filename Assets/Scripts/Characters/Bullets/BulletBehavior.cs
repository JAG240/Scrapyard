using Scrapyard.core;
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

        public virtual void Init(Weapon weapon, Vector3 dir)
        {
            range = weapon.range;
            speed = weapon.bulletSpeed;
            this.dir = ApplyBulletSpread(dir, weapon.accuracy);
        }

        private Vector3 ApplyBulletSpread(Vector3 dir, float accuracy)
        {
            float spread = CustomFunctions.remap(0, 10f, 0.5f, 0f, accuracy);
            float offsetz = dir.z + Random.Range(-spread, spread);
            float offsetx = dir.x + Random.Range(-spread, spread);

            return new Vector3(offsetx, dir.y, offsetz).normalized;
        }

        protected virtual void Start()
        {
            startPos = transform.position;
            rigidBody = GetComponent<Rigidbody>();
        }

        protected virtual void Update()
        {
            if(!atRange)
                rigidBody.velocity = dir * speed;

            if(Vector3.Distance(transform.position, startPos) >= range && !atRange)
            {
                float randDropoff = Random.Range(4f, 6f);
                rigidBody.velocity = dir * (speed / randDropoff);
                atRange = true;
                StartCoroutine(StartDespawn());
            }
        }

        protected virtual IEnumerator StartDespawn()
        {
            float timer = 0f;
            while(timer < 10f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0f;
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

        protected virtual void OnCollisionEnter(Collision collision)
        {
            
        }
    }

}
