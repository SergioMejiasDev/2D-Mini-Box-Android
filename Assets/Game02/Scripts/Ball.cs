using UnityEngine;

/// <summary>
/// Class that controls the movement of the ball.
/// </summary>
public class Ball : MonoBehaviour
{
    float speed = 4;
    [SerializeField] Rigidbody2D rb = null;

    void Start()
    {
        Launch();
    }

    /// <summary>
    /// Function called to reset the ball position.
    /// </summary>
    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

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