using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that manages the main functions of the bases.
/// </summary>
public class BaseHealth : MonoBehaviour
{
    int health = 10;
    [SerializeField] GameObject piece1 = null, piece2 = null, piece3 = null;
    SpriteRenderer sr;

    public void Restart()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        piece1.SetActive(true);
        piece2.SetActive(false);
        piece3.SetActive(false);
        health = 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Game3/BulletPlayer")) || (collision.gameObject.CompareTag("Game3/BulletEnemy")))
        {
            health--;
            collision.gameObject.SetActive(false);
            
            if (health == 8)
            {
                piece1.SetActive(false);
                piece2.SetActive(true);
            }
            else if (health == 5)
            {
                piece2.SetActive(false);
                piece3.SetActive(true);
            }
            else if (health == 2)
            {
                piece3.SetActive(false);
                sr.enabled = true;
            }
            else if (health == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
