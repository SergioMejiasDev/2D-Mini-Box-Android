using UnityEngine;

/// <summary>
/// Class that manages the functions of the blocks of the pyramid.
/// </summary>
public class QBertBlocks : MonoBehaviour
{
    [SerializeField] AudioSource sound = null;
    [SerializeField] SpriteRenderer sr = null;
    [SerializeField] Animator anim = null;
    [SerializeField] Sprite blueSprite = null;
    [SerializeField] Sprite pinkSprite = null;
    [SerializeField] Sprite yellowSprite = null;

    int state = 0;
    bool canChange = true;

    /// <summary>
    /// Function that activates or deactivates the colliders of the blocks.
    /// </summary>
    /// <param name="enable">Enable or disable.</param>
    public void EnableOrDisable(bool enable)
    {
        canChange = enable;
    }

    /// <summary>
    /// Function that activates or deactivates the animation of the blocks.
    /// </summary>
    /// <param name="enable">Enable or disable.</param>
    public void EnableAnimator(bool enable)
    {
        anim.enabled = enable;
    }

    /// <summary>
    /// Function that resets the sprite of the block to the initial one.
    /// </summary>
    public void ResetSprite()
    {
        state = 0;
        sr.sprite = blueSprite;
        canChange = true;
    }

    /// <summary>
    /// Function that changes the color of the block to the corresponding one.
    /// </summary>
    void ChangeColour()
    {
        if (state == 1)
        {
            sr.sprite = pinkSprite;
        }

        else if (state == 2)
        {
            sr.sprite = yellowSprite;

            GameManager10.manager.ReduceBlocks();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Game10/GreenBall"))
        {
            if (!canChange)
            {
                return;
            }

            sound.Play();

            if (state == 2)
            {
                return;
            }

            GameManager10.manager.UpdateScore(5);

            state += 1;

            ChangeColour();
        }
    }
}
