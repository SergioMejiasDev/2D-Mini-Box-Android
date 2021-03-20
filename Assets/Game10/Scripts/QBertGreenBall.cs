using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the functions of the green balls.
/// </summary>
public class QBertGreenBall : MonoBehaviour
{
    [Header("Movement")]
    Vector2 destination = Vector2.zero;
    Vector2 newDestination = Vector2.zero;
    [SerializeField] LayerMask borderMask = 8;
    bool falling = false;
    bool isActive = false;
    float waitTime = 0.5f;
    float timer;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    [SerializeField] SpriteRenderer sr = null;

    void OnEnable()
    {
        GameManager10.StopMovement += StopMovement;

        timer = 0;
        isActive = false;
        falling = false;
        rb.gravityScale = 0;
        sr.sortingOrder = 1;

        StartCoroutine(Falling());
    }

    private void OnDisable()
    {
        GameManager10.StopMovement -= StopMovement;
    }

    void Update()
    {
        if (!isActive)
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
                    Fall();
                    return;
                }
            }

            if (timer > waitTime)
            {
                timer = 0;

                destination = NewDestination();
            }
        }

        timer = timer + Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!isActive)
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
    /// Function that causes the ball to be affected by gravity.
    /// </summary>
    void Fall()
    {
        rb.gravityScale = 0.75f;

        isActive = false;
    }

    /// <summary>
    /// Function called through the delegate and that stops the movement of the ball.
    /// </summary>
    void StopMovement()
    {
        isActive = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game10/Destructor"))
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
        while (transform.position.y > 0.55f)
        {
            transform.Translate(Vector2.down * 5f * Time.deltaTime);

            yield return null;
        }

        transform.position = new Vector2(transform.position.x, 0.55f);
        destination = transform.position;
        newDestination = transform.position;

        isActive = true;
    }

    /// <summary>
    /// Function that decides what will be the next position to which the ball will move.
    /// </summary>
    /// <returns>A Vector2 with one of two possible positions.</returns>
    Vector2 NewDestination()
    {
        if (Random.value < 0.5f)
        {
            if (InBorder(new Vector2(0.5f, -0.75f)))
            {
                falling = true;
            }

            newDestination = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.75f);
            return new Vector2(transform.position.x + 0.43f, transform.position.y - 0.25f);
        }

        else
        {
            if (InBorder(new Vector2(-0.5f, -0.75f)))
            {
                falling = true;
            }

            newDestination = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.75f);
            return new Vector2(transform.position.x - 0.43f, transform.position.y - 0.25f);
        }
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
