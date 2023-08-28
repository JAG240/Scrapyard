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

        private float _timeCount = 0.0f;

        private bool _inConsole = false;
        private bool _inInventory = false;

        public Transform overrideLookTo;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        protected override void Update()
        {
            base.Update();

            Move();
            ApplyGravity();
            Rotate();
            UpdateBottomRotation(_controllerMove);
            RotateTire(_doding, _controllerMove);
            UpdateLean(_controllerMove);
        }

        private void Rotate()
        {
            if (!_allowMovement || _enableLook)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out _mouseLastHit, 100))
            {
                float angle = overrideLookTo == null ? CustomFunctions.AngleBetweenPoints(_mouseLastHit.point, transform.position) : CustomFunctions.AngleBetweenPoints(overrideLookTo.position, transform.position);
                Quaternion dest = Quaternion.Euler(new Vector3(0f, angle, 0f));
                transform.rotation = Quaternion.Slerp(transform.rotation, dest, _timeCount);
                _timeCount = _timeCount + Time.deltaTime;
            }
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

        private void ApplyGravity()
        {
            float r = _characterController.radius * 0.9f;
            Vector3 pos = transform.position + Vector3.down * (r * 0.2f);
            pos.y -= transform.localScale.y / 2;

            if (Physics.CheckSphere(pos, r, LayerMask.GetMask("Ground")))
                return;

            _characterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
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
                float smoothSpeed = dodgeSpeed - dodgeSpeed * (Mathf.Abs(dodgeTimer - (dodgeTime / 2)) * (2 / dodgeTime));

                dodgeTimer += Time.deltaTime;
                _characterController.Move(dir * smoothSpeed * Time.deltaTime);
                yield return null;
            }

            _doding = false;
        }

        public void OnConsole()
        {
            if (_inInventory)
                return;

            _inConsole = !_inConsole;

            if (_inConsole)
                DisableMovement();
            else
                EnableMovement();
        }

        public void OnInventoryToggle()
        {
            if (_inConsole)
                return;

            _inInventory = !_inInventory;

            if (_inInventory)
                DisableMovement();
            else
                EnableMovement();
        }

        public void OnFire(InputValue value)
        {
            if (!_allowMovement)
                return;

            float state = value.Get<float>();
            Weapon weapon = inventory.equippedWeapons[0];

            if (weapon == null)
                return;

            Vector3 dir = _mouseLastHit.point - weapon.end.position;
            dir.y = 0;
            dir.Normalize();

            if (state == 1)
                Shoot(weapon, weapon.end.position, dir);
        }

        public void OnReload()
        {
            Weapon weapon = inventory.equippedWeapons[0];

            if (weapon == null)
                return;

            weapon.Reload();
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
