using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Class that manages the online functions of Game 11.
/// </summary>
public class OnlineManager11 : MonoBehaviourPunCallbacks
{
    public static OnlineManager11 manager;

    [Header("Player")]
    GameObject player;

    [Header("Score")]
    int score1 = 0;
    [SerializeField] Text score1Text = null;
    int score2 = 0;
    [SerializeField] Text score2Text = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelLostPlayer1 = null;
    [SerializeField] GameObject waitingMessage = null;
    [SerializeField] GameObject panelVictory = null;
    [SerializeField] Text victoryText1 = null;
    [SerializeField] Text victoryText2 = null;

    [Header("Sounds")]
    [SerializeField] AudioSource goalSound = null;

    void Awake()
    {
        manager = this;
    }

    /// <summary>
    /// Function that starts the game.
    /// </summary>
    public void StartGame()
    {
        waitingMessage.SetActive(false);

        panelVictory.SetActive(false);
        victoryText1.enabled = false;
        victoryText2.enabled = false;

        photonView.RPC("CleanScore", RpcTarget.All);

        InstantiatePlayer();
    }

    /// <summary>
    /// Function that restarts the game after having a winner.
    /// </summary>
    public void RestartGame()
    {
        photonView.RPC("RestartGameNetwork", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Function that restarts the game on server after having a winner.
    /// </summary>
    [PunRPC]
    void RestartGameNetwork()
    {
        panelVictory.SetActive(false);
        victoryText1.enabled = false;
        victoryText2.enabled = false;

        CleanScore();

        InstantiatePlayer();
    }

    /// <summary>
    /// Function that instantiates the player on stage.
    /// </summary>
    void InstantiatePlayer()
    {
        if (player != null)
        {
            DestroyPlayer();
        }

        switch (NetworkManager.networkManager.playerNumber)
        {
            case 1:
                player = PhotonNetwork.Instantiate("11Player1", new Vector2(-2.85f, -2.6f), new Quaternion(0f, 0f, 0.7f, 0.7f));
                break;

            case 2:
                player = PhotonNetwork.Instantiate("11Player2", new Vector2(-2.85f, -3.27f), new Quaternion(0f, 0f, 0.7f, 0.7f));
                break;
        }
    }

    /// <summary>
    /// Function that destroys the player on stage.
    /// </summary>
    void DestroyPlayer()
    {
        PhotonNetwork.Destroy(player);
    }

    /// <summary>
    /// Function that increases the score.
    /// </summary>
    /// <param name="playerNumber">Player 1 or Player 2.</param>
    public void UpdateScore(int playerNumber)
    {
        photonView.RPC("UpdateScoreNetwork", RpcTarget.AllViaServer, playerNumber);
    }

    /// <summary>
    /// Function that increases the score on server.
    /// </summary>
    /// <param name="playerNumber">Player 1 or Player 2.</param>
    [PunRPC]
    void UpdateScoreNetwork(int playerNumber)
    {
        if (NetworkManager.networkManager.players.Length == 1)
        {
            return;
        }

        goalSound.Play();

        if (playerNumber == 1)
        {
            score1 -= 1;
            score1Text.text = score1.ToString();
        }

        else if (playerNumber == 2)
        {
            score2 -= 1;
            score2Text.text = score2.ToString();
        }

        if (score1 == 0)
        {
            panelVictory.SetActive(true);
            victoryText1.enabled = true;
            DestroyPlayer();
        }

        else if (score2 == 0)
        {
            panelVictory.SetActive(true);
            victoryText2.enabled = true;
            DestroyPlayer();
        }
    }

    /// <summary>
    /// Function that resets a player's score to zero on the server.
    /// </summary>
    [PunRPC]
    void CleanScore()
    {
        score1 = 5;
        score1Text.text = "5";

        score2 = 5;
        score2Text.text = "5";
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
    /// Function to open and close the help panel.
    /// </summary>
    public void Help()
    {
        if (!panelHelp.activeSelf)
        {
            panelHelp.SetActive(true);
        }
        else
        {
            panelHelp.SetActive(false);
        }
    }

    /// <summary>
    /// Scene change function.
    /// </summary>
    /// <param name="buildIndex">Number of the scene to be loaded.</param>
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Function that is called when we enter a room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        panelMenu.SetActive(false);
        panelControllers.SetActive(true);

        NetworkManager.networkManager.SetValues();

        if (NetworkManager.networkManager.players.Length == 1)
        {
            InstantiatePlayer();
        }

        if (NetworkManager.networkManager.players.Length == 2)
        {
            waitingMessage.SetActive(false);
        }
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

            RestartGame();
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
            DestroyPlayer();

            panelVictory.SetActive(false);

            PhotonNetwork.LeaveRoom();

            panelLostPlayer1.SetActive(true);
        }

        else
        {
            NetworkManager.networkManager.SetValues();

            panelVictory.SetActive(false);

            if (player == null)
            {
                InstantiatePlayer();
            }

            waitingMessage.SetActive(true);
        }
    }

    /// <summary>
    /// Function that is called when we disconnect from the server.
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        LoadScene(11);
    }
}
