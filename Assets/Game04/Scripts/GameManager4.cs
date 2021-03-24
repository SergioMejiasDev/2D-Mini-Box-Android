using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class to control the main functions of the game 4.
/// </summary>
public class GameManager4 : MonoBehaviour
{
    #region Variables
    public static GameManager4 manager4;
    public delegate void Manager4Delegate();
    public static event Manager4Delegate ResetPositions;
    public static event Manager4Delegate PlayerWin;

    [Header("Player")]
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject[] lifes = null;
    int remainingLifes = 4;
    [SerializeField] PacmanMovement pacmanMovement = null;

    [Header("Enemies")]
    [SerializeField] GameObject[] enemies = null;
    public int enemiesInScreen = 4;
    [SerializeField] GameObject number20 = null;
    [SerializeField] GameObject number40 = null;
    [SerializeField] GameObject number80 = null;
    [SerializeField] GameObject number160 = null;
    [SerializeField] AudioSource enemyEatenSound = null;

    [Header("Dots")]
    [SerializeField] GameObject[] dots = null;
    int dotsInScreen = 325;
    [SerializeField] GameObject[] bigDots = null;

    [Header("Fruits")]
    [SerializeField] GameObject[] fruits = null;
    [SerializeField] AudioSource fruitSound = null;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelBlack = null;
    #endregion

    private void Awake()
    {
        manager4 = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        dots = GameObject.FindGameObjectsWithTag("Game4/Dot");
        LoadHighScore();
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to start the game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelControllers.SetActive(true);

        player.SetActive(true);
        
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }

        GenerateFruit();
    }

    /// <summary>
    /// Function called to replay after Game Over.
    /// </summary>
    public void RestartGame()
    {
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);

        score = 0;
        scoreText.text = "SCORE: 0";

        remainingLifes = 4;

        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].SetActive(true);
        }

        dotsInScreen = 325;

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(true);
        }

        for (int i = 0; i < bigDots.Length; i++)
        {
            bigDots[i].SetActive(true);
        }

        if (ResetPositions != null)
        {
            ResetPositions();
        }

        StopAllCoroutines();

        GameObject[] activeFruit = GameObject.FindGameObjectsWithTag("Game4/Fruit");

        if (activeFruit != null)
        {
            for (int i = 0; i < activeFruit.Length; i++)
            {
                Destroy(activeFruit[i]);
            }
        }

        GenerateFruit();

        player.transform.position = pacmanMovement.startPosition;
        player.SetActive(true);
        pacmanMovement.ResetPosition();
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
    /// Function called when the player eats a dot.
    /// </summary>
    public void DotEaten()
    {
        dotsInScreen -= 1;

        UpdateScore(1);

        if (dotsInScreen == 0)
        {
            StartCoroutine(ContinuePlaying());
        }
    }

    /// <summary>
    /// Function that calls the coroutine to generate the fruit in the center of the map.
    /// </summary>
    public void GenerateFruit()
    {
        StartCoroutine(WaitForFruit());
    }

    /// <summary>
    /// Function that reproduces the sound of the fruit.
    /// </summary>
    public void FruitSound()
    {
        fruitSound.Play();
    }

    /// <summary>
    /// Function called every time the player eats an enemy.
    /// </summary>
    /// <param name="enemyPosition">The position of the enemy eaten.</param>
    public void EnemyEaten(Vector2 enemyPosition)
    {
        enemyEatenSound.Play();

        switch (enemiesInScreen)
        {
            case 4:
                UpdateScore(20);
                Destroy(Instantiate(number20, enemyPosition, Quaternion.identity), 1);
                break;
            case 3:
                UpdateScore(40);
                Destroy(Instantiate(number40, enemyPosition, Quaternion.identity), 1);
                break;
            case 2:
                UpdateScore(80);
                Destroy(Instantiate(number80, enemyPosition, Quaternion.identity), 1);
                break;
            case 1:
                UpdateScore(160);
                Destroy(Instantiate(number160, enemyPosition, Quaternion.identity), 1);
                break;
        }

        enemiesInScreen -= 1;
    }

    /// <summary>
    /// Function called when the player dies.
    /// </summary>
    public void PlayerDeath()
    {
        remainingLifes -= 1;

        if (remainingLifes > 0)
        {
            for (int i = remainingLifes - 1; i < lifes.Length; i++)
            {
                lifes[i].SetActive(false);
            }

            StartCoroutine(ResetPosition());
        }

        else
        {
            panelGameOver.SetActive(true);
            panelControllers.SetActive(false);
            SaveHighScore();
        }
    }

    /// <summary>
    /// Function called to load the high score from the PlayerPrefs.
    /// </summary>
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore4", 0);
    }

    /// <summary>
    /// Function called to save the high score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore4", score);
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
    /// Coroutine that restarts the position of the player and the enemies.
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(1);

        panelBlack.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        panelBlack.SetActive(false);
        if (ResetPositions != null)
        {
            ResetPositions();
        }

        player.transform.position = pacmanMovement.startPosition;
        player.SetActive(true);
        pacmanMovement.ResetPosition();
    }

    /// <summary>
    /// Coroutine that restarts the map when the player has won the game.
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinuePlaying()
    {
        pacmanMovement.enabled = false;
        PlayerWin();

        yield return new WaitForSeconds(2);
        
        panelBlack.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        panelBlack.SetActive(false);

        dotsInScreen = 325;

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(true);
        }

        for (int i = 0; i < bigDots.Length; i++)
        {
            bigDots[i].SetActive(true);
        }

        StopCoroutine(WaitForFruit());

        GameObject[] activeFruit = GameObject.FindGameObjectsWithTag("Game4/Fruit");

        if (activeFruit != null)
        {
            for (int i = 0; i < activeFruit.Length; i++)
            {
                Destroy(activeFruit[i]);
            }
        }

        GenerateFruit();

        if (ResetPositions != null)
        {
            ResetPositions();
        }

        player.transform.position = pacmanMovement.startPosition;
        pacmanMovement.enabled = true;
        pacmanMovement.ResetPosition();
    }

    /// <summary>
    /// Corroutine that makes the wait and generates a random fruit in the center of the map.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForFruit()
    {
        yield return new WaitForSeconds(Random.Range(30, 60));

        float randomNumber = Random.value;
        GameObject fruitToGenerate;

        if (randomNumber <= 0.01f)
        {
            fruitToGenerate = fruits[7];
        }

        else if (randomNumber <= 0.03f)
        {
            fruitToGenerate = fruits[6];
        }

        else if (randomNumber <= 0.07f)
        {
            fruitToGenerate = fruits[5];
        }

        else if (randomNumber <= 0.13f)
        {
            fruitToGenerate = fruits[4];
        }

        else if (randomNumber <= 0.21f)
        {
            fruitToGenerate = fruits[3];
        }

        else if (randomNumber <= 0.37f)
        {
            fruitToGenerate = fruits[2];
        }

        else if (randomNumber <= 0.61f)
        {
            fruitToGenerate = fruits[1];
        }

        else
        {
            fruitToGenerate = fruits[0];
        }

        Instantiate(fruitToGenerate, new Vector2(14, 14), Quaternion.identity);
    }
}
