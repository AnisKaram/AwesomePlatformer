using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D myBody;

    private Vector3 moveDirection = Vector3.down;

    // Used to start and stop the coroutine (without string we can only start the coroutine)
    private string coroutineName = "ChangeSpiderMovement";


    void Awake()
    {
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Because our spider won't move up if we did not call the coroutine here
        StartCoroutine (coroutineName);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpider();
    }

    void MoveSpider()
    {
        transform.Translate (moveDirection * Time.smoothDeltaTime);
    }

    IEnumerator ChangeSpiderMovement()
    {
        yield return new WaitForSeconds (Random.Range (2f,  4.8f));

        if (moveDirection == Vector3.down)
        {
            moveDirection = Vector3.up;
        }
        else
        {
            moveDirection = Vector3.down;
        }

        StartCoroutine (coroutineName);
    }

    IEnumerator SpiderDead()
    {
        yield return new WaitForSeconds (3f);

        gameObject.SetActive (false);
    }

    void OnTriggerEnter2D (Collider2D target)
    {
        if (target.tag == MyTags.BULLET_TAG)
        {
            anim.Play ("SpiderDeadAnimation");

            // To let our spider fall down
            myBody.bodyType = RigidbodyType2D.Dynamic;

            StartCoroutine (SpiderDead());
            StopCoroutine (coroutineName);
        }
    }

}   // class
