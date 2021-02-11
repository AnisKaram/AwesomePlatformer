using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    private float speed = 10f;
    private Animator anim;

    private bool canMove;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        
        // After firing the bullet, we will disable it after 5 seconds.
        StartCoroutine (DisableBullet(5f));
    }

    // Update is called once per frame
    void Update()
    {
        Move ();
    }

    void Move()
    {
        if (canMove)
        {
            // Since we can't change the Vector3 position, we need to store it in a 'temp' variable, of type Vector3
            Vector3 temp = transform.position;

            // Then we add to it the speed (+)
            temp.x += speed * Time.deltaTime;

            // Then we assign it back
            transform.position = temp;
       }

    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    IEnumerator DisableBullet (float time)
    {
        yield return new WaitForSeconds (time);
        gameObject.SetActive (false);
    }

    void OnTriggerEnter2D (Collider2D target)
    {
        if (target.gameObject.tag == MyTags.BEETLE_TAG || target.gameObject.tag == MyTags.SNAIL_TAG)
        {
            // To play the animation explode, then desactivate it
            anim.Play("ExplodeAnimation");

            // When we touch one of the game objects, we can't move it anymore (for each clone object)
            canMove = false;

            // To desactivate the bullet
            StartCoroutine (DisableBullet(0.05f));
        }
    }

} // class
