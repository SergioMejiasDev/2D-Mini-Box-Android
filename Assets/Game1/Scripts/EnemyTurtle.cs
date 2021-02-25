using UnityEngine;

/// <summary>
/// Class that controls the enemy movement.
/// </summary>
public class EnemyTurtle : MonoBehaviour
{
    int direction = 1;
    [SerializeField] SpriteRenderer sr;

    private void OnEnable()
    {
        if (transform.position.x < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        FlipEnemy();
    }

    private void Update()
    {
        transform.Translate(Vector2.right * 3 * direction * Time.deltaTime);
    }

    /// <summary>
    /// Function in charge of correcting the enemy's flip.
    /// </summary>
    void FlipEnemy()
    {
        if (direction == 1)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Game1/Walls")))
        {
            direction *= -1;
            FlipEnemy();
        }
        if (other.gameObject.CompareTag("Game1/Pipe"))
        {
            gameObject.SetActive(false);
        }
    }
}
