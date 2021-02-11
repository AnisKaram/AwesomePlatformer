using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailScript : MonoBehaviour
{
    private float moveSpeed = 1f;
    private Rigidbody2D myBody;
    private Animator anim;

    private bool moveLeft;

    private bool canMove;
    private bool stunned;

    public Transform leftCollision, rightCollision, topCollision, downCollision;
    private Vector3 leftCollisionPos, rightCollisionPos;

    public LayerMask playerMask;

    private float pushSnail = 30f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        leftCollisionPos = leftCollision.position;     // when the enemy changes his scale (position), collision position must also change too.
        rightCollisionPos = rightCollision.position;   // when the enemy changes his scale (position), collision position must also change too.
    }
    // Start is called before the first frame update
    void Start()
    {
        moveLeft = true;    // so that our enemy start moving to left
        canMove = true;    // to indicate that we can move (enemy can move)
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (moveLeft)
            {
                myBody.velocity = new Vector2 (-moveSpeed, myBody.velocity.y);
            }
            else
            {
                myBody.velocity = new Vector2 (moveSpeed, myBody.velocity.y);
            }

        }

        CheckCollision();

    }

    void CheckCollision()
    {
        RaycastHit2D leftHit = Physics2D.Raycast (leftCollision.position, Vector2.left, 0.2f, playerMask);
        RaycastHit2D rightHit = Physics2D.Raycast (rightCollision.position, Vector2.right, 0.2f, playerMask);

        Collider2D topHit = Physics2D.OverlapCircle(topCollision.position, 0.2f, playerMask);

        if (topHit != null)
        {
            if (topHit.gameObject.tag == MyTags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    // we need to bounce the player a bit in the air using velocity
                    topHit.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(topHit.gameObject.GetComponent<Rigidbody2D>().velocity.x, 7f);

                    canMove = false;
                    // Snail cannot move anymore
                    myBody.velocity = new Vector2(0, 0);

                    anim.Play("SnailStunnedAnimation");
                    stunned = true;

                    //  BEETLE CODE HERE
                    if (tag == MyTags.BEETLE_TAG)
                    {
                        anim.Play ("BeetleStunnedAnimation");
                        StartCoroutine (Dead (2f));
                    }
                }
            }

        }

        if (leftHit)
        {
            if (leftHit.collider.gameObject.tag == MyTags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    // APPLY DAMAGE TO PLAYER
                }
                else
                {   // We only need to push the snail when stunned, not the beetle too
                    if (tag != MyTags.BEETLE_TAG)
                    {
                        myBody.velocity = new Vector2 (pushSnail, myBody.velocity.y);
                        StartCoroutine (Dead (1.5f));
                    }  
                }
            }
        }

        if (rightHit)
        {
            if (rightHit.collider.gameObject.tag == MyTags.PLAYER_TAG)
            {
                if (!stunned)
                {
                    // APPLY DAMAGE TO PLAYER  
                }
                else
                {   // We only need to push the snail when stunned, not the beetle too
                    if (tag != MyTags.BEETLE_TAG)
                    {
                        myBody.velocity = new Vector2 (-pushSnail, myBody.velocity.y);
                        StartCoroutine (Dead (1.5f));
                    }
                    
                }
            }
        }

        // If the CheckCollision function does not detect a collision downwards, so the snail will change it directions
        if (!Physics2D.Raycast (downCollision.position, Vector2.down, 0.1f))
        {
            ChangeDirection ();
        }
    }

    void ChangeDirection ()
    {
        moveLeft = !moveLeft;

        // Temporary scale of our current enemy (here is: snail)
        Vector3 tempScale = transform.localScale;

        if (moveLeft)
        {
            tempScale.x = Mathf.Abs (tempScale.x);

            leftCollision.position = leftCollisionPos;
            rightCollision.position = rightCollisionPos;
        }
        else
        {
            tempScale.x = -Mathf.Abs (tempScale.x);

            leftCollision.position = rightCollisionPos;     // when the enemy face the right side, left collision must be at the right position
            rightCollision.position = leftCollisionPos;     // when the enemy face the right side, right collision must be at the left position
        }

        transform.localScale = tempScale;
    }

    IEnumerator Dead (float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive (false);
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == MyTags.BULLET_TAG)
        {
            if (tag == MyTags.BEETLE_TAG)
            {
                anim.Play("BeetleStunnedAnimation");

                canMove = false;
                myBody.velocity = new Vector2 (0f, 0f);

                StartCoroutine (Dead(0.3f));
            }

            if (tag == MyTags.SNAIL_TAG)
            {
                if (!stunned)
                {
                    anim.Play ("SnailStunnedAnimation");
                    canMove = false;
                    stunned = true;
                    myBody.velocity = new Vector2 (0f, 0f);
                }
                else
                {
                    gameObject.SetActive (false);
                }
            }
        } 
    }
    
}   // class
