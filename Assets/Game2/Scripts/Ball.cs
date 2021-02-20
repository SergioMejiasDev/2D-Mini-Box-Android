using UnityEngine;

/// <summary>
/// Class that controls the movement of the ball.
/// </summary>
public class Ball : MonoBehaviour
{
    float speed = 4;
    Rigidbody2D rb;
    Vector3 startPosition;

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
    }
}
