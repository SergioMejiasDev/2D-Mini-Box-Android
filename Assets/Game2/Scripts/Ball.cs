using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that controls the movement of the ball.
/// </summary>
public class Ball : MonoBehaviour
{
    float speed = 6;
    Rigidbody2D rb;
    Vector3 startPosition;
    int direction = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        Launch();
    }

    /// <summary>
    /// Function called to reset the ball position.
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
        Launch();
    }

    /// <summary>
    /// Function that makes the ball start moving.
    /// </summary>
    void Launch()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(speed * x, speed * y);
        if (y == 1)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Wall"))
        {
            direction *= -1;
        }
    }
}
