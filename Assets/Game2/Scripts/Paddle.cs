using UnityEngine;

/// <summary>
/// Class that controls the movement of the paddles.
/// </summary>
public class Paddle : MonoBehaviour
{
    [Header("Movement")]
    float speed = 3.5f;
    bool moveUp = false;
    bool dontMove = true;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] AudioSource audioSource;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager2.manager.PauseGame();
        }

        HandleMoving();
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
            if (moveUp)
            {
                MoveUp();
            }
            else if (!moveUp)
            {
                MoveDown();
            }
        }
    }

    /// <summary>
    /// Function that allows the player to move.
    /// </summary>
    /// <param name="upMovement">True if the movement is upwards, false if it is downwards.</param>
    public void AllowMovement(bool upMovement)
    {
        dontMove = false;
        moveUp = upMovement;
    }

    /// <summary>
    /// Function that cancels the player's movement.
    /// </summary>
    public void DontAllowMovement()
    {
        dontMove = true;
    }

    /// <summary>
    /// Function that moves the player up.
    /// </summary>
    void MoveUp()
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
    }

    /// <summary>
    /// Function that moves the player down.
    /// </summary>
    void MoveDown()
    {
        rb.velocity = new Vector2(rb.velocity.x, -speed);
    }

    /// <summary>
    /// Function that keeps the player in position.
    /// </summary>
    void StopMoving()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    /// <summary>
    /// Function called to reset the paddle position.
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector2(-5.75f, 0);
        DontAllowMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            audioSource.Play();
        }
    }
}