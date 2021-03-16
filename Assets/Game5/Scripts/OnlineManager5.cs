using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/// <summary>
/// Class that manages the online functions of Game 05.
/// </summary>
public class OnlineManager5 : MonoBehaviourPunCallbacks
{
    public static OnlineManager5 onlineManager;

    [Header("Player")]
    GameObject snake;
    SnakeMovementNetwork snakeClass;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
    int score2 = 0;
    [SerializeField] Text score2Text = null;

    [Header("Food")]
    [SerializeField] LayerMask snakeMask = 0;
    int spawnAttemps = 0;

    [Header("Borders")]
    [SerializeField] Transform borderTop = null;
    [SerializeField] Transform borderBottom = null;
    [SerializeField] Transform borderLeft = null;
    [SerializeField] Transform borderRight = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelLostPlayer1 = null;
    [SerializeField] GameObject waitingMessage = null;
    [SerializeField] GameObject panelVictory = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] Text victoryText1 = null;
    [SerializeField] Text victoryText2 = null;

    [Header("Sounds")]
    [SerializeField] AudioSource newRedFoodSound = null;
    [SerializeField] AudioSource foodSound = null;
    [SerializeField] AudioSource redFoodSound = null;
    [SerializeField] AudioSource hurtSound = null;

    /// <summary>
    /// Function that searches for a random position on the plane and checks if it is available.
    /// </summary>
    /// <returns>The vector where the objects will be instantiated.</returns>
    Vector2 SpawnVector()
    {
        int x = (int)Random.Range(borderLeft.position.x + 1, borderRight.position.x - 1);

        int y = (int)Random.Range(borderBottom.position.y + 1, borderTop.position.y - 1);

        if (Physics2D.OverlapCircle(new Vector2(x, y), 0.5f, snakeMask))
        {
            spawnAttemps += 1;

            if (spawnAttemps > 20)
            {
                spawnAttemps = 0;

                return new Vector2(x, y);
            }

            return SpawnVector();
        }

        else
        {
            spawnAttemps = 0;

            return new Vector2(x, y);
        }
    }

    private void Awake()
    {
        onlineManager = this;
    }

    /// <summary>
    /// Function that starts the multiplayer game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
    }

    /// <summary>
    /// Function that restarts the game after Game Over.
    /// </summary>
    public void Restart()
    {
        photonView.RPC("RestartNetwork", RpcTarget.AllViaServer);
        photonView.RPC("ResetScoreNetwork", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Function that restarts the game on the server after Game Over
    /// </summary>
    [PunRPC]
    void RestartNetwork()
    {
        CleanTails();

        panelControllers.SetActive(true);
        panelVictory.SetActive(false);
        victoryText1.enabled = false;
        victoryText2.enabled = false;

        InstantiatePlayer();
    }

    /// <summary>
    /// Function that instantiates the player in the scene.
    /// </summary>
    void InstantiatePlayer()
    {
        if (snake != null)
        {
            return;
        }

        switch (NetworkManager.networkManager.playerNumber)
        {
            case 1:
                snake = PhotonNetwork.Instantiate("5Head1", new Vector2(0, 10), Quaternion.identity);
                break;
            case 2:
                snake = PhotonNetwork.Instantiate("5Head2", new Vector2(0, -10), Quaternion.identity);
                break;
        }

        snakeClass = snake.GetComponent<SnakeMovementNetwork>();
        
        snakeClass.ResetPosition();
    }

    /// <summary>
    /// Function that reproduces sounds.
    /// </summary>
    /// <param name="sound">The sound we want to reproduce.</param>
    public void PlaySound(string sound)
    {
        photonView.RPC("PlaySoundNetwork", RpcTarget.All, sound);
    }

    /// <summary>
    /// Function that reproduces sounds on the server.
    /// </summary>
    /// <param name="sound">The sound we want to reproduce.</param>
    [PunRPC]
    void PlaySoundNetwork(string sound)
    {
        switch (sound)
        {
            case "NewRedFood":
                newRedFoodSound.Play();
                break;
            case "Food":
                foodSound.Play();
                break;
            case "RedFood":
                redFoodSound.Play();
                break;
            case "Hurt":
                hurtSound.Play();
                break;
        }
    }

    /// <summary>
    /// Function called to spawn a normal point.
    /// </summary>
    public void Spawn()
    {
        photonView.RPC("SpawnFoodNetwork", RpcTarget.MasterClient);
    }

    /// <summary>
    /// Function called to spawn a normal point on the server.
    /// </summary>
    [PunRPC]
    void SpawnFoodNetwork()
    {
        PhotonNetwork.Instantiate("5Food", SpawnVector(), Quaternion.identity);
    }

    /// <summary>
    /// Function called to spawn a red point.
    /// </summary>
    public void SpawnRed()
    {
        StartCoroutine(SpawnRedFood());
    }

    /// <summary>
    /// Coroutine called to spawn a red point after a while.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnRedFood()
    {
        yield return new WaitForSeconds(Random.Range(20, 40));

        photonView.RPC("SpawnRedNetwork", RpcTarget.MasterClient);
    }

    /// <summary>
    /// Function called to spawn a red point on the server.
    /// </summary>
    [PunRPC]
    void SpawnRedNetwork()
    {
        GameObject activeRedFood = PhotonNetwork.Instantiate("5RedFood", SpawnVector(), Quaternion.identity);

        newRedFoodSound.Play();

        PlaySound("NewRedFood");
    }

    /// <summary>
    /// Function that stop all coroutines.
    /// </summary>
    void StopCoroutines()
    {
        photonView.RPC("StopCoroutinesNetwork", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Function that stop all coroutines on the server.
    /// </summary>
    [PunRPC]
    void StopCoroutinesNetwork()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Function called to increase the score.
    /// </summary>
    /// <param name="scoreValue">How much the score increases.</param>
    /// <param name="playerNumber">Player 1 or player 2.</param>
    public void UpdateScore(int scoreValue, int playerNumber)
    {
        photonView.RPC("UpdateScoreNetwork", RpcTarget.All, scoreValue, playerNumber);
    }

    /// <summary>
    /// Function called to increase the score on the server.
    /// </summary>
    /// <param name="scoreValue">How much the score increases.</param>
    /// <param name="playerNumber">Player 1 or player 2.</param>
    [PunRPC]
    void UpdateScoreNetwork(int scoreValue, int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                score += scoreValue;
                scoreText.text = "Score: " + score.ToString();
                break;
            case 2:
                score2 += scoreValue;
                score2Text.text = "Score: " + score2.ToString();
                break;
        }
    }

    /// <summary>
    /// Function that resets the scores to zero.
    /// </summary>
    [PunRPC]
    void ResetScoreNetwork()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();

        score2 = 0;
        score2Text.text = "Score: " + score2.ToString();
    }

    /// <summary>
    /// Function that cleans objects from the scene.
    /// </summary>
    void CleanScene()
    {
        photonView.RPC("CleanSceneNetwork", RpcTarget.MasterClient);
    }

    /// <summary>
    /// Function that cleans objects from the scene on the server.
    /// </summary>
    [PunRPC]
    void CleanSceneNetwork()
    {
        GameObject[] activeFood = GameObject.FindGameObjectsWithTag("Game5/Food");

        if (activeFood != null)
        {
            for (int i = 0; i < activeFood.Length; i++)
            {
                DestroyFood(activeFood[i].GetComponent<PhotonView>().ViewID);
            }
        }

        GameObject[] activeRedFood = GameObject.FindGameObjectsWithTag("Game5/RedFood");

        if (activeRedFood != null)
        {
            for (int i = 0; i < activeRedFood.Length; i++)
            {
                DestroyFood(activeRedFood[i].GetComponent<PhotonView>().ViewID);
            }
        }

        StopCoroutines();
    }

    /// <summary>
    /// Feature that removes all queue fragments from the screen after a player disconnects.
    /// </summary>
    void CleanTails()
    {
        GameObject[] tails = GameObject.FindGameObjectsWithTag("Game5/Tail");

        if (tails != null)
        {
            for (int i = 0; i < tails.Length; i++)
            {
                Destroy(tails[i]);
            }
        }
    }

    /// <summary>
    /// Function that is responsible for destroying the food in the scene.
    /// </summary>
    /// <param name="food">The Photon View ID of the food we want to destroy.</param>
    public void DestroyFood(int food)
    {
        photonView.RPC("DestroyFoodNetwork", RpcTarget.MasterClient, food);
    }

    /// <summary>
    /// Function that is responsible for destroying the food on the server.
    /// </summary>
    /// <param name="food">The Photon View ID of the food we want to destroy.</param>
    [PunRPC]
    void DestroyFoodNetwork(int food)
    {
        if (PhotonView.Find(food) != null)
        {
            PhotonNetwork.Destroy(PhotonView.Find(food));
        }
    }

    /// <summary>
    /// Function called when a snake collides with the wall or bites its tail.
    /// </summary>
    /// <param name="playerNumber">Player 1 or Player 2.</param>
    public void GameOver(int playerNumber)
    {
        photonView.RPC("GameOverNetwork", RpcTarget.AllViaServer, playerNumber);
        PlaySound("Hurt");
    }

    /// <summary>
    /// Function called on the server when a snake collides with the wall or bites its tail.
    /// </summary>
    /// <param name="playerNumber">Player 1 or Player 2.</param>
    [PunRPC]
    void GameOverNetwork(int playerNumber)
    {
        panelVictory.SetActive(true);
        panelControllers.SetActive(false);

        snakeClass.CleanTailNetwork();

        if (PhotonView.Find(snake.GetComponent<PhotonView>().ViewID) != null)
        {
            PhotonNetwork.Destroy(PhotonView.Find(snake.GetComponent<PhotonView>().ViewID));
        }

        switch (playerNumber)
        {
            case 1:
                victoryText2.enabled = true;
                break;

            case 2:
                victoryText1.enabled = true;
                break;
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
    /// Function that is called when we enter a room.
    /// </summary>
    public override void OnJoinedRoom()
    {
        StartGame();

        NetworkManager.networkManager.SetValues();

        if (NetworkManager.networkManager.players.Length == 2)
        {
            waitingMessage.SetActive(false);
            panelControllers.SetActive(true);

            InstantiatePlayer();

            photonView.RPC("ResetScoreNetwork", RpcTarget.All);
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
            panelControllers.SetActive(true);

            InstantiatePlayer();

            Spawn();
            SpawnRed();
        }
    }

    /// <summary>
    /// Function called when a player leaves the room.
    /// </summary>
    /// <param name="otherPlayer">The player who left the room.</param>
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        panelVictory.SetActive(false);
        victoryText1.enabled = false;
        victoryText2.enabled = false;

        if (otherPlayer.NickName == "1")
        {
            panelLostPlayer1.SetActive(true);

            PhotonNetwork.LeaveRoom();
        }

        else
        {
            NetworkManager.networkManager.SetValues();

            waitingMessage.SetActive(true);
            panelControllers.SetActive(false);

            CleanTails();

            CleanScene();
        }
    }

    /// <summary>
    /// Function that is called when we disconnect from the server.
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        GameManager5.manager5.LoadScene(5);
    }
}
