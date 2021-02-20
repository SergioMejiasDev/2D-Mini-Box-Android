using System.Collections;
using UnityEngine;

/// <summary>
/// Class that controls the movement and firing of the entire wave of enemies.
/// </summary>
public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed = 50;
    float waitTime;
    [SerializeField] float minWait = 0.001f, maxWait = 0.75f;
    GameObject[] enemies;
    [SerializeField] float fireRate = 0.997f;
    public bool stopMoving;
    AudioSource audioSource;

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Game3/Enemy");
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Function that starts the movement of enemies.
    /// </summary>
    /// <param name="restart">It will be positive if we want the enemies to return to the starting position.</param>
    public void StartPlay(bool restart)
    {
        if (restart)
        {
            transform.position = new Vector2(0, 0.4f);
            waitTime = maxWait;
        }

        stopMoving = false;
        StartCoroutine(MoveEnemies());
    }

    /// <summary>
    /// Feature that makes enemies move down when reaching the edge.
    /// </summary>
    void MoveDown()
    {
        transform.Translate(Vector2.down * 0.4f);
        speed *= -1;
        StartCoroutine(MoveEnemies());
    }

    /// <summary>
    /// This function is called upon when an enemy dies, and causes the speed at which they move to increase.
    /// </summary>
    public void DecreaseWaitTime()
    {
        waitTime -= ((maxWait - minWait) / 55f);
    }

    /// <summary>
    /// Coroutine that manages the movement and shooting of enemies.
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveEnemies()
    {
        yield return new WaitForSeconds(waitTime);

        while (true)
        {
            if (stopMoving)
            {
                yield break;
            }

            transform.Translate(Vector2.right * speed);
            audioSource.Play();

            for (int i = 0; i < enemies.Length; i++)
            {
                if ((enemies[i].activeSelf == true) && (Random.value > fireRate))
                {
                    GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Game3/BulletEnemy");
                    
                    if (bullet != null)
                    {
                        bullet.SetActive(true);
                        bullet.transform.position = enemies[i].transform.position;
                        bullet.transform.rotation = Quaternion.identity;
                    }
                }

                if ((enemies[i].transform.position.y < -2) && (enemies[i].activeSelf == true))
                {
                    GameManager3.manager3.LoseHealth(4);
                    yield break;
                }
            }

            for (int i = 0; i < enemies.Length; i++)
            {
                if (((enemies[i].transform.position.x < -7) && (speed < 0)) || ((enemies[i].transform.position.x > 7) && (speed > 0)))
                {
                    if (enemies[i].activeSelf == true)
                    {
                        yield return new WaitForSeconds(waitTime);
                        MoveDown();
                        yield break;
                    }
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
