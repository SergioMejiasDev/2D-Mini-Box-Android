using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that controls the movement of players.
/// </summary>
public class SnakeMovement : MonoBehaviour
{
    [SerializeField] bool isPlayer2 = false;

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

        if (!isPlayer2)
        {
            GameObject[] activeTail = GameObject.FindGameObjectsWithTag("Game5/Tail");

            if (activeTail != null)
            {
                for (int i = 0; i < activeTail.Length; i++)
                {
                    Destroy(activeTail[i]);
                }
            }

            direction = Vector2.right;

            Vector2 tailPosition = new Vector2(transform.position.x - 1, transform.position.y);
            GameObject newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);
            tail.Insert(0, newTail.transform);

            tailPosition = new Vector2(transform.position.x - 2, transform.position.y);
            newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);
            tail.Insert(1, newTail.transform);

            InvokeRepeating("Move", 0.3f, 0.15f);
        }

        else
        {
            direction = -Vector2.right;

            Vector2 tailPosition = new Vector2(transform.position.x + 1, transform.position.y);
            GameObject newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);
            tail.Insert(0, newTail.transform);

            tailPosition = new Vector2(transform.position.x + 2, transform.position.y);
            newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);
            tail.Insert(1, newTail.transform);

            InvokeRepeating("Move", 0.3f, 0.15f);
        }
    }

    void Update()
    {
        if (canMove)
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

            GameManager5.manager5.UpdateScore(10, isPlayer2);
            GameManager5.manager5.Spawn();

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Game5/RedFood"))
        {
            redFoodSound.Play();
            hasEaten = true;

            GameManager5.manager5.UpdateScore(50, isPlayer2);
            GameManager5.manager5.SpawnRed();

            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Game5/Tail"))
        {
            snakeHurtSound.Play();

            if (!GameManager5.manager5.multiplayer)
            {
                CancelInvoke();
                GameManager5.manager5.GameOver();
            }

            else
            {
                GameManager5.manager5.Victory(isPlayer2);
            }
        }

        else if (collision.gameObject.CompareTag("Game5/Border"))
        {
            snakeHurtSound.Play();

            if (!GameManager5.manager5.multiplayer)
            {
                CancelInvoke();
                GameManager5.manager5.GameOver();
            }

            else
            {
                GameManager5.manager5.Victory(isPlayer2);
            }
        }
    }

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
                    }

                    else if ((dragDistance.y < -0.8f) && (direction != Vector2.up))
                    {
                        direction = -Vector2.up;
                    }
                }

                else if (dragDistance.y > dragDistance.x)
                {
                    if ((dragDistance.y > 0.8f) && (direction != -Vector2.up))
                    {
                        direction = Vector2.up;
                    }

                    else if ((dragDistance.x < -0.8f) && (direction != Vector2.right))
                    {
                        direction = -Vector2.right;
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

        Vector2 v = transform.position;

        transform.Translate(direction);

        if (hasEaten)
        {
            GameObject newTail = Instantiate(tailPrefab, v, Quaternion.identity);

            tail.Insert(0, newTail.transform);

            hasEaten = false;
        }

        else if (tail.Count > 0)
        {
            tail.Last().position = v;

            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }
}
