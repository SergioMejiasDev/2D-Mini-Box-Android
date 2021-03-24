using UnityEngine;

/// <summary>
/// Class that manages the AI of the flames.
/// </summary>
public class FlameMovement : MonoBehaviour
{
    [Header("Movement")]
    float speed = 1.5f;
    Vector2 direction = Vector2.right;
    float destination;

    [Header("Components")]
    [SerializeField] CapsuleCollider2D capsuleCollider;
    [SerializeField] Rigidbody2D rb;

    void Start()
    {
        ChooseDestination();
    }

    void Update()
    {
        if (direction == Vector2.right && transform.position.x < destination)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        else if (direction == Vector2.left && transform.position.x > destination)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        else if (direction == Vector2.up)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        else
        {
            ChooseDestination();
        }
    }

    /// <summary>
    /// Function that assigns a new destination to the flame.
    /// </summary>
    void ChooseDestination()
    {
        if (transform.position.y < -4.5f)
        {
            destination = Random.Range(-5.5f, 5.5f);
        }

        else
        {
            destination = Random.Range(-4.15f, 5.5f);
        }

        if (destination > transform.position.x)
        {
            direction = Vector2.right;
            transform.localScale = new Vector2(1, 1);
        }

        else
        {
            direction = Vector2.left;
            transform.localScale = new Vector2(-1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ladder")
        {
            if (Random.value > 0.25f)
            {
                return;
            }

            transform.position = new Vector2(collision.gameObject.transform.position.x, transform.position.y);
            direction = Vector2.up;

            capsuleCollider.isTrigger = true;
            rb.gravityScale = 0;
        }

        else if (collision.gameObject.name == "FlameCollider")
        {
            if (direction == Vector2.up)
            {
                capsuleCollider.isTrigger = false;
                rb.gravityScale = 1;

                ChooseDestination();
            }
        }

        else if (collision.gameObject.CompareTag("Game7/MalletHit"))
        {
            GameManager7.manager7.DestroyFlame(transform.position);

            Destroy(gameObject);
        }
    }
}
