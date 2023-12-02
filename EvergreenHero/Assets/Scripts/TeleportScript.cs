using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    private Vector3 teleportTo;

    public GameObject TeleportExit;

    /* We first declare a public GameObject called TeleportExit, which allows us to drag and define the exiting teleporter in our editor. We log this exiting teleporter's position.
     * If an object tagged 'Player' triggers this teleporter's BoxCollider2D, their position is set to the position of the exiting teleporter, teleporting them. */
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();

        teleportTo = new Vector3(TeleportExit.transform.position.x, TeleportExit.transform.position.y + 2, TeleportExit.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D playerObject)
    {
        if (playerObject.gameObject.CompareTag("Player"))
        {
            playerObject.transform.position = teleportTo;
        }
    }

}
