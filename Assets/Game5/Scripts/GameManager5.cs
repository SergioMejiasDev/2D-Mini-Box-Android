using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that controls the main functions of Game 5.
/// </summary>
public class GameManager5 : MonoBehaviour
{
    #region Variables
    public static GameManager5 manager5;

    [Header("Players")]
    public bool multiplayer = false;
    [SerializeField] GameObject head = null;
    SnakeMovement snakeMovement;
    [SerializeField] GameObject head2 = null;
    SnakeMovement snake2Movement;

    [Header("Food")]
    [SerializeField] GameObject food = null;
    [SerializeField] GameObject redFood = null;
    [SerializeField] AudioSource newRedFoodSound = null;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
    int score2 = 0;
    [SerializeField] Text score2Text = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;

    [Header("Borders")]
    [SerializeField] Transform borderTop = null;
    [SerializeField] Transform borderBottom = null;
    [SerializeField] Transform borderLeft = null;
    [SerializeField] Transform borderRight = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelVictory = null;
    [SerializeField] Text victoryText1 = null;
    [SerializeField] Text victoryText2 = null;
    #endregion

    private void Awake()
    {
        manager5 = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
        snakeMovement = head.GetComponent<SnakeMovement>();
        snake2Movement = head2.GetComponent<SnakeMovement>();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    /// <param name="isMultiplayer">True if the game is multiplayer.</param>
    public void StartGame(bool isMultiplayer)
    {
        score = 0;
        score2 = 0;

        GameObject[] activeFood = GameObject.FindGameObjectsWithTag("Game5/Food");

        if (activeFood != null)
        {
            for (int i = 0; i < activeFood.Length; i++)
            {
                Destroy(activeFood[i]);
            }
        }

        GameObject[] activeRedFood = GameObject.FindGameObjectsWithTag("Game5/RedFood");

        if (activeRedFood != null)
        {
            for (int i = 0; i < activeRedFood.Length; i++)
            {
                Destroy(activeRedFood[i]);
            }
        }

        panelMenu.SetActive(false);
        panelControllers.SetActive(true);

        if (!isMultiplayer)
        {
            panelGameOver.SetActive(false);
            multiplayer = false;
            head.transform.position = Vector2.zero;
            head.SetActive(true);
            snakeMovement.enabled = true;
        }

        else
        {
            panelVictory.SetActive(false);
            multiplayer = true;
            head.transform.position = new Vector2(0, 10);
            head.SetActive(true);
            head2.transform.position = new Vector2(0, -10);
            head2.SetActive(true);
            snakeMovement.enabled = true;
            snake2Movement.enabled = true;
            score2Text.enabled = true;
        }

        Spawn();
        SpawnRed();
    }

    /// <summary>
    /// Function called to spawn a normal point.
    /// </summary>
    public void Spawn()
    {
        int x = (int)Random.Range(borderLeft.position.x + 1, borderRight.position.x - 1);

        int y = (int)Random.Range(borderBottom.position.y + 1, borderTop.position.y - 1);

        Instantiate(food, new Vector2(x, y), Quaternion.identity);
    }

    /// <summary>
    /// Function called to spawn a red point.
    /// </summary>
    public void SpawnRed()
    {
        StopAllCoroutines();
        StartCoroutine(SpawnRedFood());
    }

    /// <summary>
    /// Function called to increase the score.
    /// </summary>
    /// <param name="scoreValue">How much the score increases.</param>
    /// <param name="player2">True if the scoring player is player 2, false if it is player 1.</param>
    public void UpdateScore(int scoreValue, bool player2)
    {
        if (!player2)
        {
            score += scoreValue;
            scoreText.text = "SCORE: " + score.ToString();
        }

        else
        {
            score2 += scoreValue;
            score2Text.text = "SCORE: " + score2.ToString();
        }
    }

    /// <summary>
    /// Function that activates the Game Over screen.
    /// </summary>
    public void GameOver()
    {
        snakeMovement.enabled = false;
        panelGameOver.SetActive(true);
        panelControllers.SetActive(false);
        StopAllCoroutines();
        SaveHighScore();
    }

    /// <summary>
    /// Function that activates the Victory screen in multiplayer mode.
    /// </summary>
    /// <param name="player1victory">True if the winner is player 1, false if it is player 2.</param>
    public void Victory(bool player1victory)
    {
        snakeMovement.CancelInvoke();
        snakeMovement.enabled = false;
        snake2Movement.CancelInvoke();
        snake2Movement.enabled = false;
        panelVictory.SetActive(true);

        if (!player1victory)
        {
            victoryText2.enabled = true;
            victoryText1.enabled = false;
        }

        else
        {
            victoryText1.enabled = true;
            victoryText2.enabled = false;
        }

        StopAllCoroutines();
    }

    /// <summary>
    /// Function called to load the high score from the PlayerPrefs.
    /// </summary>
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore5", 0);
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore5", score);
            PlayerPrefs.Save();
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
    /// Coroutine that makes a wait before spawning red food.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnRedFood()
    {
        yield return new WaitForSeconds(Random.Range(20, 40));

        int x = (int)Random.Range(borderLeft.position.x + 1, borderRight.position.x - 1);

        int y = (int)Random.Range(borderBottom.position.y + 1, borderTop.position.y - 1);

        GameObject activeRedFood = Instantiate(redFood, new Vector2(x, y), Quaternion.identity);

        newRedFoodSound.Play();
        
        StartCoroutine(DestroyRedFood(activeRedFood));
    }

    /// <summary>
    /// A coroutine that waits before destroying the active red food.
    /// </summary>
    /// <param name="activeRedFood">The red food to be destroyed.</param>
    /// <returns></returns>
    IEnumerator DestroyRedFood(GameObject activeRedFood)
    {
        yield return new WaitForSeconds(20);

        if (activeRedFood != null)
        {
            Destroy(activeRedFood);
        }

        SpawnRed();
    }
}
