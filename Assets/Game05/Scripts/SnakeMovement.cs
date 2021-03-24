using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that controls the movement of players.
/// </summary>
public class SnakeMovement : MonoBehaviour
{
    [Header("Movement")]
    Vector2 direction;
    List<Transform> tail = new List<Transform>();
    bool hasEaten = false;
    bool canMove = true;
    [SerializeField] GameObject tailPrefab = null;

    [Header("Sounds")]
    [SerializeField] AudioSource foodSound = null;
    [SerializeField] AudioSource redFoodSound = null;
    [SerializeField] AudioSource snakeHurtSound = null;

    void OnEnable()
    {
        tail.Clear();

        GameObject[] activeTail = GameObject.FindGameObjectsWithTag("Game5/Tail");

        if (activeTail != null)
        {
            for (int i = 0; i < activeTail.Length; i++)
            {
                Destroy(activeTail[i]);
            }
        }

        direction = Vector2.right;

        for (int i = 0; i < 5; i++)
        {
            Vector2 tailPosition = new Vector2(transform.position.x - (i + 1), transform.position.y);
            GameObject newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);
            tail.Insert(i, newTail.transform);
        }

        InvokeRepeating("Move", 0.3f, 0.15f);
    }

    void Update()
    {
        if ((canMove) && (Time.timeScale == 1))
        {
            ChangeDirection();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            GameManager5.manager5.PauseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game5/Food"))
        {
            foodSound.Play();
            hasEaten = true;

            GameManager5.manager5.UpdateScore(10);
            GameManager5.manager5.Spawn();

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Game5/RedFood"))
        {
            redFoodSound.Play();
            hasEaten = true;

            GameManager5.manager5.UpdateScore(50);
            GameManager5.manager5.SpawnRed();

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Game5/Tail"))
        {
            snakeHurtSound.Play();

            CancelInvoke();
            GameManager5.manager5.GameOver();
        }

        else if (collision.gameObject.CompareTag("Game5/Border"))
        {
            snakeHurtSound.Play();

            CancelInvoke();
            GameManager5.manager5.GameOver();
        }
    }

    /// <summary>
    /// Function that allows the player to change direction by dragging their finger across the screen.
    /// </summary>
    void ChangeDirection()
    {
        if (Input.touchCount > 0)
        {
            Touch firstDetectedTouch = Input.GetTouch(0);

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 dragDistance = firstDetectedTouch.deltaPosition.normalized;

                if (dragDistance.x > dragDistance.y)
                {
                    if ((dragDistance.x > 0.8f) && (direction != -Vector2.right))
                    {
                        direction = Vector2.right;
                        canMove = false;
                    }

                    else if ((dragDistance.y < -0.8f) && (direction != Vector2.up))
                    {
                        direction = -Vector2.up;
                        canMove = false;
                    }
                }

                else if (dragDistance.y > dragDistance.x)
                {
                    if ((dragDistance.y > 0.8f) && (direction != -Vector2.up))
                    {
                        direction = Vector2.up;
                        canMove = false;
                    }

                    else if ((dragDistance.x < -0.8f) && (direction != Vector2.right))
                    {
                        direction = -Vector2.right;
                        canMove = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Function that is called every time the player moves.
    /// </summary>
    void Move()
    {
        canMove = true;

        Vector2 position = transform.position;

        transform.Translate(direction);

        if (hasEaten)
        {
            GameObject newTail = Instantiate(tailPrefab, position, Quaternion.identity);

            tail.Insert(0, newTail.transform);

            hasEaten = false;
        }

        else if (tail.Count > 0)
        {
            tail.Last().position = position;

            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }
}
