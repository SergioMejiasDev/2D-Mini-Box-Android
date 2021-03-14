using UnityEngine;

/// <summary>
/// Class that controls the enemy IA.
/// </summary>
public class ComputerAI : MonoBehaviour
{
    [Header("Movement")]
    float speed = 3.5f;
    int ballPosition;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] GameObject ball = null;

    void Update()
    {
        if (ball.transform.position.y >= transform.position.y)
        {
            ballPosition = 1;
        }
        else
        {
            ballPosition = -1;
        }

        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Function called to reset the paddle position.
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        transform.position = new Vector2(5.75f, 0);
    }

    /// <summary>
    /// Function that controls the movement of the paddle.
    /// </summary>
    void Move()
    {
        rb.velocity = new Vector2(rb.velocity.x, ballPosition * speed);
    }
}