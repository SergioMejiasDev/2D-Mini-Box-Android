using UnityEngine;
using Photon.Pun;

/// <summary>
/// Class that controls the movement of the ball in online mode.
/// </summary>
public class BallNetwork : MonoBehaviourPun
{
    float speed = 3.5f;
    [SerializeField] Rigidbody2D rb;

    void OnEnable()
    {
        ResetPosition();
    }

    /// <summary>
    /// Function called to reset the ball position.
    /// </summary>
    public void ResetPosition()
    {
        photonView.RPC("ResetPositionNetwork", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Function called to reset the ball position on the server.
    /// </summary>
    [PunRPC]
    void ResetPositionNetwork()
    {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;

        Launch();
    }

    /// <summary>
    /// Function that makes the ball start moving.
    /// </summary>
    void Launch()
    {
        if (NetworkManager.networkManager.playerNumber != 1)
        {
            return;
        }

        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        photonView.RPC("LaunchNetwork", RpcTarget.AllViaServer, x, y);
    }

    /// <summary>
    /// Function that makes the ball start moving on server.
    /// </summary>
    /// <param name="x">The direction of the ball on the x-axis.</param>
    /// <param name="y">The direction of the ball on the y-axis.</param>
    [PunRPC]
    void LaunchNetwork(float x, float y)
    {
        rb.velocity = new Vector2(speed * x, speed * y);
    }
}
