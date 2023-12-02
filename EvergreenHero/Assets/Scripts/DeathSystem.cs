using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathSystem : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sprRen;
    private Animator anim;

    private Vector3 startingPos;
    private int LivesCounter = 3;

    public Text livesCounterText;
    public Text checkpointCheckText;

    public PlayerController playerController;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprRen = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        startingPos = transform.position;

        livesCounterText.text = "Lives: " + LivesCounter.ToString();
        checkpointCheckText.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            /* When the player touches a checkpoint, it begins the checkpoint text coroutine which displays text to inform the player that they unlocked a checkpoint.
             * Additionally, the starting position for the player is updated with a slight offset for the x position. This allows for the player to respawn at this area. */
            startingPos = new Vector3(2 + transform.position.x, transform.position.y, transform.position.z);
            StartCoroutine(CheckpointTextTime(2.0f));
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
    }

    private IEnumerator CheckpointTextTime(float duration)
    {
        checkpointCheckText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        checkpointCheckText.gameObject.SetActive(false);
    }

    private void Death()
    {
        /* This function works by first disabling the player's movement and setting their rigidbody type to static. Then, the death animation is played and a life is removed
         * from the lives counter. The lives counter text on the canvas is then updated, and the DespawnTime coroutine begins. The DespawnTime coroutine is a simple coroutine
         that disables the player's sprite renderer after a certain time. Afther that, the RespawnTime coroutine begins, which enables player movement, their sprite renderer,
        sets their rigidbody type back to dynamic and teleports them back to their starting position after some time.
        
         In this function, if the lives counter reaches 0, the coroutine ResetToMenuTime begins, which boots the player back to the main menu after some time. */
        playerController.canMove = false;
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetBool("ded", true);
        LivesCounter -= 1;
        livesCounterText.text = "Lives: " + LivesCounter.ToString();
        StartCoroutine(DespawnTime(0.5f));
        if (LivesCounter == 0)
        {
            StartCoroutine(ResetToMenuTime(2.0f));
        }
        StartCoroutine(RespawnTime(2.0f));
    }

    private IEnumerator DespawnTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        sprRen.enabled = false;
    }
    private IEnumerator RespawnTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        playerController.canMove = true;
        sprRen.enabled = true;
        transform.position = startingPos;
        rb.bodyType = RigidbodyType2D.Dynamic;
        anim.SetBool("ded", false);
    }

    private IEnumerator ResetToMenuTime (float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(0);
    }
}
