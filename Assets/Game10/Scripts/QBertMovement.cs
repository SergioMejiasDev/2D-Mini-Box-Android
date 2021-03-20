using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the player's movement.
/// </summary>
public class QBertMovement : MonoBehaviour
{
    [Header("Movement")]
    Vector2 destination = Vector2.zero;
    Vector2 newDestination = Vector2.zero;
    [SerializeField] LayerMask borderMask = 8;
    bool inputsEnabled = false;
    bool inMovement = false;
    bool hasChecked = true;
    bool falling = false;
    bool isDead = true;

    [Header("Bubble")]
    [SerializeField] GameObject bubble = null;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    [SerializeField] SpriteRenderer sr = null;

    void OnEnable()
    {
        sr.sortingOrder = 1;
        rb.gravityScale = 0;
        ResetValues();

        GameManager10.manager.EnableBlocks(true);

        StartCoroutine(Falling());
    }

    private void OnDisable()
    {
        bubble.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager10.manager.PauseGame();
        }

        if (isDead)
        {
            return;
        }

        if ((Vector2)transform.position == destination)
        {
            if (destination != newDestination)
            {
                destination = newDestination;
                return;
            }

            else
            {
                if (falling)
                {
                    Die();
                }
            }

            if (!hasChecked)
            {
                hasChecked = true;

                ResetValues();
            }

            else if (!inMovement)
            {
                if (!inputsEnabled)
                {
                    inputsEnabled = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        Vector2 newPosition = Vector2.MoveTowards(transform.position, destination, 0.05f);
        rb.MovePosition(newPosition);

        Vector2 direction = destination - (Vector2)transform.position;
        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);
    }

    /// <summary>
    /// Function that allows us to call the movement inputs.
    /// </summary>
    /// <param name="input">1 up, 2 right, 3 down, 4 left.</param>
    public void EnterInputs(int input)
    {
        if (!inputsEnabled || Time.timeScale != 1)
        {
            return;
        }

        if (input == 1)
        {
            destination = new Vector2(transform.position.x + 0.05f, transform.position.y + 0.5f);
            newDestination = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.75f);

            inMovement = true;
            hasChecked = false;
            inputsEnabled = false;

            if (InBorder(new Vector2(0.5f, 0.75f)))
            {
                falling = true;
            }
        }

        if (input == 2)
        {
            destination = new Vector2(transform.position.x + 0.45f, transform.position.y - 0.25f);
            newDestination = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.75f);

            inMovement = true;
            hasChecked = false;
            inputsEnabled = false;

            if (InBorder(new Vector2(0.5f, -0.75f)))
            {
                falling = true;
            }
        }

        if (input == 3)
        {
            destination = new Vector2(transform.position.x - 0.45f, transform.position.y - 0.25f);
            newDestination = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.75f);

            inMovement = true;
            hasChecked = false;
            inputsEnabled = false;

            if (InBorder(new Vector2(-0.5f, -0.75f)))
            {
                falling = true;
            }
        }

        if (input == 4)
        {
            destination = new Vector2(transform.position.x - 0.05f, transform.position.y + 0.5f);
            newDestination = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.75f);

            inMovement = true;
            hasChecked = false;
            inputsEnabled = false;

            if (InBorder(new Vector2(-0.5f, 0.75f)))
            {
                falling = true;
            }
        }
    }


    /// <summary>
    /// Function that makes the player temporarily invulnerable.
    /// </summary>
    public void WinGame()
    {
        isDead = true;
    }

    /// <summary>
    /// Function that reset the player position.
    /// </summary>
    public void ResetPosition()
    {
        sr.sortingOrder = 1;
        rb.gravityScale = 0;
        ResetValues();

        StartCoroutine(Falling());
    }

    /// <summary>
    /// Function that is activated when the player hits an enemy.
    /// </summary>
    void Hit()
    {
        GameManager10.manager.EnableBlocks(false);

        isDead = true;

        bubble.SetActive(true);

        GameManager10.manager.DeathHit();
    }

    /// <summary>
    /// Function that is activated when the player falls into the void.
    /// </summary>
    void Die()
    {
        GameManager10.manager.EnableBlocks(false);

        isDead = true;

        rb.gravityScale = 0.75f;

        if (transform.position.y > -2.9f)
        {
            sr.sortingOrder = -1;
        }

        GameManager10.manager.DeathFall();
    }

    /// <summary>
    /// Function that resets the player's movement values.
    /// </summary>
    void ResetValues()
    {
        inMovement = false;
        falling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game10/RedBall") || collision.gameObject.CompareTag("Game10/PurpleBall"))
        {
            if (!isDead)
            {
                Hit();
            }
        }

        else if (collision.gameObject.CompareTag("Game10/GreenBall"))
        {
            GameManager10.manager.UpdateScore(75);
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Game10/Disc"))
        {
            WinGame();
        }

        else if (collision.gameObject.CompareTag("Game10/Destructor"))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Coroutine that activates the initial fall of the ball.
    /// </summary>
    /// <returns></returns>
    IEnumerator Falling()
    {
        while (transform.position.y > 1.5f)
        {
            transform.Translate(Vector2.down * 5f * Time.deltaTime);

            yield return null;
        }

        transform.position = new Vector2(transform.position.x, 1.5f);
        destination = transform.position;
        newDestination = transform.position;

        isDead = false;
    }

    /// <summary>
    /// Function that checks if the next movement will make the ball fall into the void.
    /// </summary>
    /// <param name="direction">The direction to which it is going to move.</param>
    /// <returns>True if it falls, false if not.</returns>
    bool InBorder(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, borderMask);

        return hit;
    }
}
