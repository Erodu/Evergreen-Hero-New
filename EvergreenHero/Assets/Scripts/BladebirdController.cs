using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladebirdController : MonoBehaviour
{
    public float speed = 1;

    private Rigidbody2D rb;
    private BoxCollider2D coll2D;
    private SpriteRenderer sprRen;
    public Animator anim;
    private Transform target;
    private Vector2 startingPosition;
    private bool facingRight = true;

    float moveDirection;
    public float trackingArea;
    private enum AnimState { idle, movement }
    private AnimState state;

    /* We generally follow the concepts from our PlayerController script. However, we define our target by finding an object that is tagged with 'Player'. We also use the OnDrawGizmosSelected(), which draws a circle around
     * the enemy GameObject. Inside this area is our tracking area, which is the area where our target gets chased by the enemy if they are present within it. If the player is in this area, we simply have the enemy move towards
     them. When they exit this area, we have the enemy move back towards its original position which was stored in a variable beforehand. */
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<BoxCollider2D>();
        sprRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        startingPosition = transform.position;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float targetDistance = Vector2.Distance(target.position, transform.position);
        if (targetDistance < trackingArea)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime);
            UpdateDirection();
            if (rb.velocity.x != 0f)
            {
                anim.SetInteger("state", 0);
            }
            else
            {
                anim.SetInteger("state", 1);
            }
        }
        else if (transform.position.x < startingPosition.x || transform.position.x > startingPosition.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;

        if (target.position.x < transform.position.x && facingRight) 
        {
            transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            facingRight = false;
        }
        else if (target.position.x > transform.position.x && !facingRight) 
        {
            transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            facingRight = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingArea);
    }
}
