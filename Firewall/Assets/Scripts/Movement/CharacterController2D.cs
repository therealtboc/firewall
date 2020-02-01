using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//The CharacterController2D class is a script that allows for horizontal movement, jumping, crouching, and flipping the player to accurately move. This should be added to any entity that moves.
public class CharacterController2D : MonoBehaviour
{
    //****************************************************************** Variable Declaration ******************************************************************
    // public Vector2 position = new Vector2(0.0f, 0.0f);
    //public float xPosition;

    public GameObject Entity;

    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;
    private int doubleJump = 1; //Used to prevent more than three jumps

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    //****************************************************************** Awake function ******************************************************************

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    //****************************************************************** Update function ******************************************************************

    private void Update()
    {
     
        //position.x = xPosition;
        //transform.position = position;
    }

    //****************************************************************** FixedUpdate function ******************************************************************

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                doubleJump = 1; //Sets doubleJump to 1 when grounded is true
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    //****************************************************************** Move function ******************************************************************
    //The move function gives horizontal movement functionality to the associated entity. It contains the logic for movement (velocity-based), jumping (velocity-based), and will call the Flip function for correct movement
    //NOTE - the code referenced has the logic for crouching - this is currently disabled.

    public void Move(float move, bool crouch/*, bool jump*/)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //******************** OVERARCHING MOVEMENT LOGIC ********************

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            //******************** CROUCHING ********************
            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            //******************** HORIZONTAL MOVEMENT ********************

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        //******************** JUMPING ********************

        // If the player should jump...
        if (Entity.tag == "Player")
        {
            if (m_Grounded && /*jump*/ Input.GetButtonDown("Jump"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
            else if (!m_Grounded && /*jump*/ Input.GetButtonDown("Jump") && doubleJump == 1) //Allows for a second jump while not grounded
            {

                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce)); //applies jump force
                doubleJump = 0; //sets doubleJump to 0
            }
            else //prevents a third jump
            {

            }
        }
    }

    //****************************************************************** Flip function ******************************************************************
    //The Flip function will change the direction the entity is facing.

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //****************************************************************** getGrounded function ******************************************************************
    //The getGrounded function returns the value of private bool m_grounded, allowing other scripts to glean if this entity is on the ground.

    public bool getGrounded()
    {
        return m_Grounded;
    }
}

//****************************************************************** END OF CODE ******************************************************************

