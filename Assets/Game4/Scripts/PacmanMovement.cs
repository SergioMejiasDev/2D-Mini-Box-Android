using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the main functions of the player.
/// </summary>
public class PacmanMovement : MonoBehaviour
{
    public delegate void PacmanDelegate();
    public static event PacmanDelegate EatBigDot;
    public static event PacmanDelegate PlayerDie;

    [Header("Movement")]
    public float speed = 0.1f;
    Vector2 destination = Vector2.zero;
    Vector2 currentDirection = Vector2.right;
    Vector2 selectedDirection = Vector2.right;
    public Vector2 startPosition;
    bool isDie = false;

    [Header("Components")]
    [SerializeField] CircleCollider2D col;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;

    [Header("Sounds")]
    [SerializeField] AudioSource bigDotSound = null;
    [SerializeField] AudioSource dieSound = null;

    void Start()
    {
        destination = transform.position;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager4.manager4.PauseGame();
        }

        if (!isDie && Time.timeScale != 0)
        {
            ChangeDirection();
        }
    }

    void FixedUpdate()
    {
        if (!isDie)
        {
            if ((Vector2)transform.position == destination)
            {
                if (!RayMovement(1.1f * selectedDirection))
                {
                    currentDirection = selectedDirection;
                }

                if (!RayMovement(1.01f * currentDirection))
                {
                    ChangeDestination();
                }
            }

            Vector2 newPosition = Vector2.MoveTowards(transform.position, destination, speed);
            rb.MovePosition(newPosition);

            Vector2 direction = destination - (Vector2)transform.position;
            anim.SetFloat("DirX", direction.x);
            anim.SetFloat("DirY", direction.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game4/Dot"))
        {
            collision.gameObject.SetActive(false);
            GameManager4.manager4.DotEaten();
        }

        else if (collision.gameObject.CompareTag("Game4/BigDot"))
        {
            collision.gameObject.SetActive(false);
            bigDotSound.Play();
            GameManager4.manager4.UpdateScore(5);
            GameManager4.manager4.enemiesInScreen = 4;

            if (EatBigDot != null)
            {
                EatBigDot();
            }
        }
    }

    /// <summary>
    /// Boolean that is positive if the player can keep moving in the current direction.
    /// </summary>
    /// <param name="direction">Direction in which the player is moving.</param>
    /// <returns></returns>
    bool RayMovement(Vector2 direction)
    {
        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(position + direction, position);

        return (hit.collider.gameObject.CompareTag("Game4/Maze"));
    }

    /// <summary>
    /// Function called to assign a new destination to the player.
    /// </summary>
    void ChangeDestination()
    {
        if (currentDirection == Vector2.up)
        {
            destination = (Vector2)transform.position + Vector2.up;
        }

        else if (currentDirection == Vector2.right)
        {
            destination = (Vector2)transform.position + Vector2.right;
        }

        else if (currentDirection == Vector2.down)
        {
            destination = (Vector2)transform.position - Vector2.up;
        }

        else if (currentDirection == Vector2.left)
        {
            destination = (Vector2)transform.position - Vector2.right;
        }
    }

    /// <summary>
    /// Function called to change the direction of the player.
    /// </summary>
    void ChangeDirection()
    {
        if (Input.touchCount > 0)
        {
            Touch firstDetectedTouch = Input.GetTouch(0);

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 dragDistance = firstDetectedTouch.deltaPosition.normalized;

                if (dragDistance.x > dragDistance.y)
                {
                    if (dragDistance.x > 0.8f)
                    {
                        selectedDirection = Vector2.right;
                    }

                    else if (dragDistance.y < -0.8f)
                    {
                        selectedDirection = Vector2.down;
                    }
                }

                else if (dragDistance.y > dragDistance.x)
                {
                    if (dragDistance.y > 0.8f)
                    {
                        selectedDirection = Vector2.up;
                    }

                    else if (dragDistance.x < -0.8f)
                    {
                        selectedDirection = Vector2.left;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Function called when the player dies.
    /// </summary>
    public void PlayerDeath()
    {
        if (PlayerDie != null)
        {
            PlayerDie();
        }

        StartCoroutine(PlayerDying());
    }

    /// <summary>
    /// Function that resets the player's position.
    /// </summary>
    public void ResetPosition()
    {
        isDie = false;
        col.enabled = true;
        destination = transform.position;
        currentDirection = Vector2.right;
        selectedDirection = Vector2.right;
    }

    /// <summary>
    /// Coroutine initiated when the player dies and that manages several of its main functions.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerDying()
    {
        isDie = true;
        col.enabled = false;

        yield return new WaitForSeconds(0.5f);

        dieSound.Play();

        anim.SetFloat("DirX", 0);
        anim.SetFloat("DirY", 0);
        anim.SetBool("PlayerDie", true);

        yield return new WaitForSeconds(1.5f);

        GameManager4.manager4.PlayerDeath();

        gameObject.SetActive(false);
    }
}