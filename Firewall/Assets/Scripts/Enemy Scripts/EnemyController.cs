using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemyController is a test script that allows for AI control of an enemy object
public class EnemyController : MonoBehaviour
{
    //****************************************************************** Variable Declaration ******************************************************************
    //public GameObject Player;
    public Transform target;

    public CharacterController2D enemyController;
    public float enemyRunSpeed = 30f;
    public float horizontalMove = 1f;

    private float changeDirection = 1f;

    bool playerFound = false;
       
    private bool groundedCheck = true;

    //****************************************************************** Start function ******************************************************************
    void Start()
    {
        //horizontalMove *= enemyRunSpeed;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    //****************************************************************** Update function ******************************************************************
    // Update is called once per frame
    void Update()
    {
        if (playerFound)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyRunSpeed * Time.deltaTime); //change speed
        }
    }

    //****************************************************************** FixedUpdate function ******************************************************************
    void FixedUpdate()
    {
        if (!playerFound)
        {
            horizontalMove = horizontalMove * enemyRunSpeed;
            //Debug.Log(horizontalMove);

            groundedCheck = enemyController.getGrounded();
            Debug.Log(groundedCheck + " " + horizontalMove);
            if (groundedCheck)
            {
                enemyController.Move(horizontalMove * Time.fixedDeltaTime, false);
            }

            horizontalMove = changeDirection;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            changeDirection *= -1f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerFound = true;
            //Debug.Log("player found");
        }
    }
}

//****************************************************************** END OF CODE ******************************************************************
