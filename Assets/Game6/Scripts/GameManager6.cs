using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that controls the main functions of Game 6.
/// </summary>
public class GameManager6 : MonoBehaviour
{
    public static GameManager6 manager6;

    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    [SerializeField] GameObject spawnerObject = null;
    TetrominoSpawner spawner = null;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;

    [Header("Sounds")]
    [SerializeField] AudioSource deleteRowSound = null;
    [SerializeField] AudioSource gameOverSound = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;


    #region Booleans
    /// <summary>
    /// Vector that approximates the values of another vector entered.
    /// </summary>
    /// <param name="v">The vector that we want to approximate.</param>
    /// <returns>The approximate vector with integer values.</returns>
    public Vector2 RoundVec2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    /// <summary>
    /// Boolean that checks if the position entered is within the game grid.
    /// </summary>
    /// <param name="position">Position that we want to know if it is inside the grid.</param>
    /// <returns>True if the position entered is within the grid, false if isn't.</returns>
    public bool InsideBorder(Vector2 position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0;
    }

    /// <summary>
    /// Boolean that checks if a row is complete.
    /// </summary>
    /// <param name="y">The row we want to check.</param>
    /// <returns>True if the row is full, false if it is not.</returns>
    public bool IsRowFull(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }
    #endregion

    void Awake()
    {
        manager6 = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
        spawner = spawnerObject.GetComponent<TetrominoSpawner>();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);

        score = 0;
        scoreText.text = "SCORE: " + score.ToString();

        spawnerObject.SetActive(true);
        spawner.Spawn();
    }

    /// <summary>
    /// Function that removes an entire row.
    /// </summary>
    /// <param name="y">The row we want to delete.</param>
    public void DeleteRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    /// <summary>
    /// Function that makes an entire row go down one level.
    /// </summary>
    /// <param name="y">The row that we want to descend.</param>
    public void DecreaseRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    /// <summary>
    /// Function that makes all rows higher than the one entered down one level.
    /// </summary>
    /// <param name="y">Row from which we want the upper ones to descend.</param>
    public void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height; ++i)
        {
            DecreaseRow(i);
        }
    }

    /// <summary>
    /// Function that checks if any of the rows are complete and, if applicable, deletes them.
    /// </summary>
    public void DeleteFullRows()
    {
        for (int y = 0; y < height; ++y)
        {
            if (IsRowFull(y))
            {
                deleteRowSound.Play();

                UpdateScore(50);
                DeleteRow(y);
                DecreaseRowsAbove(y + 1);
                --y;
            }
        }
    }

    /// <summary>
    /// Function we call to increase the score.
    /// </summary>
    /// <param name="scoreIncrease">How much the score increases.</param>
    public void UpdateScore(int scoreIncrease)
    {
        score += scoreIncrease;
        scoreText.text = "SCORE: " + score.ToString();
    }

    /// <summary>
    /// Function that we call when a Game Over occurs.
    /// </summary>
    public void GameOver()
    {
        gameOverSound.Play();
        spawnerObject.SetActive(false);
        panelGameOver.SetActive(true);
        panelControllers.SetActive(false);

        GameObject[] activeTetrominos = GameObject.FindGameObjectsWithTag("Game6/Tetromino");

        if (activeTetrominos != null)
        {
            for (int i = 0; i < activeTetrominos.Length; i++)
            {
                Destroy(activeTetrominos[i]);
            }
        }

        SaveHighScore();
    }

    /// <summary>
    /// Function called to load the high score from the PlayerPrefs.
    /// </summary>
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore6", 0);
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore6", score);
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
}
