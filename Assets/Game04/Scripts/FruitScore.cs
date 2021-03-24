using UnityEngine;

/// <summary>
/// Class that manages the fruit score.
/// </summary>
public class FruitScore : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] GameObject scoreImage = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager4.manager4.FruitSound();
            GameManager4.manager4.UpdateScore(score);
            GameManager4.manager4.GenerateFruit();
            Destroy(Instantiate(scoreImage, transform.position, Quaternion.identity), 1);
            Destroy(gameObject);
        }
    }
}
