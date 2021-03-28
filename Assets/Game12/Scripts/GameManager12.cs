using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that controls the main functions of the Game 12.
/// </summary>
public class GameManager12 : MonoBehaviour
{
    public static GameManager12 manager;
    public delegate void Manager12Delegate();
    public static event Manager12Delegate StopMovement;

    [Header("Player")]
    [SerializeField] GameObject player = null;
    [SerializeField] BalloonMovement playerClass = null;
    public bool magnetMode = false;
    [SerializeField] LayerMask objetsMask = 0;

    [Header("Score")]
    int score = 0;
    [SerializeField] Text scoreText = null;
    int highScore = 0;
    [SerializeField] Text highScoreMenuText = null;
    [SerializeField] Text highScoreText = null;

    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    [SerializeField] GameObject panelPause = null;
    [SerializeField] GameObject panelGameOver = null;
    [SerializeField] GameObject panelHelp = null;
    [SerializeField] GameObject panelControllers = null;

    [Header("Sounds")]
    [SerializeField] AudioSource hitSound = null;

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
    /// Function that starts the game.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelGameOver.SetActive(false);
        panelControllers.SetActive(true);

        score = 0;
        scoreText.text = "0";

        player.transform.position = new Vector2(0, -2);
        playerClass.enabled = true;
        playerClass.canMove = true;

        magnetMode = false;

        CleanScene();

        StartCoroutine(InstantiateBubbles());
        StartCoroutine(InstantiateSpikes());
    }

    /// <summary>
    /// Function that removes all objects from the scene after restarting the game.
    /// </summary>
    void CleanScene()
    {
        GameObject[] bubble = GameObject.FindGameObjectsWithTag("Game12/Bubble");
        GameObject[] bubbleColor = GameObject.FindGameObjectsWithTag("Game12/BubbleColor");
        GameObject[] magnet = GameObject.FindGameObjectsWithTag("Game12/Magnet");
        GameObject[] spike1 = GameObject.FindGameObjectsWithTag("Game12/Spike1");
        GameObject[] spike2 = GameObject.FindGameObjectsWithTag("Game12/Spike2");
        GameObject[] spike3 = GameObject.FindGameObjectsWithTag("Game12/Spike3");
        GameObject[] spike4 = GameObject.FindGameObjectsWithTag("Game12/Spike4");
        GameObject[] spike5 = GameObject.FindGameObjectsWithTag("Game12/Spike5");

        GameObject[] allObjects = bubble.Concat(bubbleColor).Concat(magnet).Concat(spike1).Concat(spike2).Concat(spike3).
            Concat(spike4).Concat(spike5).ToArray();

        for (int i = 0; i < allObjects.Length; i++)
        {
            allObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Function called to activate Game Over.
    /// </summary>
    public void GameOver()
    {
        hitSound.Play();

        StopMovement();
        StopAllCoroutines();

        SaveHighScore();
        panelGameOver.SetActive(true);
        panelControllers.SetActive(false);
    }

    /// <summary>
    /// Function to calculate the place where the objects will be instantiated.
    /// </summary>
    /// <returns>A random Vector2 with one of the possible positions.</returns>
    Vector2 RandomPosition()
    {
        float x = Random.Range(-3.6f, 3.6f);

        if (Physics2D.OverlapCircle(new Vector2(x, 6.0f), 1.0f, objetsMask))
        {
            return RandomPosition();
        }

        else
        {
            return new Vector2(x, 6.0f);
        }
    }

    /// <summary>
    /// Coroutine that instantiates objects every so often.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateBubbles()
    {
        while (true)
        {
            if (Random.value < 0.7)
            {
                EnableBubble();
            }

            else if (Random.value < 0.9f)
            {
                EnableBubble();
                EnableBubble();
            }

            else if (Random.value < 0.91f)
            {
                EnableBubbleColor();
            }

            else if (Random.value < 0.92f)
            {
                EnableMagnet();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// Function that instantiates a bubble.
    /// </summary>
    void EnableBubble()
    {
        GameObject bubble = ObjectPooler.SharedInstance.GetPooledObject("Game12/Bubble");

        if (bubble != null)
        {
            bubble.transform.position = RandomPosition();
            bubble.transform.rotation = Quaternion.identity;
            bubble.SetActive(true);
        }
    }

    /// <summary>
    /// Function that instantiates a color bubble.
    /// </summary>
    void EnableBubbleColor()
    {
        GameObject bubble = ObjectPooler.SharedInstance.GetPooledObject("Game12/BubbleColor");

        if (bubble != null)
        {
            bubble.transform.position = RandomPosition();
            bubble.transform.rotation = Quaternion.identity;
            bubble.SetActive(true);
        }
    }

    /// <summary>
    /// Function that instantiates a magnet.
    /// </summary>
    void EnableMagnet()
    {
        GameObject magnet = ObjectPooler.SharedInstance.GetPooledObject("Game12/Magnet");

        if (magnet != null)
        {
            magnet.transform.position = RandomPosition();
            magnet.transform.rotation = Quaternion.identity;
            magnet.SetActive(true);
        }
    }

    /// <summary>
    /// Coroutine that generates spikes every so often.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateSpikes()
    {
        while (true)
        {
            EnableSpike();

            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    ///  Function that instantiates spikes.
    /// </summary>
    void EnableSpike()
    {
        GameObject spike = RandomSpike();

        if (spike != null)
        {
            spike.transform.position = RandomPosition();
            spike.transform.rotation = Quaternion.identity;
            spike.SetActive(true);
        }
    }

    /// <summary>
    /// Function that decides which of the possible types of spikes are instantiated.
    /// </summary>
    /// <returns>One of the five possible types of spikes.</returns>
    GameObject RandomSpike()
    {
        switch (Random.Range(1, 6))
        {
            case 1:
                return ObjectPooler.SharedInstance.GetPooledObject("Game12/Spike1");
            case 2:
                return ObjectPooler.SharedInstance.GetPooledObject("Game12/Spike2");
            case 3:
                return ObjectPooler.SharedInstance.GetPooledObject("Game12/Spike3");
            case 4:
                return ObjectPooler.SharedInstance.GetPooledObject("Game12/Spike4");
            default:
                return ObjectPooler.SharedInstance.GetPooledObject("Game12/Spike5");
        }
    }

    /// <summary>
    /// Function in charge of increasing the score.
    /// </summary>
    /// <param name="increase">How much the score increases.</param>
    public void UpdateScore(int increase)
    {
        score += increase;
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// Function called to load the high score.
    /// </summary>
    void LoadHighScore()
    {
        highScore = SaveManager.saveManager.score12;
        highScoreText.text = highScore.ToString();
        highScoreMenuText.text = "High Score: " + highScore.ToString();
    }

    /// <summary>
    /// Function called to save the high score.
    /// </summary>
    public void SaveHighScore()
    {
        if (score > highScore)
        {
            SaveManager.saveManager.score12 = score;
            SaveManager.saveManager.SaveScores();

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

}
