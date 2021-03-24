using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that controls the main functions of Game 05.
/// </summary>
public class GameManager5 : MonoBehaviour
{
    #region Variables
    public static GameManager5 manager5;

    [Header("Players")]
    [SerializeField] GameObject head = null;
    [SerializeField] SnakeMovement snakeMovement;

    [Header("Food")]
    [SerializeField] GameObject food = null;
    [SerializeField] GameObject redFood = null;
    [SerializeField] AudioSource newRedFoodSound = null;
    [SerializeField] LayerMask snakeMask = 0;
    int spawnAttemps = 0;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
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
    [SerializeField] GameObject waiting = null;
    #endregion

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
        manager5 = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    public void StartGame()
    {
        score = 0;

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

        panelControllers.SetActive(true);
        panelMenu.SetActive(false);
        waiting.SetActive(false);
        score2Text.enabled = false;
        panelGameOver.SetActive(false);

        head.transform.position = Vector2.zero;
        head.SetActive(true);
        snakeMovement.enabled = true;

        Spawn();
        SpawnRed();
    }

    /// <summary>
    /// Function called to spawn a normal point.
    /// </summary>
    public void Spawn()
    {
        Instantiate(food, SpawnVector(), Quaternion.identity);
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
    public void UpdateScore(int scoreValue)
    {
        score += scoreValue;
        scoreText.text = "SCORE: " + score.ToString();
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

        GameObject activeRedFood = Instantiate(redFood, SpawnVector(), Quaternion.identity);

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