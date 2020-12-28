using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerAI : MonoBehaviour
{
    float speed = 5;
    Rigidbody2D rb;
    AudioSource audioSource;
    Vector2 startPosition;
    [SerializeField] GameObject ball = null;
    int ballPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
    }


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
        transform.position = startPosition;
    }

    /// <summary>
    /// Function that controls the movement of the paddle.
    /// </summary>
    void Move()
    {
        rb.velocity = new Vector2(rb.velocity.x, ballPosition * speed);
    }
}
