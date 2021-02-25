using System.Collections;
using UnityEngine;

/// <summary>
/// Class that manages all player functions.
/// </summary>
public class MarioMovement : MonoBehaviour
{
    [Header("Walking")]
    float speed = 1.5f;
    float jump = 4.0f;
    [SerializeField] LayerMask groundMask = 0;
    bool isDead = false;

    [Header("Ladders")]
    float climbSpeed = 2.0f;
    bool inLadders = false;
    bool climbingLadders = false;
    Transform ladders;
    [SerializeField] LayerMask ladderMask = 0;
    [SerializeField] LayerMask descendMask = 0;

    [Header("Mallet")]
    bool malletMode = false;
    [SerializeField] GameObject topCollider;
    [SerializeField] GameObject rightCollider;

    [Header("Components")]
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BoxCollider2D boxCollider;

    [Header("Sounds")]
    [SerializeField] AudioSource jumpSound = null;
    [SerializeField] AudioSource deathSound = null;
    [SerializeField] AudioSource malletSound = null;

    [Header("Inputs")]
    int h = 0;
    int v = 0;

    /// <summary>
    /// Boolean to know if we are on the ground through a Raycast.
    /// </summary>
    /// <returns>True if we are on the ground, false if not.</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, boxCollider.bounds.extents.y + 0.2f, 0), Vector2.down, 0.1f, groundMask);
        Debug.DrawRay(transform.position - new Vector3(0, boxCollider.bounds.extents.y + 0.2f, 0), Vector2.down * 0.1f, Color.red);

        return hit;
    }

    /// <summary>
    /// Boolean to know if we have reached the edge of the ladders through a Raycast.
    /// </summary>
    /// <returns>True if we are, false if not.</returns>
    bool InBorderOfLadders()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, boxCollider.bounds.extents.y + 0.2f, 0), Vector2.down, 0.1f, ladderMask);
        Debug.DrawRay(transform.position - new Vector3(0, boxCollider.bounds.extents.y + 0.2f, 0), Vector2.down * 0.01f, Color.blue);

        return hit;
    }

    /// <summary>
    /// Boolean to know if we are on some stairs that we can go down through a Raycast.
    /// </summary>
    /// <returns>True if we are, false if not.</returns>
    bool CanDescend()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, boxCollider.bounds.extents.y + 0.2f, 0), Vector2.down, 0.1f, descendMask);
        Debug.DrawRay(transform.position - new Vector3(0, boxCollider.bounds.extents.y + 0.2f, 0), Vector2.down * 0.01f, Color.green);

        if (hit)
        {
            ladders = hit.transform;
        }

        return hit;
    }

    private void OnEnable()
    {
        GameManager7.Stop += StopAnimation;
        GameManager7.Reset += ResetScene;
    }

    private void OnDisable()
    {
        GameManager7.Stop -= StopAnimation;
        GameManager7.Reset -= ResetScene;
    }

    void Update()
    {        
        if (Time.timeScale == 1 && !isDead)
        {
            if (!climbingLadders)
            {
                Movement();

                if (!malletMode)
                {
                    UseLadders();
                }
            }

            else if (climbingLadders)
            {
                MovementInLadders();

                if (InBorderOfLadders())
                {
                    LeaveLadders();
                }
            }

            Animation();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager7.manager7.PauseGame();
        }
    }

    /// <summary>
    /// Function called to activate the inputs on the touch screen.
    /// </summary>
    /// <param name="input">1 up, 2 right, 3 down, 4 left.</param>
    public void EnableInput(int input)
    {
        switch (input)
        {
            case 1:
                v = 1;
                break;

            case 2:
                h = 1;
                break;

            case 3:
                v = -1;
                break;

            case 4:
                h = -1;
                break;
        }
    }

    /// <summary>
    /// Function called to deactivate the inputs on the touch screen.
    /// </summary>
    /// <param name="input">1 up, 2 right, 3 down, 4 left.</param>
    public void DisableInput(int input)
    {
        switch (input)
        {
            case 1:
                v = 0;
                break;

            case 2:
                h = 0;
                break;

            case 3:
                v = 0;
                break;

            case 4:
                h = 0;
                break;
        }
    }

    /// <summary>
    /// Function called to make the player move.
    /// </summary>
    void Movement()
    {
        if (h > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        else if (h < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime * h);
    }

    /// <summary>
    /// Function that makes the player move on the ladders.
    /// </summary>
    void MovementInLadders()
    {
        transform.Translate(Vector2.up * climbSpeed * Time.deltaTime * v);
    }

    /// <summary>
    /// Function that activates character animations.
    /// </summary>
    void Animation()
    {
        anim.SetBool("IsWalking", (h != 0) && IsGrounded() && !climbingLadders);
        anim.SetBool("IsJumping", !IsGrounded() && !climbingLadders);
        anim.SetBool("IsClimbingIddle", (v == 0) && climbingLadders);
        anim.SetBool("IsClimbingMovement", (v != 0) && climbingLadders);
    }

    /// <summary>
    /// Function called to make the player jump.
    /// </summary>
    public void Jump()
    {
        if (!climbingLadders && IsGrounded() && (!inLadders || malletMode))
        {
            jumpSound.Play();

            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Function that makes the player climb the ladders to go up or down.
    /// </summary>
    void UseLadders()
    {
        if (v == 1 && inLadders)
        {
            transform.position = new Vector3(ladders.position.x, transform.position.y, 0);
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            climbingLadders = true;
        }

        else if (v == -1 && !climbingLadders && CanDescend())
        {
            transform.position = new Vector3(ladders.position.x, transform.position.y - 0.2f, 0);
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            climbingLadders = true;
        }
    }

    /// <summary>
    /// Function that makes the player get off the ladders if he has reached the limit.
    /// </summary>
    void LeaveLadders()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        climbingLadders = false;
    }

    /// <summary>
    /// Function that activates the colliders of the mullet.
    /// </summary>
    /// <param name="isTopCollider">1 if it is the top collider, 0 if it is the right one.</param>
    public void MalletCollider(int isTopCollider)
    {
        if (isTopCollider == 1)
        {
            topCollider.SetActive(true);
            rightCollider.SetActive(false);
        }

        else
        {
            topCollider.SetActive(false);
            rightCollider.SetActive(true);
        }
    }

    /// <summary>
    /// Function called through the delegate to reset the position of the player in the scene.
    /// </summary>
    void ResetScene()
    {
        boxCollider.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = Vector2.zero;

        inLadders = false;
        climbingLadders = false;

        anim.SetBool("IsDying", false);
        anim.SetBool("IsDie", false);

        transform.position = new Vector2(-4.1f, -4.2f);
        transform.localScale = new Vector2(1f, 1f);

        isDead = false;
    }

    /// <summary>
    /// Function that stops all player animations.
    /// </summary>
    void StopAnimation()
    {
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsJumping", false);
        anim.SetBool("IsClimbingIddle", false);
        anim.SetBool("IsClimbingMovement", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game7/Barrel"))
        {
            if (malletMode)
            {
                return;
            }

            StartCoroutine(Dying());
        }

        else if (collision.gameObject.CompareTag("Game7/Flame"))
        {
            if (malletMode)
            {
                return;
            }

            StartCoroutine(Dying());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game7/Ladders"))
        {
            ladders = collision.gameObject.transform;
        }

        else if (collision.gameObject.CompareTag("Game7/Mallet"))
        {
            collision.gameObject.SetActive(false);

            malletSound.Play();
            
            StartCoroutine(ModeMallet());
        }

        else if (collision.gameObject.CompareTag("Game7/JumpArea"))
        {
            if (!climbingLadders)
            {
                GameManager7.manager7.JumpBarrel(collision.gameObject.transform.position);
            }
        }
        
        else if (collision.gameObject.name == "WinCollider")
        {
            if (!climbingLadders)
            {
                GameManager7.manager7.WinGame();
            }
        }

        else if (collision.gameObject.CompareTag("Game7/Barrel"))
        {
            if (malletMode)
            {
                return;
            }

            StartCoroutine(Dying());
        }

        else if (collision.gameObject.CompareTag("Game7/Flame"))
        {
            if (malletMode)
            {
                return;
            }

            StartCoroutine(Dying());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game7/Ladders"))
        {
            if (!inLadders)
            {
                inLadders = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game7/Ladders"))
        {
            inLadders = false;
        }
    }

    /// <summary>
    /// Coroutine that starts when the player picks up a mallet.
    /// </summary>
    /// <returns></returns>
    IEnumerator ModeMallet()
    {
        malletMode = true;
        anim.SetBool("ActivateMallet", true);

        yield return new WaitForSeconds(7);

        malletMode = false;
        anim.SetBool("ActivateMallet", false);

        topCollider.SetActive(false);
        rightCollider.SetActive(false);
    }

    /// <summary>
    /// Coroutine that starts when the player dies.
    /// </summary>
    /// <returns></returns>
    IEnumerator Dying()
    {
        deathSound.Play();

        isDead = true;
        boxCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;

        StopAnimation();
        GameManager7.manager7.CleanScene();
        GameManager7.manager7.StopGame();

        yield return new WaitForSeconds(1f);

        anim.SetBool("IsDying", true);

        yield return new WaitForSeconds(1.5f);

        anim.SetBool("IsDying", false);
        anim.SetBool("IsDie", true);

        yield return new WaitForSeconds(1.5f);

        GameManager7.manager7.GameOver();
    }
}
