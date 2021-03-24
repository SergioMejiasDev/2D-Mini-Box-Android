using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that controls the main functions of Game 09.
/// </summary>
public class GameManager9 : MonoBehaviour
{
    public static GameManager9 manager;
    public delegate void Manager9Delegate(float newSpeed);
    public static event Manager9Delegate ChangeSpeed;
    public static event Manager9Delegate StopMovement;

    [Header("Player")]
    [SerializeField] GameObject player = null;
    [SerializeField] Dinosaur playerClass = null;

    [Header("Movement")]
    public float speed;
    float maxWait;
    float minWait;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
    int highScore = 0;
    [SerializeField] Text highScoreText = null;
    [SerializeField] Text highScoreMenuText = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;

    [Header("Sounds")]
    [SerializeField] AudioSource increaseSound = null;
    [SerializeField] AudioSource gameOverSound = null;

    private void Awake()
    {
        manager = this;
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
        score = 0;
        scoreText.text = "Score: 0";

        panelGameOver.SetActive(false);
        panelMenu.SetActive(false);
        panelControllers.SetActive(true);

        player.SetActive(true);
        playerClass.ResetValues();

        minWait = 1f;
        maxWait = 1.5f;

        CleanScene();

        StartCoroutine(InstantiateCloud());
        StartCoroutine(IncreaseScore());
        StartCoroutine(InstantiateObject());

        speed = 7f;
        ChangeSpeed(speed);
    }

    /// <summary>
    /// Function that increases the speed of objects.
    /// </summary>
    void IncreaseSpeed()
    {
        minWait -= 0.025f;
        maxWait -= 0.025f;

        speed += 0.5f;
        ChangeSpeed(speed);
    }

    /// <summary>
    /// Function that activates the Game Over.
    /// </summary>
    public void GameOver()
    {
        gameOverSound.Play();

        panelGameOver.SetActive(true);
        panelControllers.SetActive(false);

        SaveHighScore();
        
        speed = 0;
        StopMovement(0);

        StopAllCoroutines();
    }

    /// <summary>
    /// Function that removes all objects from the scene after restarting the game.
    /// </summary>
    void CleanScene()
    {
        GameObject[] cactus1 = GameObject.FindGameObjectsWithTag("Game9/Cactus1");
        GameObject[] cactus2 = GameObject.FindGameObjectsWithTag("Game9/Cactus2");
        GameObject[] cactus3 = GameObject.FindGameObjectsWithTag("Game9/Cactus3");
        GameObject[] cactus4 = GameObject.FindGameObjectsWithTag("Game9/Cactus4");
        GameObject[] cactus5 = GameObject.FindGameObjectsWithTag("Game9/Cactus5");
        GameObject[] cactus6 = GameObject.FindGameObjectsWithTag("Game9/Cactus6");
        GameObject[] bird = GameObject.FindGameObjectsWithTag("Game9/Bird");
        GameObject[] cloud = GameObject.FindGameObjectsWithTag("Game9/Cloud");

        GameObject[] allObjects = cactus1.Concat(cactus2).Concat(cactus3).Concat(cactus4).Concat(cactus5).
            Concat(cactus6).Concat(bird).Concat(cloud).ToArray();

        for (int i = 0; i < allObjects.Length; i++)
        {
            allObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Function called to load the high score from the PlayerPrefs.
    /// </summary>
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore9", 0);
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        highScoreMenuText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore9", score);
            PlayerPrefs.Save();

            LoadHighScore();
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
    /// Coroutine that randomly instantiates clouds.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateCloud()
    {
        while (true)
        {
            GameObject cloud = ObjectPooler.SharedInstance.GetPooledObject("Game9/Cloud");

            if (cloud != null)
            {
                cloud.transform.position = new Vector2(8, Random.Range(0f, 2.75f));
                cloud.transform.rotation = Quaternion.identity;
                cloud.SetActive(true);
            }

            yield return new WaitForSeconds(Random.Range(5f, 15f));
        }
    }

    /// <summary>
    /// Coroutine that constantly increases the score.
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseScore()
    {
        int temporalScore = 0;

        while (true)
        {
            score += 1;
            scoreText.text = "Score: " + score.ToString();

            temporalScore += 1;

            if (temporalScore >= 100)
            {
                temporalScore = 0;

                increaseSound.Play();

                IncreaseSpeed();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Coroutine that randomly instantiates objects (cactus or birds).
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));

            if (Random.value > 0.1f)
            {
                GameObject cactus = RandomCactus();

                if (cactus != null)
                {
                    cactus.transform.position = new Vector2(8f, -1.48f);
                    cactus.transform.rotation = Quaternion.identity;
                    cactus.SetActive(true);
                }
            }

            else
            {
                GameObject bird = ObjectPooler.SharedInstance.GetPooledObject("Game9/Bird");

                if (bird != null)
                {
                    bird.transform.position = BirdPosition();
                    bird.transform.rotation = Quaternion.identity;
                    bird.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// Function that decides which cactus is to be instantiated.
    /// </summary>
    /// <returns>One of the six possible types of cactus.</returns>
    GameObject RandomCactus()
    {
        int randomNumber = Random.Range(1, 7);

        switch (randomNumber)
        {
            case 1:
                return ObjectPooler.SharedInstance.GetPooledObject("Game9/Cactus1");
            case 2:
                return ObjectPooler.SharedInstance.GetPooledObject("Game9/Cactus2");
            case 3:
                return ObjectPooler.SharedInstance.GetPooledObject("Game9/Cactus3");
            case 4:
                return ObjectPooler.SharedInstance.GetPooledObject("Game9/Cactus4");
            case 5:
                return ObjectPooler.SharedInstance.GetPooledObject("Game9/Cactus5");
            default:
                return ObjectPooler.SharedInstance.GetPooledObject("Game9/Cactus6");
        }
    }

    /// <summary>
    /// Function to know where a bird is going to be instantiated.
    /// </summary>
    /// <returns>One of three possible positions.</returns>
    Vector2 BirdPosition()
    {
        int randomNumber = Random.Range(1, 4);

        switch (randomNumber)
        {
            case 1:
                return new Vector2(8f, -1.38f);
            case 2:
                return new Vector2(8f, -0.63f);
            default:
                return new Vector2(8f, 0.85f);
        }
    }
}
