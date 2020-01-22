using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterControllerV2 : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector2 previousPosition;
    Vector2 currentPosition;
    Vector2 nextMovement;
    Vector2 jump_scalar;
  
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        Vector2 currentPosition = rb.position;
        Vector2 previousPosition = rb.position;

        nextMovement = new Vector2(1, 0);
        jump_scalar = new Vector2(0, 1);
    }

    //Fixed update is called a set amount of times per second
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            nextMovement = new Vector2(-1, 0);
            Move(nextMovement);
        }

        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            nextMovement = new Vector2(1, 0);
            Move(nextMovement);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += jump_scalar;
        }

    }

    public void Move(Vector2 m)
    {
        previousPosition = rb.position;
        currentPosition = previousPosition + m;

        rb.position = currentPosition;
    }

    public void Jump()
    {

    }
}
