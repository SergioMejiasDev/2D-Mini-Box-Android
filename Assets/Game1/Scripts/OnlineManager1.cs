using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Class that manages the online functions of Game 01.
/// </summary>
public class OnlineManager1 : MonoBehaviourPunCallbacks
{
    public static OnlineManager1 onlineManager;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelLostPlayer1 = null;

    [Header("Generators")]
    [SerializeField] GameObject[] generators = null;
    
    [Header("Score")]
    int score1 = 0;
    [SerializeField] Text score1Text = null;
    int score2 = 0;
    [SerializeField] Text score2Text = null;

    [Header("Player")]
    GameObject player;
    Photon.Realtime.Player[] players;
    public int playerNumber = 0;

    [Header("Sounds")]
    [SerializeField] AudioSource hurtSound = null;
    [SerializeField] AudioSource coinSound = null;

    private void Awake()
    {
        onlineManager = this;
    }

    /// <summary>
    /// Function that starts the game online.
    /// </summary>
    public void StartGame()
    {
        photonView.RPC("EnableGenerators", RpcTarget.MasterClient);

        photonView.RPC("CleanScore", RpcTarget.All, true);
        photonView.RPC("CleanScore", RpcTarget.All, false);

        InstantiatePlayer();

        panelMenu.SetActive(false);
        panelControllers.SetActive(true);
    }

    /// <summary>
    /// Function that returns the player to the original position after dying.
    /// </summary>
    public void Respawn()
    {
        player = null;

        photonView.RPC("PlaySound", RpcTarget.All, "hurt");

        StartCoroutine(WaitForRespawn());
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

        switch (playerNumber)
        {
            case 1:
                player = PhotonNetwork.Instantiate("1Player1Server", new Vector2(-6.3f, -5.4f), Quaternion.identity);
                photonView.RPC("CleanScore", RpcTarget.All, true);
                break;

            case 2:
                player = PhotonNetwork.Instantiate("1Player2Server", new Vector2(6.3f, -5.4f), Quaternion.identity);
                photonView.RPC("CleanScore", RpcTarget.All, false);
                break;
        }
    }

    /// <summary>
    /// Function that activates the generators (enemies and coins) on the server.
    /// </summary>
    [PunRPC] void EnableGenerators()
    {
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].SetActive(true);
        }
    }

    /// <summary>
    /// Function that plays the sounds on the server.
    /// </summary>
    /// <param name="soundToPlay">The sound we want to play.</param>
    [PunRPC] void PlaySound(string soundToPlay)
    {
        switch (soundToPlay)
        {
            case "coin":
                coinSound.Play();
                break;
            case "hurt":
                hurtSound.Play();
                break;
        }
    }

    /// <summary>
    /// Function we call to destroy a coin.
    /// </summary>
    /// <param name="coin">The Photon View ID of the coin that we are going to destroy.</param>
    public void DestroyCoin(int coin)
    {
        photonView.RPC("DestroyCoinServer", RpcTarget.MasterClient, coin);
    }

    /// <summary>
    /// Function that destroy a coin on the server.
    /// </summary>
    /// <param name="coin">The Photon View ID of the coin that we are going to destroy.</param>
    [PunRPC] void DestroyCoinServer(int coin)
    {
        if (PhotonView.Find(coin) != null)
        {
            PhotonNetwork.Destroy(PhotonView.Find(coin));
        }
    }

    /// <summary>
    /// Function that increases the score.
    /// </summary>
    /// <param name="isPlayer1">True if player 1 scores.</param>
    public void Scored(bool isPlayer1)
    {
        photonView.RPC("UpdateScore", RpcTarget.All, isPlayer1);
    }

    /// <summary>
    /// Function that increases the score on the server.
    /// </summary>
    /// <param name="isPlayer1">True if player 1 scores.</param>
    [PunRPC] void UpdateScore(bool isPlayer1)
    {
        photonView.RPC("PlaySound", RpcTarget.All, "coin");

        if (isPlayer1)
        {
            score1 += 1;
            score1Text.text = "Score: " + score1;
        }

        else 
        {
            score2 += 1;
            score2Text.text = "Score: " + score2;
        }
    }

    /// <summary>
    /// Function that resets a player's score to zero on the server.
    /// </summary>
    /// <param name="isPlayer1">True if is for player 1.</param>
    [PunRPC] void CleanScore(bool isPlayer1)
    {
        if (isPlayer1)
        {
            score1 = 0;
            score1Text.text = "Score: " + score1;
        }

        else
        {
            score2 = 0;
            score2Text.text = "Score: " + score2;
        }
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
    /// Coroutine that makes a wait before instantiating the player again.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(2);
        {
            InstantiatePlayer();
        }
    }

    /// <summary>
    /// Function that is called when we enter a room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        players = PhotonNetwork.PlayerList;

        playerNumber = players.Length;

        PhotonNetwork.NickName = playerNumber.ToString();

        StartGame();
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
            photonView.RPC("CleanScore", RpcTarget.All, false);
        }
    }

    /// <summary>
    /// Function that is called when we disconnect from the server.
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        GameManager1.manager.LoadGame(1);
    }
}
