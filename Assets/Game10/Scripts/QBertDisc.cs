using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the functions of the two lifting discs.
/// </summary>
public class QBertDisc : MonoBehaviour
{
    [Header("Movement")]
    bool isActive = false;
    Vector2 destination;
    Vector2 destination1;
    Vector2 destination2;
    Rigidbody2D player;

    [Header("Components")]
    [SerializeField] Rigidbody2D rb = null;
    [SerializeField] GameObject borderCollider = null;
    [SerializeField] AudioSource sound = null;

    void OnEnable()
    {
        player = rb;

        destination1 = new Vector2(transform.position.x, -0.28f);
        destination2 = new Vector2(0, 2.45f);

        destination = destination1;

        isActive = false;
    }

    void FixedUpdate()
    {
        if (!isActive)
        {
            return;
        }

        if ((Vector2)transform.position == destination1)
        {
            destination = destination2;
        }

        Vector2 newPosition = Vector2.MoveTowards(transform.position, destination, 0.05f);
        rb.MovePosition(newPosition);
        player.MovePosition(newPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = new Vector2(transform.position.x, -1.10f);

            player = collision.gameObject.GetComponent<Rigidbody2D>();

            borderCollider.SetActive(true);

            StartCoroutine(EnableDisc(collision.gameObject));
        }
    }

    /// <summary>
    /// Coroutine that deactivates the disk after reaching the top of the pyramid.
    /// </summary>
    /// <param name="playerObject">The player.</param>
    /// <returns></returns>
    IEnumerator EnableDisc(GameObject playerObject)
    {
        yield return new WaitForSeconds(1);

        isActive = true;

        sound.Play();

        yield return new WaitForSeconds(3);

        playerObject.GetComponent<QBertMovement>().ResetPosition();

        gameObject.SetActive(false);
    }
}
