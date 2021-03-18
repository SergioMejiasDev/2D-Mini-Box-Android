using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages the main functions of the frog.
/// </summary>
public class FrogMovement : MonoBehaviour
{
    [Header("Movement")]
    float speed = 0.1f;
    Vector2 destination = Vector2.zero;
    [SerializeField] LayerMask borderMask = 8;
    bool isDie = false;
    bool inputsEnabled = false;

    [Header("Water Area")]
    GameObject parent;
    bool moving = false;
    bool inWaterArea = false;
    bool hasChecked = true;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] Animator anim = null;
    [SerializeField] CircleCollider2D circleCollider = null;

    [Header("Sounds")]
    [SerializeField] AudioSource jumpSound = null;
    [SerializeField] AudioSource plunkSound = null;
    [SerializeField] AudioSource squashSound = null;

    /// <summary>
    /// Boolean to check if the player can move in one direction.
    /// </summary>
    /// <param name="direction">The direction we want to check.</param>
    /// <returns>True if we can move, false if not.</returns>
    bool RayMovement(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, borderMask);

        return hit;
    }

    void Start()
    {
        destination = transform.position;
    }

    private void Update()
    {
        if ((Vector2)transform.position == destination)
        {
            if (!hasChecked)
            {
                hasChecked = true;

                if (inWaterArea && !moving)
                {
                    plunkSound.Play();
                    ResetValues();
                    StartCoroutine(Die("Plunk"));
                }
            }

            if (!inputsEnabled)
            {
                inputsEnabled = true;
            }
        }

        else if (moving)
        {
            if (!inputsEnabled)
            {
                inputsEnabled = true;
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager8.manager8.PauseGame();
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = Vector2.MoveTowards(transform.position, destination, speed);
        rb.MovePosition(newPosition);

        Vector2 direction = destination - (Vector2)transform.position;
        anim.SetFloat("DirX", direction.x);
        anim.SetFloat("DirY", direction.y);
        anim.SetBool("InWater", moving);
    }

    /// <summary>
    /// Function that allows us to call the movement inputs.
    /// </summary>
    /// <param name="input">1 up, 2 right, 3 down, 4 left.</param>
    public void EnterInputs(int input)
    {
        if (!inputsEnabled || Time.timeScale != 1 || isDie)
        {
            return;
        }

        if (input == 1 && !RayMovement(Vector2.up))
        {
            jumpSound.Play();
            ResetValues();
            destination = (Vector2)transform.position + 0.5f * Vector2.up;
        }

        if (input == 2 && !RayMovement(Vector2.right))
        {
            jumpSound.Play();
            ResetValues();
            destination = (Vector2)transform.position + 0.45f * Vector2.right;
        }

        if (input == 3 && !RayMovement(-Vector2.up))
        {
            jumpSound.Play();
            ResetValues();
            destination = (Vector2)transform.position - 0.5f * Vector2.up;
        }

        if (input == 4 && !RayMovement(-Vector2.right))
        {
            jumpSound.Play();
            ResetValues();
            destination = (Vector2)transform.position - 0.45f * Vector2.right;
        }
    }

    /// <summary>
    /// Function that resets the movement values in the water zone.
    /// </summary>
    void ResetValues()
    {
        moving = false;
        parent = null;
        transform.SetParent(null);

        hasChecked = false;
    }

    /// <summary>
    /// Function that resets all the player's values after dying.
    /// </summary>
    public void ResetPosition()
    {
        isDie = false;
        inWaterArea = false;
        circleCollider.enabled = true;

        ResetValues();

        transform.position = new Vector2(0, -3.35f);
        destination = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Mask2"))
        {
            parent = collision.gameObject;
            moving = true;
            inWaterArea = true;
            transform.position = parent.transform.position;
            transform.parent = parent.transform;
        }

        if (collision.gameObject.CompareTag("Game8/Water"))
        {
            inWaterArea = true;
        }

        if (collision.gameObject.name == "Line")
        {
            collision.gameObject.SetActive(false);
            GameManager8.manager8.UpdateScore(10);
        }

        if (collision.gameObject.name == "Frog")
        {
            GameManager8.manager8.FrogRescued(collision.gameObject);

            ResetPosition();
        }

        if (collision.gameObject.CompareTag("Game8/DeathZone"))
        {
            ResetValues();
            plunkSound.Play();
            StartCoroutine(Die("Plunk"));
        }

        else if (collision.gameObject.CompareTag("Game8/Car1") ||
            collision.gameObject.CompareTag("Game8/Car2") ||
            collision.gameObject.CompareTag("Game8/Car3") ||
            collision.gameObject.CompareTag("Game8/Car4") ||
            collision.gameObject.CompareTag("Game8/Car5"))
        {
            ResetValues();
            squashSound.Play();
            StartCoroutine(Die("Squash"));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game8/Water"))
        {
            inWaterArea = false;
        }
    }

    /// <summary>
    /// Corroutine that starts the death animation and deactivates the player.
    /// </summary>
    /// <param name="animation">The string parameter of the animation that we want to start.</param>
    /// <returns></returns>
    IEnumerator Die(string animation)
    {
        isDie = true;
        circleCollider.enabled = false;

        anim.SetBool(animation, true);

        yield return new WaitForSeconds(2);

        anim.SetBool(animation, false);

        GameManager8.manager8.IsDie();

        gameObject.SetActive(false);
    }
}
