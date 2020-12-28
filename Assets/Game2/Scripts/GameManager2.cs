using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Script to control the main functions of the game 2.
/// </summary>
public class GameManager2 : MonoBehaviour
{
    public static GameManager2 manager;

    [Header("Ball")]
    [SerializeField] GameObject ball = null;
    Ball ballScript;

    [Header("Player 1")]
    [SerializeField] GameObject player1Paddle = null;
    Paddle paddle1;

    [Header("Player 2")]
    [SerializeField] GameObject player2Paddle = null;
    Paddle paddle2;
    ComputerAI paddleAI;

    [Header("Score")]
    [SerializeField] Text player1Text = null;
    [SerializeField] Text player2Text = null;
    int player1Score = 0;
    int player2Score = 0;
    [SerializeField] Text highScoreText = null;
    int highScore1;
    int highScore2;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;

    private void Awake()
    {
        manager = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        ballScript = ball.GetComponent<Ball>();
        paddle1 = player1Paddle.GetComponent<Paddle>();
        paddle2 = player2Paddle.GetComponent<Paddle>();
        paddleAI = player2Paddle.GetComponent<ComputerAI>();
        LoadHighScore();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    /// <param name="numberOfPlayers">Number of players who will play.</param>
    public void StartGame(int numberOfPlayers)
    {
        panelMenu.SetActive(false);
        ball.SetActive(true);
        paddle1.enabled = true;
        
        if (numberOfPlayers == 1)
        {
            paddleAI.enabled = true;
        }
        else if (numberOfPlayers == 2)
        {
            paddle2.enabled = true;
        }
    }

    /// <summary>
    /// Function called to reset the position of the paddles and the ball.
    /// </summary>
    private void ResetPosition()
    {
        paddle1.DontAllowMovement();
        paddle1.ResetPosition();
        if (paddle2.enabled == true)
        {
            paddle2.ResetPosition();
        }
        else if (paddleAI.enabled == true)
        {
            paddleAI.ResetPosition();
        }

        ballScript.ResetPosition();
    }

    /// <summary>
    /// Function called to increase the score.
    /// </summary>
    /// <param name="playerNumber">Player who has scored.</param>
    public void UpdateScore(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1Score++;
            player1Text.text = player1Score.ToString();
        }
        
        else if (playerNumber == 2)
        {
            player2Score++;
            player2Text.text = player2Score.ToString();
        }
        
        ResetPosition();
    }

    /// <summary>
    /// Function called to load the High Score from the PlayerPrefs.
    /// </summary>
    public void LoadHighScore()
    {
        if (PlayerPrefs.HasKey("HighScore2-1"))
        {
            highScore1 = PlayerPrefs.GetInt("HighScore2-1");
            highScore2 = PlayerPrefs.GetInt("HighScore2-2");
        }

        highScoreText.text = "HIGH SCORE: " + highScore1.ToString() + " - " + highScore2.ToString();
    }

    /// <summary>
    /// Function called to save the High Score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (paddleAI.enabled == true)
        {
            if ((player1Score - player2Score) > (highScore1 - highScore2))
            {
                PlayerPrefs.SetInt("HighScore2-1", player1Score);
                PlayerPrefs.SetInt("HighScore2-2", player2Score);
                PlayerPrefs.Save();
            }
        }
    }

    /// <summary>
    /// Function called to pause and resume the game.
    /// </summary>
    public void PauseGame()
    {
        if (panelPause.activeSelf == false)
        {
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
            paddle1.DontAllowMovement();
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
    /// Function to open and close the help panel.
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
    /// Scene change function.
    /// </summary>
    /// <param name="buildIndex">Number of the scene to be loaded.</param>
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
