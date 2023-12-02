using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectItem : MonoBehaviour
{
    private BoxCollider2D bc2d;
    private SpriteRenderer sprRen;
    private int collectedItems;

    public Text collectedText;
    public Text itemText;

    
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        sprRen = GetComponent<SpriteRenderer>();
        collectedItems = 0;

        collectedText.text = collectedItems.ToString() + "/3";
        itemText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* When the player runs into a collectible item, we first increment the number of collected items by 1. After which, we update the previous
         * text displaying the number of collected items. We then destroy the collected item's sprite renderer and BoxCollider2D components before 
         starting the DisplayItemText coroutine, which shows a text indicating the player's collection of an item. After a certain time, this text
        is deactivated and the item itself is destroyed.
        
         The reason that the item cannot be destroyed outright is that it will stop the script in the middle of executing the DisplayItemText coroutine.
        This means that the displayed text remains displayed for the whole game, and thus the item can only be destroyed after the text is deactivated.*/
        if (collision.gameObject.CompareTag("Player"))
        {
            collectedItems++;
            collectedText.text = collectedItems.ToString() + "/3";

            Destroy(GetComponent<SpriteRenderer>());
            Destroy(GetComponent<BoxCollider2D>());
            StartCoroutine(DisplayItemText(2.0f));
        }
    }

    private IEnumerator DisplayItemText(float duration)
    {
        itemText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        itemText.gameObject.SetActive(false);
    }
}
