using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    private Rigidbody2D myBody;
    private Animator anim;

    private Vector3 moveDirection = Vector3.left;
    private Vector3 originPosition;
    private Vector3 movePosition;
    private float scale = 0.5f;
    private float speed = 2f;

    [SerializeField]
    public GameObject birdEgg;
    public LayerMask playerLayer;
    private bool attacked;

    private bool canMove;

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {   
        // go forward
        originPosition = transform.position;
        originPosition.x += 6f;

        // go backward
        movePosition = transform.position;
        movePosition.x -= 6f;

        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTheBird();
        DropTheEgg();
    }

    void MoveTheBird()
    {
        if (canMove)
        {
            transform.Translate (moveDirection * speed * Time.smoothDeltaTime);

            if (transform.position.x >= originPosition.x)
            {   
                // if greater, we need to go the left
                moveDirection = Vector3.left;

                ChangeDirection (scale);
            }
            else if (transform.position.x <= movePosition.x)
            {
                // if less or equal, we need to go to the right
                moveDirection = Vector3.right;

                ChangeDirection (-scale);
            }
        }
    }

    void ChangeDirection (float direction)
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = direction;
        transform.localScale = tempScale;
    }

    void DropTheEgg ()
    {
        if (!attacked)
        {
            if (Physics2D.Raycast (transform.position, Vector2.down, Mathf.Infinity, playerLayer))
            {
                Instantiate (birdEgg, new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                
                // It cannot attack anymore
                attacked = true;
                anim.Play ("BirdFlyAnimation");
            }
        }
    }

    IEnumerator BirdDead()
    {
        yield return new WaitForSeconds (4f);
        gameObject.SetActive (false);
    }

    // When we hit the bird with the bullet, the bird will fall down and dissappear after 3secs.
    void OnTriggerEnter2D (Collider2D target)
    {
        if (target.tag == MyTags.BULLET_TAG)
        {
            anim.Play ("BirdDeadAnimation");

            GetComponent<BoxCollider2D>().isTrigger = true;
            myBody.bodyType = RigidbodyType2D.Dynamic;

            canMove = false;

            StartCoroutine (BirdDead());
        }
    }
}
