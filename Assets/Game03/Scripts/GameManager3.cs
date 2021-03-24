using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that control the main functions of the game 3.
/// </summary>
public class GameManager3 : MonoBehaviour
{
    #region Variables
    public static GameManager3 manager3;

    [Header("Player")]
    [SerializeField] PlayerController playerController = null;
    [SerializeField] GameObject player = null;
    [SerializeField] GameObject[] lifes = null;
    [SerializeField] GameObject[] bases = null;
    int remainingLifes;
    [SerializeField] GameObject playerExplosion = null;
    [SerializeField] AudioSource playerExplosionAudio = null;
    
    [Header("Enemies")]
    [SerializeField] EnemyController enemyController = null;
    [SerializeField] GameObject enemyHolder = null;
    [SerializeField] GameObject[] enemies = null;
    int enemiesInScreen;
    [SerializeField] GameObject enemyExplosion = null;
    [SerializeField] AudioSource enemyExplosionAudio = null;

    [Header("UFO")]
    [SerializeField] GameObject ufo = null;
    [SerializeField] GameObject ufoExplosion = null;
    [SerializeField] AudioSource ufoExplosionAudio = null;
    [Range (0, 100)] [SerializeField] float ufoProbability = 25;
    [SerializeField] float ufoWaitTime = 10;

    [Header("Score")]
    int score;
    [SerializeField] Text scoreText = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;
    [SerializeField] Text highScoreMenu = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelControllers = null;
    [SerializeField] GameObject panelBlack = null;
    #endregion

    void Awake()
    {
        manager3 = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        LoadHighScore();
        highScoreMenu.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to start a new game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);
        
        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].SetActive(true);
        }
        remainingLifes = 4;


        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }
        enemyHolder.SetActive(true);
        enemiesInScreen = 55;

        for (int i = 0; i < bases.Length; i++)
        {
            bases[i].SetActive(true);
            bases[i].GetComponent<BaseHealth>().Restart();
        }
        
        player.SetActive(true);
        scoreText.enabled = true;
        highScoreText.enabled = true;
        score = 0;
        scoreText.text = "SCORE: " + score.ToString();
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();

        enemyController.StartPlay(true);
        StartCoroutine(WaitForUFO());
    }

    /// <summary>
    /// Function called to continue playing after defeating a wave.
    /// </summary>
    public void ContinuePlaying()
    {
        playerController.enabled = true;
        
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
        }
        enemiesInScreen = 55;
        
        enemyController.StartPlay(true);
        StopAllCoroutines();
        StartCoroutine(WaitForUFO());
    }

    /// <summary>
    /// Function called to load the High Score from the PlayerPrefs.
    /// </summary>
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore3", 0);
    }

    /// <summary>
    /// Function called to save the High Score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore3", score);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Function called to update the score.
    /// </summary>
    /// <param name="scoreValue">Value to be added to the current score.</param>
    void UpdateScore(int scoreValue)
    {
        score += scoreValue;
        scoreText.text = "SCORE: " + score.ToString();
    }

    /// <summary>
    /// Function called to lower the lives of the player after being destroyed.
    /// </summary>
    /// <param name="lifesLost">Number of lives the player will lose.</param>
    public void LoseHealth(int lifesLost)
    {
        remainingLifes -= lifesLost;
        StartCoroutine(Death());
    }

    /// <summary>
    /// Function that makes a UFO appear on the screen.
    /// </summary>
    void GenerateUFO()
    {
        Instantiate(ufo, new Vector2(9.75f, 4f), Quaternion.identity);
    }

    /// <summary>
    /// Function called when the player destroys a UFO.
    /// </summary>
    /// <param name="ufoScore">Score that the UFO will give us.</param>
    /// <param name="location">Position of the UFO at the time of being destroyed.</param>
    public void UFODeath(int ufoScore, Vector2 location)
    {
        Destroy(Instantiate(ufoExplosion, location, Quaternion.identity), 2);
        ufoExplosionAudio.Play();
        UpdateScore(ufoScore);
    }

    /// <summary>
    /// Function called when the player destroys an enemy.
    /// </summary>
    /// <param name="enemyScore">Score that the enemy will give us.</param>
    /// <param name="location">Position of the enemy at the time of being destroyed.</param>
    public void EnemyDeath(int enemyScore, Vector2 location)
    {
        Destroy(Instantiate(enemyExplosion, location, Quaternion.identity), 0.5f);
        enemyExplosionAudio.Play();
        enemyController.DecreaseWaitTime();
        UpdateScore(enemyScore);
        enemiesInScreen -= 1;
        if (enemiesInScreen <= 0)
        {
            StartCoroutine(Victory());
        }
    }

    /// <summary>
    /// Function to pause and resume the game.
    /// </summary>
    public void PauseGame()
    {
        if (!panelPause.activeSelf)
        {
            panelPause.SetActive(true);
            panelControllers.SetActive(false);
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        else if (panelPause.activeSelf)
        {
            panelPause.SetActive(false);
            panelControllers.SetActive(true);
            Time.timeScale = 1;
            AudioListener.volume = 1;
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

    /// <summary>
    /// Coroutine started when the player dies.
    /// </summary>
    /// <returns></returns>
    IEnumerator Death()
    {
        enemyController.stopMoving = true;
        player.SetActive(false);

        if (remainingLifes > 0)
        {
            for (int i = remainingLifes - 1; i < lifes.Length; i++)
            {
                lifes[i].SetActive(false);
            }
            Destroy(Instantiate(playerExplosion, player.transform.position, Quaternion.identity), 2);
            playerExplosionAudio.Play();
            yield return new WaitForSeconds(2);
            player.SetActive(true);
            player.transform.position = new Vector2(0, -4);
            enemyController.StartPlay(false);
        }
        else
        {
            for (int i = 0; i < lifes.Length; i++)
            {
                lifes[i].SetActive(false);
            }

            Destroy(Instantiate(playerExplosion, player.transform.position, Quaternion.identity), 2);
            playerExplosionAudio.Play();

            GameObject[] ufos = GameObject.FindGameObjectsWithTag("Game3/UFO");
            if (ufos != null)
            {
                for (int i = 0; i < ufos.Length; i++)
                {
                    Destroy(ufos[i]);
                }
            }
            
            panelGameOver.SetActive(true);
            panelControllers.SetActive(false);
            SaveHighScore();
            LoadHighScore();
            StopAllCoroutines();
        }
    }

    /// <summary>
    /// Coroutine started when the player defeats a wave of enemies.
    /// </summary>
    /// <returns></returns>
    IEnumerator Victory()
    {
        playerController.enabled = false;
        enemyController.stopMoving = true;

        SaveHighScore();
        LoadHighScore();

        yield return new WaitForSeconds(1.5f);

        panelBlack.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        panelBlack.SetActive(false);
        player.transform.position = new Vector2(0, -4);

        ContinuePlaying();
    }

    /// <summary>
    /// A coroutine that randomly calls the function to instantiate a UFO.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForUFO()
    {
        while (true)
        {
            yield return new WaitForSeconds(ufoWaitTime);

            if ((enemiesInScreen <= 0) || (remainingLifes <= 0))
            {
                yield break;
            }

            else if (Random.value < (ufoProbability / 100))
            {
                GenerateUFO();
            }
        }
    }
}
