using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D myBody;
    private Animator anim;

    public Transform groundCheckPosition;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool jumped;

    private float jumpPower = 11.5f;

    // Called First
    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    // void Start()
    // {
       
    // }

    // Update is called once per frame
    void Update()
    { 
        CheckIfGrounded();
        PlayerJump();
    }

    void FixedUpdate()
    {
        PlayerWalk();
    }

    void PlayerWalk()
    {
        // Get the input from the player
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0)
        {
            myBody.velocity = new Vector2 (speed, myBody.velocity.y);

            ChangeDirection(1);
        } 
        else if (h < 0)
        {
            myBody.velocity = new Vector2 (-speed, myBody.velocity.y);

            ChangeDirection(-1);
        }
        else
        {
            myBody.velocity = new Vector2(0f, myBody.velocity.y);
        }

        // Animation returning the absolute value of the velocity.x, if greater than 0 idle to run, equal to 0 run to idle
        anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));
    }

    void ChangeDirection(int direction)
    {
        Vector3 tempScale = transform.localScale;   // putting the current scale, inside a variable (temp)
        tempScale.x = direction;                   // changing the value of x (because our player facing x)
        transform.localScale = tempScale;         // reassigning the value back
    }

    void CheckIfGrounded()
    {
        // if the player is on the Ground Layer, it will return -> isGround = true
        isGrounded = Physics2D.Raycast (groundCheckPosition.position, Vector2.down, 0.1f, groundLayer);

        if (isGrounded)
        {
            // jumped before
            if (jumped) 
            {
                jumped = false;

                anim.SetBool("Jump", false);
            }
        }
    }

    void PlayerJump()
    {
        // Checking if true or not, if player is on the ground, and player pressed 'Space'
        if (isGrounded)
        {
            if (Input.GetKeyDown (KeyCode.Space))
            {
                jumped = true;
                myBody.velocity = new Vector2(myBody.velocity.x, jumpPower);

                anim.SetBool("Jump", true);
            }
        }
    }

}   // class
