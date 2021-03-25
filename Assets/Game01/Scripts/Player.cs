using UnityEngine;

/// <summary>
/// Class that takes care of the player's movement.
/// </summary>
public class Player : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    float speed = 4;
    float jump = 9.5f;
    bool dontMove = true;
    bool moveLeft = false;
    [SerializeField] LayerMask groundMask = 0;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] AudioSource audioSource;

    [Header("Sounds")]
    [SerializeField] AudioSource hurtSound = null;

    #endregion

    /// <summary>
    /// Boolean that is true if the player is in contact with the ground.
    /// </summary>
    /// <returns>True if the player is in contact with the ground, false if it isn't.</returns>
    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0, sr.bounds.extents.y + 0.01f, 0), Vector2.down, 0.1f, groundMask);
        
        return hit;
    }

    private void OnEnable()
    {
        transform.position = new Vector2(-6.3f, -5.4f);
        DontAllowMovement();
    }

    void Update()
    {
        HandleMoving();

        Animation();
    }

    /// <summary>
    /// Function that manages the movement of the player.
    /// </summary>
    void HandleMoving()
    {
        if (dontMove)
        {
            StopMoving();
        }

        else
        {
            if (moveLeft)
            {
                MoveLeft();
            }
            else if (!moveLeft)
            {
                MoveRight();
            }
        }
    }

    /// <summary>
    /// Function that allows the player to move.
    /// </summary>
    /// <param name="leftMovement">True if the movement is to the left, false if it is to the right.</param>
    public void AllowMovement(bool leftMovement)
    {
        dontMove = false;
        moveLeft = leftMovement;
    }

    /// <summary>
    /// Function that cancels the player's movement.
    /// </summary>
    public void DontAllowMovement()
    {
        dontMove = true;
    }

    /// <summary>
    /// Function that moves the character to the left.
    /// </summary>
    void MoveLeft()
    {
        transform.Translate(Vector2.right * -speed * Time.deltaTime);
        transform.localScale = new Vector2(-0.85f, 0.85f);
    }

    /// <summary>
    /// Function that moves the character to the right.
    /// </summary>
    void MoveRight()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        transform.localScale = new Vector2(0.85f, 0.85f);
    }

    /// <summary>
    /// Function that keeps the player in position.
    /// </summary>
    void StopMoving()
    {
        transform.Translate(Vector2.right * 0 * Time.deltaTime);
    }

    /// <summary>
    /// Function that activates character animations.
    /// </summary>
    void Animation()
    {
        anim.SetBool("IsWalking", (!dontMove && IsGrounded()));
        anim.SetBool("IsJumping", !IsGrounded());
    }

    /// <summary>
    /// Function we call to make the player jump.
    /// </summary>
    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            audioSource.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Game1/Enemy")) || (other.gameObject.CompareTag("Game1/Missile")))
        {
            gameObject.SetActive(false);
            GameManager1.manager.GameOver();

            hurtSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Game1/Coin"))
        {
            other.gameObject.SetActive(false);
            GameManager1.manager.UpdateScore();
        }
    }
}