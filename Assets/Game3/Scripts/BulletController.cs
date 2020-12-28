using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that is responsible for the movement and destruction of the bullets.
/// </summary>
public class BulletController : MonoBehaviour
{
    [SerializeField] bool isEnemy = false;
    float speedPlayer = 9;
    float speedEnemy = 5;
    
    void Update()
    {
        if (isEnemy)
        {
            transform.Translate(Vector2.down * speedEnemy * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * speedPlayer * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game3/Limits"))
        {
            gameObject.SetActive(false);
        }
    }
}
