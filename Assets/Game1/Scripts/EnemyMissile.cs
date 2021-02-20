using UnityEngine;

/// <summary>
/// Class that controls the missile movement.
/// </summary>
public class EnemyMissile : MonoBehaviour
{
    int direction;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (transform.position.x < 0)
        {
            direction = 1;
            sr.flipX = true;
        }
        else
        {
            direction = -1;
            sr.flipX = false;
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * 4 * direction * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game1/Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
