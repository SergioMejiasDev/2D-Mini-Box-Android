using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that is responsible for updating the score when someone scores.
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField] bool isPlayer1Goal = false;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
