using System;
using UnityEngine;

namespace _Workspace.Scripts
{
    public class GameInput
    {
        private PlayerInput  _playerInput;

        public GameInput()
        {
                _playerInput = new PlayerInput();
                _playerInput.Enable();
        }

        public Vector2 GetMovementDirection()
        {
            var value =  _playerInput.Player.Move.ReadValue<Vector2>();
            
            // Debug.Log($"Movement Value -> {value}");
            
            return value;
        }

        public bool GetBrakeStatus()
        {
            var value = _playerInput.Player.HandBrake.IsPressed();
            
            // Debug.Log($"Break Value -> {value}");
            
            return value;
        }
    }
}