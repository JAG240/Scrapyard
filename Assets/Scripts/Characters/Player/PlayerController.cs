using Scrapyard.items.weapons;
using Scrapyard.services;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scrapyard.core.character
{
    public class PlayerController : Player
    {
        private bool _allowMovement = true;
        private bool _doding = false;

        private CharacterController _characterController;

        private Vector2 _rawMove;
        private Vector2 _controllerMove;
        private Vector3 _currentDir;

        private bool _enableLook = false;
        private RaycastHit _mouseLastHit;

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
            if (Input.mousePosition == _mousePos && overrideLookTo == null || !_allowMovement || _enableLook)
                return;

            _mousePos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out _mouseLastHit, 100))
            {
                float angle = overrideLookTo == null ? AngleBetweenPoints(_mouseLastHit.point, transform.position) : AngleBetweenPoints(overrideLookTo.position, transform.position);
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
            if (!_characterController.enabled || _doding)
                return;

             _currentDir = new Vector3(_controllerMove.x, 0, _controllerMove.y).normalized;

            Vector3 rawCamForward = Camera.main.transform.forward;
            Vector3 cameraForward = Vector3.Normalize(new Vector3(rawCamForward.x, 0f, rawCamForward.z));

            _currentDir = Vector3.Normalize(Camera.main.transform.right * _currentDir.x + cameraForward * _currentDir.z);
            _characterController.Move(_currentDir * speed * Time.deltaTime);
        }

        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            _rawMove = input;

            if (_allowMovement)
                _controllerMove = input;
        }

        public void OnDodge()
        {
            if (_doding || _currentDir == Vector3.zero)
                return;

            _doding = true;
            StartCoroutine(Dodge());
        }

        private IEnumerator Dodge()
        {
            Vector3 dir = _currentDir;
            float dodgeTimer = 0f;

            while(dodgeTimer < dodgeTime)
            {
                dodgeTimer += Time.deltaTime;
                _characterController.Move(dir * dodgeSpeed * Time.deltaTime);
                yield return null;
            }

            _doding = false;
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
            if (!_allowMovement)
                return;

            //TODO:Handle melee attacks, there will be no bullets or ends

            float state = value.Get<float>();
            Weapon weapon = inventory.equippedWeapons[0];

            if (weapon == null)
                return;

            Vector3 dir = _mouseLastHit.point - transform.position;
            dir.y = 0;
            dir.Normalize();

            if (state == 1)
                Shoot(weapon.bullet, weapon.end.position, dir * weapon.range);
        }

        public void OnEnableLook(InputValue value)
        {
            float state = value.Get<float>();

            if (state > 0)
            {
                _enableLook = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                _enableLook = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void OnLook(InputValue value)
        {
            if(!_allowMovement || !_enableLook)
                return;

            Vector2 lookDelta = value.Get<Vector2>();
            float updateRot = lookDelta.x * Time.deltaTime * _cameraFollow.cameraRotationSpeed;
            _cameraFollow.rotation = Mathf.Clamp(_cameraFollow.rotation - updateRot, 0.55f, 0.95f);
        }

        public void OnZoom(InputValue value)
        {
            if (!_allowMovement || !_enableLook)
                return;

            Vector2 zoomDelta = value.Get<Vector2>().normalized;
            float updateZoom = zoomDelta.y * Time.deltaTime * _cameraFollow.cameraZoomSpeed;
            _cameraFollow._zoom = Mathf.Clamp(_cameraFollow._zoom - updateZoom, 8f, 13f);
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

        private void OnBecameInvisible()
        {
            if (_allowMovement)
                _cameraFollow.ReturnCameraToPlayer();
        }
    }
}
