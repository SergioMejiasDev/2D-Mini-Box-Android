using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script to control the main functions of the game 1.
/// </summary>
public class GameManager1 : MonoBehaviour
{
    #region Variables
    public static GameManager1 manager;
    
    [Header("Score")]
    int score;
    [SerializeField] Text scoreText = null;
    int score2;
    [SerializeField] Text score2Text = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;

    [Header("Players")]
    [SerializeField] GameObject player1 = null;
    [SerializeField] GameObject player2 = null;
    public bool isMultiplayer;

    [Header("Spawns")]
    [SerializeField] GameObject[] generators = null;
    #endregion

    void Awake()
    {
        manager = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
    }

    /// <summary>
    /// Function that starts a new game.
    /// </summary>
    /// <param name="multiplayer">True if is a multiplayer game.</param>
    public void StartGame(bool multiplayer)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Game1/Enemy");
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SetActive(false);
            }
        }

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Game1/Coin");
        if (coins != null)
        {
            for (int i = 0; i < coins.Length; i++)
            {
                coins[i].SetActive(false);
            }
        }

        GameObject[] missiles = GameObject.FindGameObjectsWithTag("Game1/Missile");
        if (missiles != null)
        {
            for (int i = 0; i < missiles.Length; i++)
            {
                missiles[i].SetActive(false);
            }
        }

        panelMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);

        player1.GetComponent<Player>().DontAllowMovement();
        player1.SetActive(true);

        if (multiplayer)
        {
            player2.SetActive(true);
            score2Text.enabled = true;
            isMultiplayer = true;
            score2 = 0;
            score2Text.text = "SCORE: 0";
        }
        else
        {
            isMultiplayer = false;
        }

        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].SetActive(true);
        }

        score = 0;
        scoreText.text = "SCORE: 0";
    }

    /// <summary>
    /// Function that starts a new game after Game Over.
    /// </summary>
    public void Restart()
    {
        StartGame(isMultiplayer);
    }

    /// <summary>
    /// Function that returns the player to the original position after dying.
    /// </summary>
    /// <param name="isPlayer1">True if the dead player is player 1.</param>
    public void Respawn(bool isPlayer1)
    {
        if (isPlayer1)
        {
            player1.SetActive(false);
            score = 0;
            scoreText.text = "SCORE: 0";
        }
        else
        {
            player2.SetActive(false);
            score2 = 0;
            score2Text.text = "SCORE: 0";
        }
        
        StartCoroutine(WaitForRespawn(isPlayer1));
    }

    /// <summary>
    /// Function that activates the Game Over.
    /// </summary>
    public void GameOver()
    {
        panelGameOver.SetActive(true);
        panelControllers.SetActive(false);
        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].SetActive(false);
        }
        SaveHighScore();
    }

    /// <summary>
    /// Function that pauses and resumes the game.
    /// </summary>
    public void PauseGame()
    {
        if (panelPause.activeSelf == false)
        {
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
            player1.GetComponent<Player>().DontAllowMovement();
            Time.timeScale = 0;
        }
        else if (panelPause.activeSelf == true)
        {
            panelPause.SetActive(false);
            panelControllers.SetActive(true);
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Function that updates the score.
    /// </summary>
    /// <param name="isPlayer1">True if the scored player is player 1.</param>
    public void UpdateScore(bool isPlayer1)
    {
        if (isPlayer1)
        {
            score += 1;
            scoreText.text = "SCORE: " + score.ToString();
        }
        else
        {
            score2 += 1;
            score2Text.text = "SCORE: " + score2.ToString();
        }
    }

    /// <summary>
    /// Function called to load the High Score from the PlayerPrefs.
    /// </summary>
    public void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore1"))
        {
            highScore = PlayerPrefs.GetInt("HighScore1");
        }

        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the High Score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (!isMultiplayer)
        {
            if ((score) > (highScore))
            {
                PlayerPrefs.SetInt("HighScore1", score);
                PlayerPrefs.Save();
            }
        }
    }

    /// <summary>
    /// Function that activates and deactivates the help panel.
    /// </summary>
    public void Help()
    {
        if (panelHelp.activeSelf == false)
        {
            panelHelp.SetActive(true);
        }
        else
        {
            panelHelp.SetActive(false);
        }
    }

    /// <summary>
    /// Function that is called to change the scene.
    /// </summary>
    /// <param name="buildIndex">Scene to load.</param>
    public void LoadGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Coroutine that takes care of the player's respawn after dying.
    /// </summary>
    /// <param name="isPlayer1">True if the dead player is player 1.</param>
    /// <returns></returns>
    IEnumerator WaitForRespawn(bool isPlayer1)
    {
        yield return new WaitForSeconds(2);
        {
            if (isPlayer1)
            {
                player1.transform.position = new Vector2(-8.76f, -5.4f);
                player1.SetActive(true);
            }
            else
            {
                player2.transform.position = new Vector2(7.5f, -5.4f);
                player2.SetActive(true);
            }
        }
    }
}
