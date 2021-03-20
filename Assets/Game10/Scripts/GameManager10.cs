using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the main functions of Game 10.
/// </summary>
public class GameManager10 : MonoBehaviour
{
    public static GameManager10 manager;

    public delegate void Manager10Delegate();
    public static event Manager10Delegate StopMovement;

    [Header("Player")]
    [SerializeField] GameObject player = null;
    [SerializeField] QBertMovement playerClass = null;
    int remainingLifes = 3;
    [SerializeField] Image[] lifes = null;

    [Header("Block")]
    [SerializeField] QBertBlocks[] blocks = null;
    int remainingBlocks = 28;

    [Header("Discs")]
    [SerializeField] GameObject disc1 = null;
    [SerializeField] GameObject disc2 = null;
    [SerializeField] GameObject discCollider1 = null;
    [SerializeField] GameObject discCollider2 = null;

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

    [Header("Sounds")]
    [SerializeField] AudioSource fallSound = null;
    [SerializeField] AudioSource speechSound = null;
    [SerializeField] AudioSource fallSnakeSound = null;
    [SerializeField] AudioSource winSound = null;

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

        disc1.transform.position = new Vector2(-2.46f, -1.15f);
        disc1.SetActive(true);
        disc2.transform.position = new Vector2(2.46f, -1.15f);
        disc2.SetActive(true);
        discCollider1.SetActive(false);
        discCollider2.SetActive(false);

        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);

        player.transform.position = new Vector2(0, 2.76f);
        player.SetActive(true);

        remainingLifes = 3;

        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].enabled = true;
        }

        StartCoroutine(GenerateObject());

        remainingBlocks = 28;
        
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].ResetSprite();
        }
    }

    /// <summary>
    /// Function that restarts the game after turning all the blocks yellow.
    /// </summary>
    void ContinuePlaying()
    {
        disc1.transform.position = new Vector2(-2.46f, -1.15f);
        disc1.SetActive(true);
        disc2.transform.position = new Vector2(2.46f, -1.15f);
        disc2.SetActive(true);
        discCollider1.SetActive(false);
        discCollider2.SetActive(false);

        player.transform.position = new Vector2(0, 2.76f);
        player.SetActive(true);

        StartCoroutine(GenerateObject());

        remainingBlocks = 28;

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].ResetSprite();
        }
    }

    /// <summary>
    /// Function that removes all objects from the scene.
    /// </summary>
    void CleanScene()
    {
        GameObject[] green = GameObject.FindGameObjectsWithTag("Game10/GreenBall");
        GameObject[] red = GameObject.FindGameObjectsWithTag("Game10/RedBall");
        GameObject[] purple = GameObject.FindGameObjectsWithTag("Game10/PurpleBall");

        GameObject[] allObjects = green.Concat(red).Concat(purple).ToArray();

        for (int i = 0; i < allObjects.Length; i++)
        {
            allObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Function that resets the player's position and launches the spawn coroutines.
    /// </summary>
    public void Restart()
    {
        player.transform.position = new Vector2(0, 2.76f);
        player.SetActive(true);

        StartCoroutine(GenerateObject());
    }

    /// <summary>
    /// Function that is activated when the player makes contact with an enemy.
    /// </summary>
    public void DeathHit()
    {
        if (StopMovement != null)
        {
            StopMovement();
        }

        speechSound.Play();

        StopAllCoroutines();

        StartCoroutine(WaitForRespawn());
    }

    /// <summary>
    /// Function that is activated when the player falls into the void.
    /// </summary>
    public void DeathFall()
    {
        fallSound.Play();

        StopAllCoroutines();

        StartCoroutine(WaitForRespawn());
    }

    /// <summary>
    /// Function that increases the score.
    /// </summary>
    /// <param name="increase">How much the score increases.</param>
    public void UpdateScore(int increase)
    {
        score += increase;
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Function that activates or deactivates the colliders of the blocks.
    /// </summary>
    /// <param name="enable">Enable or disable.</param>
    public void EnableBlocks(bool enable)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].EnableOrDisable(enable);
        }
    }

    /// <summary>
    /// Function that reduces the number of blocks required to reach the goal.
    /// </summary>
    public void ReduceBlocks()
    {
        remainingBlocks -= 1;

        if (remainingBlocks == 0)
        {
            StopAllCoroutines();

            StartCoroutine(WinGame());
        }
    }

    /// <summary>
    /// Function that is activated when a snake falls into the void.
    /// </summary>
    public void FallSnake()
    {
        fallSnakeSound.Play();

        UpdateScore(100);
    }

    /// <summary>
    /// Corroutine that is responsible for generating objects randomly.
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 5f));

            GameObject randomObject = RandomObject();

            if (randomObject != null)
            {
                randomObject.transform.position = SpawnPoint();
                randomObject.transform.rotation = Quaternion.identity;
                randomObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Corroutine that is activated whenever the player dies, and is responsible for respawn or activate Game Over.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(3);

        remainingLifes -= 1;

        if (remainingLifes >= 0)
        {
            lifes[remainingLifes].enabled = false;
        }

        else
        {
            SaveHighScore();
            CleanScene();
            player.SetActive(false);
            panelGameOver.SetActive(true);
            panelControllers.SetActive(false);

            yield break;
        }

        panelBlack.SetActive(true);

        CleanScene();
        player.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        panelBlack.SetActive(false);

        Restart();
    }

    /// <summary>
    /// Corroutine that is activated when the player turns all the yellow blocks, and is responsible for resetting the scene little by little.
    /// </summary>
    /// <returns></returns>
    IEnumerator WinGame()
    {
        if (StopMovement != null)
        {
            StopMovement();
        }

        playerClass.WinGame();

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].EnableAnimator(true);
        }

        winSound.Play();

        yield return new WaitForSeconds(3);

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].EnableAnimator(false);
        }

        if (disc1.activeSelf)
        {
            disc1.SetActive(false);

            UpdateScore(25);

            yield return new WaitForSeconds(1);
        }

        if (disc2.activeSelf)
        {
            disc2.SetActive(false);

            UpdateScore(25);

            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(0.5f);

        panelBlack.SetActive(true);

        CleanScene();

        player.SetActive(false);

        UpdateScore(250);

        SaveHighScore();

        yield return new WaitForSeconds(0.5f);

        ContinuePlaying();

        panelBlack.SetActive(false);
    }

    /// <summary>
    /// Function called to load the high score from the PlayerPrefs.
    /// </summary>
    void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore10", 0);
        highScoreText.text = "HIGH SCORE: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score in the PlayerPrefs.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore10", score);
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
    /// A function called to see where random objects are instantiated.
    /// </summary>
    /// <returns>A Vector2 with one of two possible positions.</returns>
    Vector2 SpawnPoint()
    {
        if (Random.value < 0.5f)
        {
            return new Vector2(-0.5f, 2.68f);
        }

        else
        {
            return new Vector2(0.5f, 2.68f);
        }
    }

    /// <summary>
    /// Function that is activated to see what random object is generated.
    /// </summary>
    /// <returns>One of three possible objects, with different possibilities each.</returns>
    GameObject RandomObject()
    {
        float randomNumber = Random.value;

        if (randomNumber < 0.05f)
        {
            return ObjectPooler.SharedInstance.GetPooledObject("Game10/PurpleBall");
        }

        else if (randomNumber < 0.15f)
        {
            return ObjectPooler.SharedInstance.GetPooledObject("Game10/GreenBall");
        }

        else
        {
            return ObjectPooler.SharedInstance.GetPooledObject("Game10/RedBall");
        }
    }
}
