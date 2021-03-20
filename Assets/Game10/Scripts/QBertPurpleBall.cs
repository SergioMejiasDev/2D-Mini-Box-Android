using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the movement of purple balls (or snakes).
/// </summary>
public class QBertPurpleBall : MonoBehaviour
{
    [Header("Movement")]
    Transform player;
    Vector2 destination = Vector2.zero;
    Vector2 newDestination = Vector2.zero;
    [SerializeField] LayerMask borderMask = 8;
    bool falling = false;
    bool isActive = false;
    float waitTime = 1f;
    float timer;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    [SerializeField] SpriteRenderer sr = null;
    [SerializeField] QBertRedBall redClass = null;

    void OnEnable()
    {
        GameManager10.StopMovement += StopMovement;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        destination = transform.position;
        newDestination = transform.position;
        falling = false;
        timer = 0;

        StartCoroutine(FirstWait());
    }

    private void OnDisable()
    {
        GameManager10.StopMovement -= StopMovement;

        anim.SetBool("Snake", false);
        redClass.enabled = true;
        enabled = false;
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

        GameManager10.manager.FallSnake();

        if (transform.position.y > -2.9f)
        {
            sr.sortingOrder = -1;
        }

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

    /// <summary>
    /// Coroutine that manages the initial wait before turning into a snake.
    /// </summary>
    /// <returns></returns>
    IEnumerator FirstWait()
    {
        yield return new WaitForSeconds(2);

        isActive = true;
        anim.SetBool("Snake", true);
        anim.SetBool("Moving", false);
    }

    /// <summary>
    /// Function that decides what will be the next position to which the ball will move.
    /// </summary>
    /// <returns>A Vector2 with one of four possible positions.</returns>
    Vector2 NewDestination()
    {
        if (player == null)
        {
            return transform.position;
        }

        if (player.position.x >= transform.position.x && player.position.y >= transform.position.y)
        {
            if (InBorder(new Vector2(0.5f, 0.75f)))
            {
                falling = true;
            }

            newDestination = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.75f);
            return new Vector2(transform.position.x + 0.07f, transform.position.y + 0.5f);
        }

        else if (player.position.x >= transform.position.x && player.position.y <= transform.position.y)
        {
            if (InBorder(new Vector2(0.5f, -0.75f)))
            {
                falling = true;
            }

            newDestination = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.75f);
            return new Vector2(transform.position.x + 0.43f, transform.position.y - 0.25f);
        }

        else if (player.position.x <= transform.position.x && player.position.y <= transform.position.y)
        {
            if (InBorder(new Vector2(-0.5f, -0.75f)))
            {
                falling = true;
            }

            newDestination = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.75f);
            return new Vector2(transform.position.x - 0.43f, transform.position.y - 0.25f);
        }

        else if (player.position.x <= transform.position.x && player.position.y >= transform.position.y)
        {
            if (InBorder(new Vector2(-0.5f, 0.75f)))
            {
                falling = true;
            }

            newDestination = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.75f);
            return new Vector2(transform.position.x - 0.07f, transform.position.y + 0.5f);
        }

        else
        {
            return transform.position;
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
