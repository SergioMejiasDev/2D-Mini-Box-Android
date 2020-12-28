using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages the main functions of the UFO.
/// </summary>
public class UFO : MonoBehaviour
{
    int score;

    private void Start()
    {
        int randomNumber = Random.Range(0, 3);

        if (randomNumber == 0)
        {
            score = 50;
        }

        else if (randomNumber == 1)
        {
            score = 100;
        }

        else if (randomNumber == 2)
        {
            score = 150;
        }
    }

    private void Update()
    {
        transform.Translate(Vector2.left * 2.5f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game3/BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
            GameManager3.manager3.UFODeath(score, transform.position);
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Game3/Limits"))
        {
            Destroy(gameObject);
        }
    }
}
