using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.services; 

namespace Scrapyard.core.character
{
    public abstract class Player : Character
    {
        [SerializeField] private GameObject characterBottom;
        [SerializeField] private GameObject Tire;
        [SerializeField] private float tireRotSpeed;
        [SerializeField] private float leanAngle;
        protected CameraFollow _cameraFollow;

        private float previousAngle;

        override protected void Start()
        {
            base.Start();

            _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        }

        protected void UpdateBottomRotation(Vector2 direction)
        {
            Vector3 dir = new Vector3(direction.x, 0f, direction.y);
            float angle = direction == Vector2.zero ? previousAngle : CustomFunctions.AngleBetweenPoints(Vector3.zero, dir);
            characterBottom.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            previousAngle = angle;
        }

        protected void RotateTire(bool isDodging, Vector2 dir)
        {
            if (dir == Vector2.zero)
                return;

            float rotSpeed = isDodging ? speed : dodgeSpeed;
            Tire.transform.RotateAround(Tire.transform.position, -Tire.transform.up, rotSpeed * Time.deltaTime * tireRotSpeed);
        }

        protected void UpdateLean(Vector2 dir)
        {
            Vector3 lean = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);

            if (dir.y > 0)
                lean.x = leanAngle;
            else if (dir.y < 0)
                lean.x = -leanAngle;

            if (dir.x > 0f)
                lean.z = -leanAngle;
            else if (dir.x < 0f)
                lean.z = leanAngle;

            transform.rotation = Quaternion.Euler(lean);
        }
    }
}
