using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls the movement of the paddles.
/// </summary>
public class Paddle : MonoBehaviour
{
    float speed = 5;
    Rigidbody2D rb;
    AudioSource audioSource;
    bool moveUp = false;
    bool dontMove = true;
    Vector2 startPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager2.manager.PauseGame();
        }

        HandleMoving();
    }

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

    public void AllowMovement(bool upMovement)
    {
        dontMove = false;
        moveUp = upMovement;
    }

    public void DontAllowMovement()
    {
        dontMove = true;
    }

    void MoveUp()
    {
        rb.velocity = new Vector2(rb.velocity.x, speed);
    }

    void MoveDown()
    {
        rb.velocity = new Vector2(rb.velocity.x, -speed);
    }

    void StopMoving()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
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
}
