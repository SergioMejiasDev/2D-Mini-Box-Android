using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Class that manages the online functions of Game 02.
/// </summary>
public class OnlineManager2 : MonoBehaviourPunCallbacks
{
    public static OnlineManager2 onlineManager;

    [Header("Player")]
    GameObject player;
    PaddleNetwork playerClass;

    [Header("Ball")]
    [SerializeField] GameObject ball = null;
    [SerializeField] BallNetwork ballClass = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelLostPlayer1 = null;
    [SerializeField] GameObject waitingMessage = null;

    [Header("Score")]
    [SerializeField] Text player1Text = null;
    [SerializeField] Text player2Text = null;
    int player1Score = 0;
    int player2Score = 0;

    private void Awake()
    {
        onlineManager = this;
    }

    /// <summary>
    /// Function called to start the multiplayer game.
    /// </summary>
    void StartGame()
    {
        photonView.RPC("CleanScore", RpcTarget.All, 1);
        photonView.RPC("CleanScore", RpcTarget.All, 2);

        InstantiatePlayer();

        panelMenu.SetActive(false);
        panelControllers.SetActive(true);
    }

    /// <summary>
    /// Function that instantiates the player in the starting position.
    /// </summary>
    void InstantiatePlayer()
    {
        if (player != null)
        {
            return;
        }

        switch (NetworkManager.networkManager.playerNumber)
        {
            case 1:
                player = PhotonNetwork.Instantiate("2Player1", new Vector2(-5.75f, 0), Quaternion.identity);
                photonView.RPC("CleanScore", RpcTarget.All, 1);
                break;

            case 2:
                player = PhotonNetwork.Instantiate("2Player2", new Vector2(5.75f, 0), Quaternion.identity);
                photonView.RPC("CleanScore", RpcTarget.All, 2);
                break;
        }

        playerClass = player.GetComponent<PaddleNetwork>();
    }

    /// <summary>
    /// Function that increases the score.
    /// </summary>
    /// <param name="playerNumber">Player who has scored.</param>
    public void UpdateScore(int playerNumber)
    {
        photonView.RPC("UpdateScoreServer", RpcTarget.All, playerNumber);
        
        photonView.RPC("ResetPosition", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Function that increases the score on the server.
    /// </summary>
    /// <param name="playerNumber">Player who has scored.</param>
    [PunRPC]
    void UpdateScoreServer(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                player1Score += 1;
                player1Text.text = player1Score.ToString();
                break;
            case 2:
                player2Score += 1;
                player2Text.text = player2Score.ToString();
                break;
        }
    }

    /// <summary>
    /// Function that resets a player's score to zero on the server.
    /// </summary>
    /// <param name="playerNumber">Player 1 or player2.</param>
    [PunRPC]
    void CleanScore(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                player1Score = 0;
                player1Text.text = player1Score.ToString();
                break;
            case 2:
                player2Score = 0;
                player2Text.text = player2Score.ToString();
                break;
        }
    }

    /// <summary>
    /// Function that activates the ball on the server.
    /// </summary>
    [PunRPC]
    void ActivateBall()
    {
        ball.SetActive(true);
    }

    /// <summary>
    /// Function that resets the positions of the paddles and the ball on the server.
    /// </summary>
    [PunRPC]
    void ResetPosition()
    {
        playerClass.ResetPosition();

        ballClass.ResetPosition();
    }

    /// <summary>
    /// Function that pauses and resumes the game.
    /// </summary>
    public void PauseGame()
    {
        if (!panelPause.activeSelf)
        {
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
        }

        else if (panelPause.activeSelf)
        {
            panelPause.SetActive(false);
            panelControllers.SetActive(true);
        }
    }

    /// <summary>
    /// Function that is called when we enter a room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        NetworkManager.networkManager.SetValues();

        if (NetworkManager.networkManager.players.Length == 2)
        {
            waitingMessage.SetActive(false);
        }

        StartGame();
    }

    /// <summary>
    /// Function called when another player joins the room.
    /// </summary>
    /// <param name="newPlayer">The player who enters the room.</param>
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        NetworkManager.networkManager.SetValues();

        if (NetworkManager.networkManager.players.Length == 2)
        {
            waitingMessage.SetActive(false);

            photonView.RPC("ActivateBall", RpcTarget.AllViaServer);
        }
    }

    /// <summary>
    /// Function called when a player leaves the room.
    /// </summary>
    /// <param name="otherPlayer">The player who left the room.</param>
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (otherPlayer.NickName == "1")
        {
            PhotonNetwork.LeaveRoom();

            panelLostPlayer1.SetActive(true);
            panelControllers.SetActive(false);
        }

        else
        {
            NetworkManager.networkManager.SetValues();

            waitingMessage.SetActive(true);
        }

        ball.SetActive(false);
    }

    /// <summary>
    /// Function that is called when we disconnect from the server.
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        GameManager2.manager.LoadScene(2);
    }
}
