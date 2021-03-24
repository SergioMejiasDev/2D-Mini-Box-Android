using UnityEngine;

/// <summary>
/// Class that manages the score and destroying enemies individually.
/// </summary>
public class EnemyScore : MonoBehaviour
{
    [SerializeField] int score = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game3/BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
            GameManager3.manager3.EnemyDeath(score, transform.position);
            gameObject.SetActive(false);
        }
    }
}
