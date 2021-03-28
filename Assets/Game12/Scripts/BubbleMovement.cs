using UnityEngine;

/// <summary>
/// Class that is in charge of the movement of the objects that fall in the game.
/// </summary>
public class BubbleMovement : MonoBehaviour
{
    float speed = 6.0f;
    float magnetSpeed = 2.0f;

    [SerializeField] bool normalBubble = false;
    bool magnetMode = false;
    Transform player;

    private void OnEnable()
    {
        speed = 6.0f;
        magnetSpeed = 2.0f;

        GameManager12.StopMovement += StopMovement;

        if (normalBubble)
        {
            magnetMode = GameManager12.manager.magnetMode;
            BalloonMovement.StartMagnetMode += StartMagnetMode;
        }
    }

    private void OnDisable()
    {
        GameManager12.StopMovement -= StopMovement;

        if (normalBubble)
        {
            BalloonMovement.StartMagnetMode -= StartMagnetMode;
        }
    }

    private void Start()
    {
        if (normalBubble)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (magnetMode && Vector2.Distance(transform.position, player.position) < 3f)
        {
            transform.Translate((player.position - transform.position) * magnetSpeed * Time.deltaTime);

            return;
        }

        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game12/Destructor"))
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Function called through the delegate to stop the movement.
    /// </summary>
    void StopMovement()
    {
        speed = 0f;
        magnetSpeed = 0f;
    }

    /// <summary>
    /// Function called through the delegate to activate the magnetism.
    /// </summary>
    /// <param name="enable">Enable or disable.</param>
    void StartMagnetMode(bool enable)
    {
        magnetMode = enable;
    }
}
