using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the enemy movement.
/// </summary>
public class EnemyTurtle : MonoBehaviour
{
    int direction = 1;

    [SerializeField] bool canDestruct = false;

    [SerializeField] PhotonView pv = null;

    private void OnEnable()
    {
        if (transform.position.x < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        FlipEnemy();
    }

    private void Update()
    {
        if (pv != null && !pv.IsMine)
        {
            return;
        }

        transform.Translate(Vector2.right * 3 * direction * Time.deltaTime);
    }

    /// <summary>
    /// Function in charge of correcting the enemy's flip.
    /// </summary>
    void FlipEnemy()
    {
        if (direction == 1)
        {
            transform.localScale = new Vector2(-0.85f, 0.85f);
        }
        else
        {
            transform.localScale = new Vector2(0.85f, 0.85f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.CompareTag("Game1/Walls")))
        {
            direction *= -1;

            FlipEnemy();
        }

        else if (other.gameObject.CompareTag("Game1/Pipe"))
        {
            if (canDestruct)
            {
                if (pv.IsMine)
                {
                    PhotonNetwork.Destroy(gameObject);
                }

                return;
            }

            gameObject.SetActive(false);
        }
    }
}

