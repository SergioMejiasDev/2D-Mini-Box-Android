using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the main functions of the enemies.
/// </summary>
public class GhostMovement : MonoBehaviour
{
    [Header("Movement")]
    Vector3 startPosition;
    [SerializeField] Transform[] waypoints = null;
    int currentWaypoint = 0;
    public float speed = 0.1f;
    Rigidbody2D rb;
    
    [Header("Animation")]
    Animator anim;
    bool blueMode = false;

    [Header("Death")]
    bool isDead = false;

    private void OnEnable()
    {
        PacmanMovement.EatBigDot += ChangeState;
        PacmanMovement.PlayerDie += PlayerDeath;
        GameManager4.ResetPositions += ResetPosition;
        GameManager4.PlayerWin += PlayerDeath;
    }

    private void OnDisable()
    {
        PacmanMovement.EatBigDot -= ChangeState;
        PacmanMovement.PlayerDie -= PlayerDeath;
        GameManager4.ResetPositions -= ResetPosition;
        GameManager4.PlayerWin -= PlayerDeath;
    }

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            if (transform.position != waypoints[currentWaypoint].position)
            {
                Vector2 newPosition = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, speed);
                rb.MovePosition(newPosition);
            }

            else currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        Animation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!blueMode && !isDead)
            {
                collision.gameObject.GetComponent<PacmanMovement>().PlayerDeath();
            }
            
            else if (blueMode && !isDead)
            {
                GameManager4.manager4.EnemyEaten(transform.position);
                StopAllCoroutines();
                blueMode = false;
                isDead = true;
                anim.SetBool("BlueMode", false);
                anim.SetBool("LastSeconds", false);
                StartCoroutine(Death());
            }
        }
    }

    /// <summary>
    /// Function that manages the animations of the enemies.
    /// </summary>
    void Animation()
    {
        Vector2 direction;

        if (currentWaypoint >= 0)
        {
            direction = waypoints[currentWaypoint].position - transform.position;
        }

        else
        {
            direction = startPosition;
        }

        if (!blueMode && !isDead)
        {
            anim.SetFloat("DirX", direction.x);
            anim.SetFloat("DirY", direction.y);
        }

        else if (isDead)
        {
            if (currentWaypoint >= 0)
            {
                anim.SetFloat("DeadX", direction.x);
                anim.SetFloat("DeadY", direction.y);
            }
            
            else
            {
                anim.SetFloat("DeadX", 0);
                anim.SetFloat("DeadY", -1);
            }
        }

        else if (blueMode)
        {
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", 0);
        }
    }

    /// <summary>
    /// Function that changes the state of enemies when the player eats a big dot.
    /// </summary>
    void ChangeState()
    {
        if (!isDead)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeMode());
        }
    }

    /// <summary>
    /// Function called through delegate when player dies.
    /// </summary>
    void PlayerDeath()
    {
        speed = 0;
    }

    /// <summary>
    /// Function that resets the position and status of enemies.
    /// </summary>
    void ResetPosition()
    {
        transform.position = startPosition;
        currentWaypoint = 0;
        speed = 0.1f;
        blueMode = false;
        isDead = false;
        anim.SetFloat("DeadX", 0);
        anim.SetFloat("DeadY", 0);
        anim.SetBool("BlueMode", false);
        anim.SetBool("LastSeconds", false);
    }

    /// <summary>
    /// Coroutine that is active during the change of state of the enemies.
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeMode()
    {
        blueMode = true;
        anim.SetBool("LastSeconds", false);
        anim.SetBool("BlueMode", true);
        speed = 0.05f;
        yield return new WaitForSeconds(8);
        anim.SetBool("BlueMode", false);
        anim.SetBool("LastSeconds", true);
        yield return new WaitForSeconds(3);
        anim.SetBool("LastSeconds", false);
        blueMode = false;
        speed = 0.1f;
    }

    /// <summary>
    /// Corroutine started every time the enemy is eaten by the player.
    /// </summary>
    /// <returns></returns>
    IEnumerator Death()
    {
        while (currentWaypoint >= 0)
        {
            if (transform.position != waypoints[currentWaypoint].position)
            {
                Vector2 newPosition = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, 0.5f);
                rb.MovePosition(newPosition);
            }

            else
            {
                currentWaypoint = (currentWaypoint - 1) % waypoints.Length;
            }

            yield return null;
        }

        while (transform.position != startPosition)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, startPosition, speed);
            rb.MovePosition(newPosition);

            yield return null;
        }

        currentWaypoint = 0;
        anim.SetFloat("DeadX", 0);
        anim.SetFloat("DeadY", 0);
        isDead = false;
        speed = 0.1f;
    }
}
