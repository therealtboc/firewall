using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyController is a test script that allows for AI control of an enemy object
public class EnemyController : MonoBehaviour
{
    //****************************************************************** Variable Declaration ******************************************************************
    public CharacterController2D enemyController;
    public float enemyRunSpeed = 30f;
    public float horizontalMove = 1f;
       
    private bool groundedCheck = true;

    //****************************************************************** Start function ******************************************************************
    void Start()
    {
        //horizontalMove *= enemyRunSpeed;
    }

    //****************************************************************** Update function ******************************************************************
    // Update is called once per frame
    void Update()
    {
        
    }

    //****************************************************************** FixedUpdate function ******************************************************************
    void FixedUpdate()
    {
        horizontalMove = horizontalMove * enemyRunSpeed;
        //Debug.Log(horizontalMove);
        horizontalMove = 1;

        groundedCheck = enemyController.getGrounded();
        Debug.Log(groundedCheck + " " + horizontalMove);
        if (groundedCheck)
        {
            Debug.Log("we're moving");
            enemyController.Move(horizontalMove * Time.fixedDeltaTime, false);
        }

        //enemyController.Move(horizontalMove * Time.fixedDeltaTime, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Environment")
        //{
        //    horizontalMove *= -1;
        //}
    }
}

//****************************************************************** END OF CODE ******************************************************************
