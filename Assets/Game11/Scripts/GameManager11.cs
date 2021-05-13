using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the main functions of Game 11.
/// </summary>
public class GameManager11 : MonoBehaviour
{
    public static GameManager11 manager;

    [Header("Player")]
    [SerializeField] GameObject player = null;

    [Header("Timer")]
    [SerializeField] Timer11 timer = null;

    [Header("Score")]
    [SerializeField] Text scoreText = null;
    int highScoreSecs = 0;
    int highScoreMillisecs = 0;
    [SerializeField] Text highScoreText = null;
    [SerializeField] Text highScoreTextMenu = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject waitingMessage = null;
    [SerializeField] GameObject panelControllers = null;

    [Header("Sounds")]
    [SerializeField] AudioSource goalSound = null;

    private void Awake()
    {
        manager = this;

        LetterBoxer.AddLetterBoxingCamera();
    }

    void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
    }

    /// <summary>
    /// Function that starts the game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        waitingMessage.SetActive(false);
        panelControllers.SetActive(true);

        scoreText.enabled = true;
        highScoreText.enabled = true;

        player.SetActive(true);

        timer.enabled = true;
    }

    /// <summary>
    /// Function called to load the high score.
    /// </summary>
    void LoadHighScore()
    {
        highScoreSecs = SaveManager.saveManager.score11[0];
        highScoreMillisecs = SaveManager.saveManager.score11[1];
        highScoreText.text = string.Format("{00}:{01}", highScoreSecs, highScoreMillisecs.ToString("00"));
        highScoreTextMenu.text = string.Format("High Score: {00}:{01}", highScoreSecs, highScoreMillisecs.ToString("00"));
    }

    /// <summary>
    /// Function called to save the high score.
    /// </summary>
    public void SaveHighScore(int secs, int millisecs)
    {
        goalSound.Play();

        if (secs < highScoreSecs ||
            (secs == highScoreSecs && millisecs < highScoreMillisecs) ||
            (highScoreSecs == 0 && highScoreMillisecs == 0))
        {
            SaveManager.saveManager.score11 = new int[2] { secs, millisecs };
            SaveManager.saveManager.SaveScores();

            LoadHighScore();
        }
    }

    /// <summary>
    /// Function that pauses and resumes the game.
    /// </summary>
    public void PauseGame()
    {
        if (!panelPause.activeSelf)
        {
            Time.timeScale = 0;
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
        }

        else if (panelPause.activeSelf)
        {
            Time.timeScale = 1;
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
}
