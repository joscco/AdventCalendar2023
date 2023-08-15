using System;
using UnityEngine;

namespace GameScene
{
    public class InputManager : MonoBehaviour
    {
        public bool JustPressedE()
        {
            return Input.GetKeyDown(KeyCode.E);
        }

        public Vector2Int GetMoveDirection()
        {
            int horizontalMove = 0;
            int verticalMove = 0;

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                horizontalMove++;
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                horizontalMove--;
            }

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                verticalMove++;
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                verticalMove--;
            }

            if (Math.Abs(horizontalMove) == 1)
            {
                return new Vector2Int(horizontalMove, 0);
            }

            return new Vector2Int(0, verticalMove);
        }

        public bool JustPressedSpace()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }

        public bool IsHoldingShift()
        {
            return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        }
    }
}