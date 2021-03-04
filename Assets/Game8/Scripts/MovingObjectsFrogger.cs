using UnityEngine;

/// <summary>
/// Class that is responsible for moving and destroying objects on the screen.
/// </summary>
public class MovingObjectsFrogger : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] int direction = 1;

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game8/Destructor"))
        {
            gameObject.SetActive(false);
        }
    }
}
