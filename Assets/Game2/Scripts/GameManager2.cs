using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to control the main functions of the game 2.
/// </summary>
public class GameManager2 : MonoBehaviour
{
    public static GameManager2 manager;

    [Header("Ball")]
    [SerializeField] GameObject ball = null;
    [SerializeField] Ball ballScript = null;

    [Header("Player 1")]
    [SerializeField] GameObject player1 = null;
    [SerializeField] Paddle paddle1 = null;

    [Header("Player 2")]
    [SerializeField] GameObject playerAI = null;
    [SerializeField] ComputerAI paddleAI = null;

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
    [SerializeField] GameObject waitingMessage = null;

    private void Awake()
    {
        manager = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelControllers.SetActive(true);
        waitingMessage.SetActive(false);

        ball.SetActive(true);
        player1.SetActive(true);
        playerAI.SetActive(true);
    }

    /// <summary>
    /// Function called to reset the position of the paddles and the ball.
    /// </summary>
    private void ResetPosition()
    {
        paddle1.ResetPosition();

        paddleAI.ResetPosition();

        ballScript.ResetPosition();
    }

    /// <summary>
    /// Function called to increase the score.
    /// </summary>
    /// <param name="playerNumber">Player who has scored.</param>
    public void UpdateScore(int playerNumber)
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

        ResetPosition();
    }

    /// <summary>
    /// Function called to load the High Score from the PlayerPrefs.
    /// </summary>
    public void LoadHighScore()
    {
        highScore1 = PlayerPrefs.GetInt("HighScore2-1", 0);
        highScore2 = PlayerPrefs.GetInt("HighScore2-2", 0);

        highScoreText.text = "HIGH SCORE: " + highScore1.ToString() + " - " + highScore2.ToString();
    }

    /// <summary>
    /// Function called to save the High Score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if ((player1Score - player2Score) > (highScore1 - highScore2))
        {
            PlayerPrefs.SetInt("HighScore2-1", player1Score);
            PlayerPrefs.SetInt("HighScore2-2", player2Score);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Function called to pause and resume the game.
    /// </summary>
    public void PauseGame()
    {
        if (!panelPause.activeSelf)
        {
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
            Time.timeScale = 0;
        }
        else if (panelPause.activeSelf)
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
}
