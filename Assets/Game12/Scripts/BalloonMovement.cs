using System.Collections;
using UnityEngine;

/// <summary>
/// Class that is in charge of the movement of the player's balloon.
/// </summary>
public class BalloonMovement : MonoBehaviour
{
    public delegate void BalloonDelegate(bool enable);
    public static event BalloonDelegate StartMagnetMode;

    [Header("Movement")]
    float speed = 10.0f;
    float minBound = -3.6f;
    float maxBound = 3.6f;
    float h;
    bool colorMode = false;
    public bool canMove = false;

    [Header("Components")]
    [SerializeField] Animator anim = null;

    [Header("Sounds")]
    [SerializeField] AudioSource pickUpSound = null;

    void Update()
    {
        if (transform.position.x < minBound && h < 0)
        {
            h = 0;
        }

        else if (transform.position.x > maxBound && h > 0)
        {
            h = 0;
        }

        if (canMove)
        {
            transform.Translate(Vector2.right * speed * h * Time.deltaTime);
        }

        if (Input.GetButtonDown("Cancel") && canMove)
        {
            GameManager12.manager.PauseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game12/Bubble"))
        {
            collision.gameObject.SetActive(false);
            GameManager12.manager.UpdateScore(10);
        }

        else if (collision.gameObject.CompareTag("Game12/BubbleColor"))
        {
            collision.gameObject.SetActive(false);
            pickUpSound.Play();

            StopCoroutine(ColorMode());
            StartCoroutine(ColorMode());
        }

        else if (collision.gameObject.CompareTag("Game12/Magnet"))
        {
            collision.gameObject.SetActive(false);
            pickUpSound.Play();

            StopCoroutine(MagnetMode());
            StartCoroutine(MagnetMode());
        }

        else if (collision.gameObject.CompareTag("Game12/Spike1") ||
            collision.gameObject.CompareTag("Game12/Spike2") ||
            collision.gameObject.CompareTag("Game12/Spike3") ||
            collision.gameObject.CompareTag("Game12/Spike4") ||
            collision.gameObject.CompareTag("Game12/Spike5"))
        {
            if (colorMode)
            {
                return;
            }

            StopAllCoroutines();
            GameManager12.manager.GameOver();
            canMove = false;
            h = 0;
        }
    }

    /// <summary>
    /// Function to activate the mobile inputs.
    /// </summary>
    /// <param name="input">2 right, 4 left, 5 disable.</param>
    public void EnableInputs(int input)
    {
        switch (input)
        {
            case 2:
                h = 1;
                break;
            case 4:
                h = -1;
                break;
            case 5:
                h = 0;
                break;
        }
    }

    /// <summary>
    /// Coroutine that activates temporary invulnerability.
    /// </summary>
    /// <returns></returns>
    IEnumerator ColorMode()
    {
        colorMode = true;

        anim.SetBool("ColorMode", true);

        yield return new WaitForSeconds(5);

        anim.SetBool("ColorMode", false);

        yield return new WaitForSeconds(0.5f);

        colorMode = false;
    }

    /// <summary>
    /// Coroutine that activates magnetism temporarily.
    /// </summary>
    /// <returns></returns>
    IEnumerator MagnetMode()
    {
        StartMagnetMode(true);
        GameManager12.manager.magnetMode = true;

        yield return new WaitForSeconds(7);

        StartMagnetMode(false);
        GameManager12.manager.magnetMode = false;
    }
}
