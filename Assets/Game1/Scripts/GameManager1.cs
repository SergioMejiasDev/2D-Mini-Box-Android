﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class to control the main functions of the game 1.
/// </summary>
public class GameManager1 : MonoBehaviour
{
    #region Variables
    public static GameManager1 manager;
    
    [Header("Score")]
    int score;
    [SerializeField] Text scoreText = null;
    [SerializeField] Text score2Text = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject waitingText = null;

    [Header("Players")]
    [SerializeField] GameObject player1 = null;

    [Header("Spawns")]
    [SerializeField] GameObject[] generators = null;

    [Header("Sounds")]
    [SerializeField] AudioSource coindSound = null;
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
    public void StartGame()
    {
        score2Text.enabled = false;
        waitingText.SetActive(false);

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

        player1.SetActive(true);

        for (int i = 0; i < generators.Length; i++)
        {
            generators[i].SetActive(true);
        }

        score = 0;
        scoreText.text = "SCORE: 0";
    }

    /// <summary>
    /// Function that activates the Game Over.
    /// </summary>
    public void GameOver()
    {
        panelControllers.SetActive(false);
        panelGameOver.SetActive(true);

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
    public void UpdateScore()
    {
        score += 1;
        scoreText.text = "SCORE: " + score.ToString();

        coindSound.Play();
    }

    /// <summary>
    /// Function called to load the High Score from the PlayerPrefs.
    /// </summary>
    public void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore1", 0);

        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the High Score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if ((score) > (highScore))
        {
            PlayerPrefs.SetInt("HighScore1", score);
            PlayerPrefs.Save();
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
}
