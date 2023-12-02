using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    private Vector2 initialPosition;

    /* First, we log the initial position of the platform. If an object tagged 'Player' collides with this falling platform, we start to count down the time before
     * the platform falls using a coroutine. We set the Rigidbody2D to be not kinematic and also disable its BoxCollider2D, which makes the platform intangible. 
     * Afterwards, we start the count down for respawning the platform, and after the count down is finished we set the velocity of the platform to 0, set its
     Rigidbody2D to be kinematic once more and enable its BoxCollider2D. Then, we have the platform's position be its initial position. */
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Drop(0.5f));
        }
    }

    private IEnumerator Drop(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.isKinematic = false;
        bc2d.enabled = false;
        yield return new WaitForSeconds(delay);
        StartCoroutine(RespawnPlatform(2.0f));
    }

    private IEnumerator RespawnPlatform(float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        bc2d.enabled = true;
        rb.position = initialPosition;
    }
}
