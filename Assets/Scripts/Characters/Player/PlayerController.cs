using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scrapyard.core.character
{
    public class PlayerController : Player
    {
        [SerializeField] private bool allowMovement = true;

        private CharacterController _characterController;

        private Vector2 _rawMove;
        private Vector2 _controllerMove;

        private Vector3 _mousePos;
        private float _timeCount = 0.0f;

        public Transform overrideLookTo;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Rotate()
        {
            if (Input.mousePosition == _mousePos && overrideLookTo == null)
                return;

            _mousePos = Input.mousePosition;


            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 100))
            {
                float angle = overrideLookTo == null ? AngleBetweenPoints(hit.point, transform.position) : AngleBetweenPoints(overrideLookTo.position, transform.position);
                Quaternion dest = Quaternion.Euler(new Vector3(0f, angle, 0f));
                transform.rotation = Quaternion.Slerp(transform.rotation, dest, _timeCount);
                _timeCount = _timeCount + Time.deltaTime;
            }
        }

        private float AngleBetweenPoints(Vector3 a, Vector3 b) 
        { 
            return Mathf.Atan2(a.x - b.x, a.z - b.z) * Mathf.Rad2Deg; 
        }

        private void Move()
        {
            if (!_characterController.enabled)
                return;

            Vector3 dir = new Vector3(_controllerMove.x, 0, _controllerMove.y).normalized;
            _characterController.Move(dir * speed * Time.deltaTime);
        }

        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            _rawMove = input;

            if (allowMovement)
                _controllerMove = input;
        }

        public void DisableMovement()
        {
            _controllerMove = Vector2.zero;
            allowMovement = false;
        }

        public void EnableMovement()
        {
            _controllerMove = _rawMove;
            allowMovement = true;
        }
    }
}
