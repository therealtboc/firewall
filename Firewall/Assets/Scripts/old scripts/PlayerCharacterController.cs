using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{

    private Rigidbody2D rb;
    public Vector2 movementSpeed;
    bool isMoving;
    private float moveTime = 0.1f;
    public bool onCooldown = false;


    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //StartCoroutine(actionCooldown(0.1f));
            Move(-movementSpeed);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //StartCoroutine(actionCooldown(0.1f));
            Move(movementSpeed);
        }

    }

    private void Move(Vector2 m)
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + m;

        rb.position = endPosition;

        StartCoroutine(SmoothMovement(endPosition));
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        //while (isMoving) yield return null;

        isMoving = true;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }

        isMoving = false;
    }

    private IEnumerator actionCooldown(float cooldown)
    {
        onCooldown = true;

        //float cooldown = 0.2f;
        while (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            yield return null;
        }

        onCooldown = false;
    }
}
