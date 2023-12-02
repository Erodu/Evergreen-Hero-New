using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextActivation : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    public Text DoubleJumpTutorialText;
    public Text DashTutorialText;
    public Text FallingPlatformTutorialText;
    public Text EnemyTutorialText;
    public int tutorialNumberType;

    /* Tutorial number type will decide which text to display for the tutorial. 0 is for the Double Jump Tutorial,
     * 1 is for the Dash Tutorial, 2 is for the Falling Platform Tutorial, and 3 is for the Enemy Tutorial. */

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (tutorialNumberType == 0)
            {
                StartCoroutine(ShowDoubleJump(2.0f));
            }
            else if (tutorialNumberType == 1)
            {
                StartCoroutine(ShowDash(2.0f));
            }
            else if (tutorialNumberType == 2)
            {
                StartCoroutine(ShowFallingPlatform(2.0f));
            }
            else if (tutorialNumberType == 3)
            {
                StartCoroutine(ShowEnemy(2.0f));
            }
        }
    }

    private IEnumerator ShowDoubleJump(float duration)
    {
        DoubleJumpTutorialText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(DoubleJumpTutorialText);
    }

    private IEnumerator ShowDash(float duration)
    {
        DashTutorialText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(DashTutorialText);
    }
    private IEnumerator ShowFallingPlatform(float duration)
    {
        FallingPlatformTutorialText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(FallingPlatformTutorialText);
    }

    private IEnumerator ShowEnemy(float duration)
    {
        EnemyTutorialText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        Destroy(EnemyTutorialText);
    }
}
