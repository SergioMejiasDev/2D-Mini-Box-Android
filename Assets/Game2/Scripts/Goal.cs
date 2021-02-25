using UnityEngine;

/// <summary>
/// Class that is responsible for updating the score when someone scores.
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField] bool isPlayer1Goal = false;
    [SerializeField] AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            if (isPlayer1Goal == true)
            {
                GameManager2.manager.UpdateScore(2);
                audioSource.Play();
            }
            else
            {
                GameManager2.manager.UpdateScore(1);
                audioSource.Play();
            }
        }
    }
}
