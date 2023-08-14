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

        public virtual void Init(float range, Vector3 dir, float speed)
        {
            this.range = range;
            this.dir = dir;
            this.speed = speed;
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

            if(Vector3.Distance(transform.position, startPos) >= range)
            {
                atRange = true;
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            
        }
    }

}
