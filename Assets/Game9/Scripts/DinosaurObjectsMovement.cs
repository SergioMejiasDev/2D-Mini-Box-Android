using UnityEngine;

/// <summary>
/// Class that automates the movement of objects on the screen.
/// </summary>
public class DinosaurObjectsMovement : MonoBehaviour
{
    [SerializeField] bool variableSpeed = false;

    [SerializeField] float speed = 1.5f;

    private void OnEnable()
    {
        if (variableSpeed)
        {
            speed = GameManager9.manager.speed;

            GameManager9.ChangeSpeed += ChangeSpeed;
        }

        GameManager9.StopMovement += ChangeSpeed;
    }

    private void OnDisable()
    {
        if (variableSpeed)
        {
            GameManager9.ChangeSpeed -= ChangeSpeed;
        }

        GameManager9.StopMovement -= ChangeSpeed;
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game9/Destructor"))
        {
            gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            if (variableSpeed)
            {
                GameManager9.manager.GameOver();
            }
        }
    }

    /// <summary>
    /// Function called through the delegate to change the speed of the object.
    /// </summary>
    /// <param name="newSpeed">The speed we want to set.</param>
    void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
