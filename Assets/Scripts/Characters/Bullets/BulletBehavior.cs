using Scrapyard.core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons 
{
    public class BulletBehavior : Teamable
    {
        Rigidbody RB;
        bool notGround = true;

        private void Start()
        {
            RB = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(notGround)
                Debug.Log(RB.velocity.magnitude);
        }

        private void OnCollisionEnter(Collision collision)
        {
            notGround = false;
        }
    }

}
