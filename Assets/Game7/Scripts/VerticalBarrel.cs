using UnityEngine;

/// <summary>
/// Class that manages the movement of the vertical barrels.
/// </summary>
public class VerticalBarrel : MonoBehaviour
{
    float speed = 2.5f;

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Drum")
        {
            GameManager7.manager7.SpawnFlame(true);
            Destroy(gameObject);
        }
    }
}
