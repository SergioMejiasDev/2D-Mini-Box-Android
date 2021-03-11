using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the missile movement.
/// </summary>
public class EnemyMissile : MonoBehaviour
{
    int direction;
    [SerializeField] PhotonView pv = null;

    [SerializeField] bool canDestruct = false;

    private void OnEnable()
    {
        if (transform.position.x < 0)
        {
            direction = 1;
            transform.localScale = new Vector2(-0.85f, 0.85f);
        }
        else
        {
            direction = -1;
            transform.localScale = new Vector2(0.85f, 0.85f);
        }
    }

    void Update()
    {
        if (pv != null && !pv.IsMine)
        {
            return;
        }

        transform.Translate(Vector2.right * 4 * direction * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game1/Ground"))
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
