using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{

    private Animator anim;

    private bool animation_Started;
    private bool animation_Finished;

    private int timesJumped;
    private bool jumpLeft = true;

    private string coroutineName = "FrogJump";

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine (coroutineName);
    }

    // LateUpdate is called after the frame is finished
    void LateUpdate()
    {
        if (animation_Finished && animation_Started)
        {
            animation_Started = false;

            // These 2 line of codes, will make the animation independent from its own position
            transform.parent.position = transform.position; // Parent position, positioning it to our gameObject

            // Local Postion of the child (because there's a parent child), we set it to zero (stay attached to it)
            transform.localPosition = Vector3.zero;
        }
    }

    IEnumerator FrogJump()
    {
        yield return new WaitForSeconds (Random.Range (1f, 4f));

        animation_Started = true;
        animation_Finished = false;

        timesJumped++;

        if (jumpLeft)
        {
            anim.Play ("FrogJumpLeft");
        }
        else
        {
            anim.Play ("FrogJumpRight");
        }

        StartCoroutine (coroutineName);
    }

    void AnimationFinished()
    {
        animation_Finished = true;

        anim.Play ("FrogIdleLeft");

        if (jumpLeft)
        {
            anim.Play ("FrogIdleLeft");
        }
        else
        {
            anim.Play ("FrogIdleRight");
        }
        if (timesJumped == 3)
        {
            timesJumped = 0;

            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1f;
            transform.localScale = tempScale;

            jumpLeft = !jumpLeft;

        }
    }
}
