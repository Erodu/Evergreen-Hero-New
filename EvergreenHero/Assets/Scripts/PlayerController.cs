using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public float jumpForce = 1f;
    public float gravityScale = 1.4f;
    public float gravityFalling = 1.6f;
    private bool facingRight = true;
    //private float dir;

    private Rigidbody2D rb;
    private BoxCollider2D coll2D;
    private SpriteRenderer sprRen;
    private Animator anim;

    [SerializeField] private LayerMask jumpGround;

    float horizontal;
    private bool doubleJump;
    private float coyoteTime = 0.15f;
    private float coyoteTimeCount;
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCount;

    private bool dashCheck = true;
    private bool isDashing;
    public float dashForce = 24f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;
    [SerializeField] private TrailRenderer trailRend;

    [HideInInspector] public bool canMove;
    public Image canDashImage;

    /*Using a special enum here allows us an alternate method to set the player GameObject's animation state. Later on, whenever the player is moving, we set their animation state to one of the following
     in the enum. This means that we use a single integer value for our animation states instead of multiple boolean values, which will hopefully let us have a cleaner workflow.*/
    private enum AnimState { idle, run, jump, fall }
    private AnimState state;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll2D = GetComponent<BoxCollider2D>();
        sprRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        canMove = true;
        canDashImage.gameObject.SetActive(true);
    }

    private void Update()
    {
        /* In this update method, the first if statement detects if the player is in the middle of dashing. Dashing is a mechanic that sends the player forward in the direction they face, assuming that dashCheck is true. 
         * A player can dash by pressing shift. If the player is dashing, they are unable to move (therefore be unable to change direction).
         
         Additionally, the horizontal variable reads horizontal input. Finally, when the player jumps by pressing space, the script calls the Jump() function. */
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        Jump();

        if (Input.GetButtonDown("Dash") && dashCheck)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (canMove == true)
        {
            Move(horizontal);
        }
    }

    /* The Move() function is what allows the player to move. First, we log the scale of the player GameObject, which will allow for the sprite to flip according to direction by multiplying -1 with its x scale.
     * It also calculates the movement speed with the float xSpeed. The player's velocity is recorded with this xSpeed and its y velocity.
     * The way that this function decides how to flip the player by using the boolean variable facingRight as well as the direction and if it is less or more than 0. Essentially, if the player is looking right
     * and left is clicked, the sprite is flipped to look left, and vice versa.
     * Finally, we use the AnimState from earlier to set the player GameObject's animation state. */
    void Move(float dir)
    {
        Vector3 locScale = transform.localScale;

        float xSpeed = dir * speed * 100 * Time.deltaTime;
        Vector2 playerVelocity = new Vector2(xSpeed,rb.velocity.y);
        rb.velocity = playerVelocity;

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z); 
            facingRight = true;
        }

        if (dir != 0f) 
        {
            state = AnimState.run;
        }

        else
        {
            state = AnimState.idle;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        /* This function creates a box around the player, which is the same as their BoxCollider2D and is offset slightly downwards. This function is able to check if the player GameObject is on the ground by
         * checking if the box overlaps with the ground. */
        return Physics2D.BoxCast(coll2D.bounds.center, coll2D.bounds.size, 0f, Vector2.down, .1f, jumpGround);
    }

    private IEnumerator Dash()
    {
        /* When the player presses shift to dash, we set the dashCheck to false in order to signal that the player cannot dash again. Additionally, we set isDashing to true. Afterwards, we store the original gravity
         * in the ogGrav variable since we will be disabling gravity while the player is dashing. This allows us to apply gravity once more after the player is finished dashing. after setting the gravity to 0, we have
         * the player be launched in the direction they face by multiplying their x localScale by a predefined Dash Force. We also set the emitting for our trail renderer to true, which gives the player a special trail
         while they're dashing. Finally, we set the dash image to false. The dash image is a UI component which indicates that the player can dash when it is visible. 
        
         We then start a new dash time. This is so that the dash plays for the number of seconds, and the dash stops after that time is up. We then set the trail renderer's emitting to false, reset the gravity to our
        stored original gravity and set isDashing to false. Afterwards, we start the cooldown. After the cooldown timer is up, dashCheck is set to true to allow for another dash, and the dash image is visible again. */
        dashCheck = false;
        isDashing = true;
        float ogGrav = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
        trailRend.emitting = true;
        canDashImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(dashTime);
        trailRend.emitting = false;
        rb.gravityScale = ogGrav; 
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        dashCheck = true;
        canDashImage.gameObject.SetActive(true);
    }

    private void Jump()
    {
        /* This function handles coyote time, jump buffering and general jump mechanics. First, if the player is grounded (as decided by the function from earlier), we set the coyote time counter to our predefined coyote
         * time. If the player isn't grounded, we begin counting the coyote time counter down. When the player jumps, we do the same with our jump buffer counter, and when the player isn't jumping we count down the jump
         buffer counter. If the jump buffer counter is greater than 0, we also check if the coyote time counter is greater than 0. If so, we allow for a small window where if the player is off a platform or the ground,
        they are able to stay in the air. After jumping, however, we set the coyote time counter to 0 in order to prevent the player from being able to spam jumps and exploit.
        
         Finally, like in our Move() function we set the animation state depending on if the player GameObject is rising or falling. */
        if (IsGrounded()) //If grounded, set the counter to coyote time
        {
            coyoteTimeCount = coyoteTime;
        }
        else
        {
            coyoteTimeCount -= Time.deltaTime; 
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCount = jumpBufferTime;
        }
        else
        {
            jumpBufferCount -= Time.deltaTime; 
        }

        if (jumpBufferCount > 0f)
        {
            jumpBufferCount = 0f;
            if (coyoteTimeCount > 0f)
            {
                doubleJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (doubleJump) 
            {
                doubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            coyoteTimeCount = 0f; 

        }

        if (rb.velocity.y > 0.1f)
        {
            rb.gravityScale = gravityScale;
            state = AnimState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            rb.gravityScale = gravityFalling;
            state = AnimState.fall;
        }

        anim.SetInteger("state", (int)state);
    }
}
