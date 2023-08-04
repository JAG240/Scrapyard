using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scrapyard.core.character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool allowMovement = true;

        private CharacterController characterController;
        private Player player;

        private Vector2 rawInput;
        private Vector2 controllerInput;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            player = GetComponent<Player>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            if (!characterController.enabled)
                return;

            Vector3 dir = new Vector3(controllerInput.x, 0, controllerInput.y).normalized;
            characterController.Move(dir * player.speed * Time.deltaTime);
        }

        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            rawInput = input;

            if (allowMovement)
                controllerInput = input;
        }

        public void DisableMovement()
        {
            controllerInput = Vector2.zero;
            allowMovement = false;
        }

        public void EnableMovement()
        {
            controllerInput = rawInput;
            allowMovement = true;
        }
    }
}
