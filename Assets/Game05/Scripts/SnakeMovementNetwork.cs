using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the player's movement in online mode.
/// </summary>
public class SnakeMovementNetwork : MonoBehaviourPun, IPunObservable
{
    [Header("Movement")]
    Vector2 direction;
    [SerializeField] GameObject tailPrefab = null;
    List<Transform> tail = new List<Transform>();
    Transform[] activeTails;
    bool hasEaten = false;
    bool canMove = true;

    [Header("Components")]
    [SerializeField] PhotonView pv = null;

    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        if ((canMove) && (Time.timeScale == 1))
        {
            ChangeDirection();
        }

        if (Input.GetButtonDown("Cancel"))
        {
            OnlineManager5.onlineManager.PauseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Game5/Food"))
        {
            if (pv.IsMine)
            {
                OnlineManager5.onlineManager.PlaySound("Food");

                OnlineManager5.onlineManager.UpdateScore(10, NetworkManager.networkManager.playerNumber);
                
                OnlineManager5.onlineManager.Spawn();

                OnlineManager5.onlineManager.DestroyFood(collision.gameObject.GetComponent<PhotonView>().ViewID);

                hasEaten = true;
            }
        }

        else if (collision.gameObject.CompareTag("Game5/RedFood"))
        {
            if (pv.IsMine)
            {
                OnlineManager5.onlineManager.PlaySound("RedFood");

                OnlineManager5.onlineManager.UpdateScore(50, NetworkManager.networkManager.playerNumber);

                OnlineManager5.onlineManager.SpawnRed();

                OnlineManager5.onlineManager.DestroyFood(collision.gameObject.GetComponent<PhotonView>().ViewID);

                hasEaten = true;
            }
        }

        else if (collision.gameObject.CompareTag("Game5/Tail"))
        {
            if (pv.IsMine)
            {
                OnlineManager5.onlineManager.GameOver(NetworkManager.networkManager.playerNumber);
            }
        }

        else if (collision.gameObject.CompareTag("Game5/Border"))
        {
            if (pv.IsMine)
            {
                OnlineManager5.onlineManager.GameOver(NetworkManager.networkManager.playerNumber);
            }
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
    /// Function that cleans all the fragments of the snake's tail.
    /// </summary>
    public void CleanTailNetwork()
    {
        activeTails = tail.ToArray();

        if (activeTails != null)
        {
            for (int i = 0; i < activeTails.Length; i++)
            {
                if (activeTails[i] != null)
                {
                    Destroy(activeTails[i].gameObject);
                }
            }
        }

        tail.Clear();
    }

    /// <summary>
    /// Function that resets the position of the snake.
    /// </summary>
    public void ResetPosition()
    {
        photonView.RPC("ResetPositionNetwork", RpcTarget.AllViaServer, NetworkManager.networkManager.playerNumber);
    }

    /// <summary>
    /// Function that resets the position of the snake on the server.
    /// </summary>
    /// <param name="playerNumber">Player 1 or Player 2.</param>
    [PunRPC]
    void ResetPositionNetwork(int playerNumber)
    {
        CancelMoveNetwork();

        CleanTailNetwork();

        switch (playerNumber)
        {
            case 1:

                transform.position = new Vector2(0, 10);
                
                for (int i = 0; i < 5; i++)
                {
                    Vector2 tailPosition = new Vector2(transform.position.x - (i + 1), transform.position.y);
                    GameObject newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);

                    tail.Insert(i, newTail.transform);
                }

                direction = Vector2.right;

                break;

            case 2:

                transform.position = new Vector2(0, -10);

                for (int i = 0; i < 5; i++)
                {
                    Vector2 tailPosition = new Vector2(transform.position.x + (i + 1), transform.position.y);
                    GameObject newTail = Instantiate(tailPrefab, tailPosition, Quaternion.identity);

                    tail.Insert(i, newTail.transform);
                }

                direction = Vector2.left;

                break;
        }

        StartMoving();
    }

    /// <summary>
    /// Function that initiates the movement of the snake.
    /// </summary>
    void StartMoving()
    {
        InvokeRepeating("Move", 0.3f, 0.15f);
    }

    /// <summary>
    /// Function that stops the movement of the snake.
    /// </summary>
    public void CancelMove()
    {
        photonView.RPC("CancelMoveNetwork", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Function that stops the movement of the snake on the server.
    /// </summary>
    [PunRPC]
    void CancelMoveNetwork()
    {
        CancelInvoke();
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
            photonView.RPC("IncreaseSize", RpcTarget.All, position);
        }

        else if (tail.Count > 0)
        {
            tail.Last().position = position;

            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    /// <summary>
    /// Function that increases the size of the snake.
    /// </summary>
    /// <param name="position">Position of the new fragment.</param>
    [PunRPC]
    void IncreaseSize(Vector2 position)
    {
        GameObject newTail = Instantiate(tailPrefab, position, Quaternion.identity);

        tail.Insert(0, newTail.transform);

        hasEaten = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((Vector2)transform.position);
            stream.SendNext(direction);
        }

        else
        {
            transform.position = (Vector2)stream.ReceiveNext();
            direction = (Vector2)stream.ReceiveNext();
        }
    }
}
