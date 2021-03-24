using UnityEngine;

/// <summary>
/// Class that is responsible for updating the score when someone scores.
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField] bool isPlayer1Goal = false;
    [SerializeField] AudioSource audioSource = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game2/Ball"))
        {
            if (!NetworkManager.networkManager.isConnected)
            {
                if (isPlayer1Goal)
                {
                    GameManager2.manager.UpdateScore(2);
                }

                else
                {
                    GameManager2.manager.UpdateScore(1);
                }
            }

            else if (NetworkManager.networkManager.playerNumber == 1)
            {
                if (isPlayer1Goal)
                {
                    OnlineManager2.onlineManager.UpdateScore(2);
                }

                else
                {
                    OnlineManager2.onlineManager.UpdateScore(1);
                }
            }

            audioSource.Play();
        }
    }
}