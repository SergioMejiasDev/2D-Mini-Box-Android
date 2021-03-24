using UnityEngine;

/// <summary>
/// Class that is responsible for managing the operation of the barrels.
/// </summary>
public class RollingBarrel : MonoBehaviour
{
    [Header("Movement")]
    float speed = 2.5f;
    Vector2 direction = Vector2.right;

    [Header("Points")]
    [SerializeField] GameObject pointsCollider = null;

    [Header("Components")]
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Animator anim;
    [SerializeField] CapsuleCollider2D capsuleCollider;

    private void OnEnable()
    {
        speed = 2.5f;
        direction = Vector2.right;
        
        capsuleCollider.isTrigger = false;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    /// <summary>
    /// Function that flip the barrel based on direction.
    /// </summary>
    void FlipBarrel()
    {
        speed *= -1;

        sr.flipX = !sr.flipX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Game7/Walls"))
        {
            if (Random.value < 0.6f)
            {
                capsuleCollider.isTrigger = true;
            }

            else
            {
                FlipBarrel();
            }
        }

        else if (collision.gameObject.name == "Drum")
        {
            GameManager7.manager7.SpawnFlame(false);

            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game7/MalletHit"))
        {
            GameManager7.manager7.DestroyBarrel(transform.position);

            gameObject.SetActive(false);
        }

        else if (collision.gameObject.name == "TopCollider")
        {
            float randomNumber = Random.value;

            if (randomNumber < 0.5f)
            {
                transform.position = new Vector2(collision.gameObject.transform.position.x, transform.position.y);

                anim.SetBool("InLadders", true);
                
                direction = Vector2.down;
                speed = 1.5f;

                pointsCollider.SetActive(false);
                capsuleCollider.isTrigger = true;
            }
        }

        else if (collision.gameObject.name == "BottomCollider")
        {
            if (direction == Vector2.down)
            {
                transform.position = new Vector2(transform.position.x, collision.gameObject.transform.position.y);

                anim.SetBool("InLadders", false);

                direction = Vector2.right;
                speed = 2.5f;

                pointsCollider.SetActive(true);
                capsuleCollider.isTrigger = false;

                if (transform.position.x > -0.1f)
                {
                    FlipBarrel();
                }
            }
        }

        else if (collision.gameObject.name == "DownLimit")
        {
            gameObject.SetActive(false);
        }
    }
}
