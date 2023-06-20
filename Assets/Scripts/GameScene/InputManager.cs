using System;
using GameScene.UI;
using UnityEngine;

public class InputManager : MonoBehaviour
{
   public static InputManager instance;

   private void Start()
   {
       instance = this;
   }

   public bool GetEnterOrSpace()
   {
       return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ;
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
}