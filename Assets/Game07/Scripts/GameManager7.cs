using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that controls the main functions of Game 7.
/// </summary>
public class GameManager7 : MonoBehaviour
{
    #region Variables
    public static GameManager7 manager7;

    public delegate void Manager7Delegate();
    public static event Manager7Delegate Stop;
    public static event Manager7Delegate Reset;

    [Header("Player")]
    [SerializeField] MarioMovement player = null;
    int lifes;
    [SerializeField] Image[] lifesImages = null;

    [Header("Princess")]
    [SerializeField] PrincessAnimation princess = null;
    [SerializeField] GameObject help = null;
    [SerializeField] GameObject heart = null;

    [Header("Kong")]
    [SerializeField] KongMovement kong = null;

    [Header("Mallets")]
    [SerializeField] GameObject[] mallets = null;

    [Header("Flames")]
    [SerializeField] GameObject flame = null;
    [SerializeField] Transform flameSpawnPoint = null;

    [Header("Score")]
    int score;
    [SerializeField] Text scoreText = null;
    [SerializeField] GameObject number10 = null;
    [SerializeField] GameObject number30 = null;
    [SerializeField] GameObject number50 = null;
    int highScore;
    [SerializeField] Text highScoreText = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelBlack = null;
    [SerializeField] GameObject panelControllers = null;

    [Header("Sounds")]
    [SerializeField] AudioSource scoreSound = null;
    [SerializeField] AudioSource winSound = null;
    #endregion

    private void Awake()
    {
        manager7 = this;

        LetterBoxer.AddLetterBoxingCamera();
    }

    void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    /// <param name="newGame">True if it is a new game, false if it is not.</param>
    public void StartGame(bool newGame)
    {
        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);

        if (newGame)
        {
            score = 0;

            lifes = 3;
            
            for (int i = 0; i < lifesImages.Length; i++)
            {
                lifesImages[i].enabled = true;
            }

            scoreText.text = "Score: 0";
        }

        player.enabled = true;

        heart.SetActive(false);
        princess.enabled = true;

        kong.enabled = true;

        for (int i = 0; i < mallets.Length; i++)
        {
            mallets[i].SetActive(true);
        }

        Reset();
    }

    /// <summary>
    /// Function that starts the coroutine of reaching the princess.
    /// </summary>
    public void WinGame()
    {
        StartCoroutine(WinningGame());
    }

    /// <summary>
    /// Function that starts the coroutine of when a barrel or flame hits us.
    /// </summary>
    public void GameOver()
    {
        StartCoroutine(LosingGame());
    }

    /// <summary>
    /// Function that stops all animations through the delegate.
    /// </summary>
    public void StopGame()
    {
        Stop();
    }

    /// <summary>
    /// Function that removes all barrels and flames from the scene.
    /// </summary>
    public void CleanScene()
    {
        GameObject[] activeBarrels = GameObject.FindGameObjectsWithTag("Game7/Barrel");

        if (activeBarrels != null)
        {
            for (int i = 0; i < activeBarrels.Length; i++)
            {
                activeBarrels[i].SetActive(false);
            }
        }

        GameObject[] activeFlames = GameObject.FindGameObjectsWithTag("Game7/Flame");

        if (activeFlames != null)
        {
            for (int i = 0; i < activeFlames.Length; i++)
            {
                Destroy(activeFlames[i]);
            }
        }
    }

    /// <summary>
    /// Function called when the player jumps over a barrel.
    /// </summary>
    /// <param name="barrelPosition">The position of the barrel.</param>
    public void JumpBarrel(Vector2 barrelPosition)
    {
        UpdateScore(10);

        scoreSound.Play();

        Destroy(Instantiate(number10, barrelPosition, Quaternion.identity), 1);
    }

    /// <summary>
    /// Function called when the player breaks a barrel.
    /// </summary>
    /// <param name="barrelPosition">The position of the barrel.</param>
    public void DestroyBarrel(Vector2 barrelPosition)
    {
        UpdateScore(30);

        scoreSound.Play();

        Destroy(Instantiate(number30, barrelPosition, Quaternion.identity), 1);
    }

    /// <summary>
    /// Function called when the player destroys a flame.
    /// </summary>
    /// <param name="flamePosition">The position of the flame.</param>
    public void DestroyFlame(Vector2 flamePosition)
    {
        UpdateScore(50);

        scoreSound.Play();

        Destroy(Instantiate(number50, flamePosition, Quaternion.identity), 1);
    }

    /// <summary>
    /// Function called to instantiate a flame in the drum.
    /// </summary>
    /// <param name="isVertical">True if the barrel hitting the drum has been thrown down the Kong.</param>
    public void SpawnFlame(bool isVertical)
    {
        if (isVertical || Random.value > 0.5f)
        {
            Instantiate(flame, flameSpawnPoint.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Function called when the score increases.
    /// </summary>
    /// <param name="increase">How much the score increases.</param>
    void UpdateScore(int increase)
    {
        score += increase;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Function called to load the high score.
    /// </summary>
    void LoadHighScore()
    {
        highScore = SaveManager.saveManager.score7;
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            SaveManager.saveManager.score7 = score;
            SaveManager.saveManager.SaveScores();
        }
    }

    /// <summary>
    /// Function that opens and closes the pause menu.
    /// </summary>
    public void PauseGame()
    {
        if (!panelPause.activeSelf)
        {
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
            Time.timeScale = 0;
        }

        else
        {
            panelPause.SetActive(false);
            panelControllers.SetActive(true);
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Function that opens and closes the help menu.
    /// </summary>
    public void OpenHelp()
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
    /// Function that loads a new scene.
    /// </summary>
    /// <param name="buildIndex">The scene to be loaded.</param>
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Coroutine started when the player reaches the princess, and takes care of the animations and resetting the scene.
    /// </summary>
    /// <returns></returns>
    IEnumerator WinningGame()
    {
        StopGame();

        player.enabled = false;
        princess.enabled = false;
        kong.enabled = false;

        help.SetActive(false);
        heart.SetActive(true);

        CleanScene();

        yield return new WaitForSeconds(0.25f);

        winSound.Play();

        yield return new WaitForSeconds(3);

        UpdateScore(100);
        panelBlack.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        panelBlack.SetActive(false);
        StartGame(false);
        Reset();
    }

    /// <summary>
    /// Coroutine started when the player dies, and resets the scene or opens the Game Over panel.
    /// </summary>
    /// <returns></returns>
    IEnumerator LosingGame()
    {
        if (lifes > 0)
        {
            lifes -= 1;

            lifesImages[lifes].enabled = false;

            panelBlack.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            panelBlack.SetActive(false);
            StartGame(false);
            Reset();
        }

        else
        {
            SaveHighScore();

            panelGameOver.SetActive(true);
            panelControllers.SetActive(false);
        }
    }
}
