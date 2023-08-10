using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scrapyard.core.character
{
    public class PlayerController : Player
    {
        [SerializeField] private bool _allowMovement = true;

        private CharacterController _characterController;

        private Vector2 _rawMove;
        private Vector2 _controllerMove;

        private Vector3 _mousePos;
        private float _timeCount = 0.0f;

        private bool _inConsole = false;

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
            if (Input.mousePosition == _mousePos && overrideLookTo == null || !_allowMovement)
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

            Vector3 rawCamForward = Camera.main.transform.forward;
            Vector3 cameraForward = Vector3.Normalize(new Vector3(rawCamForward.x, 0f, rawCamForward.z));

            dir = Camera.main.transform.right * dir.x + cameraForward * dir.z;
            _characterController.Move(dir * speed * Time.deltaTime);
        }

        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            _rawMove = input;

            if (_allowMovement)
                _controllerMove = input;
        }

        public void OnConsole()
        {
            _inConsole = !_inConsole;

            if (_inConsole)
                DisableMovement();
            else
                EnableMovement();
        }

        public void OnFire(InputValue value)
        {
            //1f for pressed and 0f for released
            //Debug.Log(value.Get<float>());
        }

        public void DisableMovement()
        {
            _controllerMove = Vector2.zero;
            _allowMovement = false;
        }

        public void EnableMovement()
        {
            _controllerMove = _rawMove;
            _allowMovement = true;
        }
    }
}
