using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//V3PlayerCharacterController is a script that gives player input functionality to a Player object. This allows for control of movement in tandem with a CharacterController2D script
public class V3PlayerCharacterControler : MonoBehaviour
{
    //****************************************************************** Variable Declaration ******************************************************************
    //Associated CharacterController2D object
    public CharacterController2D controller;
    //Run speed for horizontal movement
    public float runSpeed = 40f;

    float horizontalMove = 0f;

    //bool jump = false;

    //****************************************************************** Update function ******************************************************************

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        /*if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }*/

    }

    //****************************************************************** FixedUpdate function ******************************************************************

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false /*, jump*/);
       //jump = false;
    }

}

//****************************************************************** END OF CODE ******************************************************************
