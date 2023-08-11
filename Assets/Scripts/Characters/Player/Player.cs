using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.services; 

namespace Scrapyard.core.character
{
    public abstract class Player : Character
    {
        protected CameraFollow _cameraFollow;

        override protected void Start()
        {
            base.Start();

            _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }
    }
}
