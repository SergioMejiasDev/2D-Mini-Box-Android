using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the main functions of Game 08.
/// </summary>
public class GameManager8 : MonoBehaviour
{
    #region Variables

    public static GameManager8 manager8;

    [Header("Player")]
    [SerializeField] GameObject player = null;
    [SerializeField] FrogMovement playerClass = null;
    int remainingLifes;
    [SerializeField] Image[] lifes = null;

    [Header("Spawner")]
    [SerializeField] FroggerSpawners spawner = null;

    [Header("Rescued Frogs")]
    int remainingFrogs;
    [SerializeField] GameObject[] rescuedFrogs = null;

    [Header("Score")]
    int score;
    [SerializeField] Text scoreText = null;
    int highScore;
    [SerializeField] Text highScoreText = null;
    [SerializeField] GameObject[] scoreLines = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelBlack = null;

    [Header("Sounds")]
    [SerializeField] AudioSource rescuedFrogSound = null;

    #endregion

    void Awake()
    {
        manager8 = this;

        LetterBoxer.AddLetterBoxingCamera();
    }

    void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
    }

    /// <summary>
    /// Function we call to start the game.
    /// </summary>
    /// <param name="newGame">True if it is a new game.</param>
    public void StartGame(bool newGame)
    {
        if (newGame)
        {
            score = 0;
            scoreText.text = "Score: 0";

            remainingLifes = 3;

            for (int i = 0; i < lifes.Length; i++)
            {
                lifes[i].enabled = true;
            }
        }

        for (int i = 0; i < scoreLines.Length; i++)
        {
            scoreLines[i].SetActive(true);
        }

        remainingFrogs = 5;

        for (int i = 0; i < rescuedFrogs.Length; i++)
        {
            rescuedFrogs[i].GetComponent<SpriteRenderer>().enabled = false;
            rescuedFrogs[i].layer = 0;
        }

        spawner.StartSpawns();

        player.SetActive(true);
        playerClass.ResetPosition();

        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);
    }

    /// <summary>
    /// Function that we call when we reach a base.
    /// </summary>
    /// <param name="frog">The frog we have rescued.</param>
    public void FrogRescued(GameObject frog)
    {
        frog.GetComponent<SpriteRenderer>().enabled = true;
        frog.layer = 10;

        for (int i = 0; i < scoreLines.Length; i++)
        {
            scoreLines[i].SetActive(true);
        }

        remainingFrogs -= 1;
        rescuedFrogSound.Play();

        UpdateScore(50);

        if (remainingFrogs <= 0)
        {
            StartCoroutine(WinGame());
        }
    }

    /// <summary>
    /// Function called when the player dies.
    /// </summary>
    public void IsDie()
    {
        StartCoroutine(IsDying());
    }

    /// <summary>
    /// Function called when the score increases.
    /// </summary>
    /// <param name="increase">How much the score increases.</param>
    public void UpdateScore(int increase)
    {
        score += increase;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Function called to load the high score.
    /// </summary>
    void LoadHighScore()
    {
        highScore = SaveManager.saveManager.score8;
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            SaveManager.saveManager.score8 = score;
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
    /// Coroutine that restarts the scene after dying or activates the Game Over.
    /// </summary>
    /// <returns></returns>
    IEnumerator IsDying()
    {
        if (remainingLifes > 0)
        {
            remainingLifes -= 1;
            lifes[remainingLifes].enabled = false;
        }

        else
        {
            panelGameOver.SetActive(true);
            panelControllers.SetActive(false);
            SaveHighScore();

            yield break;
        }

        panelBlack.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        player.SetActive(true);
        playerClass.ResetPosition();

        panelBlack.SetActive(false);
    }

    /// <summary>
    /// Coroutine that restarts the scene after rescuing all the frogs.
    /// </summary>
    /// <returns></returns>
    IEnumerator WinGame()
    {
        player.SetActive(false);

        yield return new WaitForSeconds(2);

        panelBlack.SetActive(true);
        UpdateScore(100);

        yield return new WaitForSeconds(0.5f);

        StartGame(false);
        panelBlack.SetActive(false);
    }
}
