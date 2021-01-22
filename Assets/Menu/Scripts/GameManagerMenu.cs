using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that controls the main functions of the menu.
/// </summary>
public class GameManagerMenu : MonoBehaviour
{
    #region Variables
    [Header("Panels")]
    [SerializeField] GameObject[] panels = null;

    [Header("Games")]
    [SerializeField] GameObject[] games = null;
    int activeGame = 0;
    [SerializeField] GameObject arrowLeft = null;
    [SerializeField] GameObject arrowRight = null;

    [Header("Volume")]
    int volume;
    [SerializeField] Text volumeText = null;
    [SerializeField] GameObject volumeLeftArrow = null;
    [SerializeField] GameObject volumeRightArrow = null;
    #endregion

    private void Start()
    {
        CheckVolume();
    }

    /// <summary>
    /// Function called to load a new game (scene).
    /// </summary>
    /// <param name="buildIndex">Number of the scene to be loaded.</param>
    public void LoadGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Function called to load a random game.
    /// </summary>
    public void LoadRandomGame()
    {
        SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCountInBuildSettings));
    }

    /// <summary>
    /// Function used to navigate between the main menu panels.
    /// </summary>
    /// <param name="panel">The panel to open.</param>
    public void OpenPanel(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        
        panel.SetActive(true);
    }

    /// <summary>
    /// Function that changes the active game.
    /// </summary>
    public void ArrowGames(bool leftArrow)
    {
        if (leftArrow)
        {
            activeGame -= 1;

            arrowRight.SetActive(true);

            if (activeGame <= 0)
            {
                activeGame = 0;
                arrowLeft.SetActive(false);
            }

            for (int i = 0; i < games.Length; i++)
            {
                games[i].SetActive(false);
            }

            games[activeGame].SetActive(true);
        }

        else
        {
            activeGame += 1;

            arrowLeft.SetActive(true);

            if (activeGame >= (games.Length - 1))
            {
                activeGame = games.Length - 1;
                arrowRight.SetActive(false);
            }

            for (int i = 0; i < games.Length; i++)
            {
                games[i].SetActive(false);
            }

            games[activeGame].SetActive(true);
        }
    }

    /// <summary>
    /// Function called to check the volume at the start of the game.
    /// </summary>
    void CheckVolume()
    {
        LoadOptions();

        AudioListener.volume = (volume / 100f);

        if (volume > 0)
        {
            volumeText.text = volume.ToString() + "%";

            if (volume >= 100)
            {
                volumeRightArrow.SetActive(false);
            }
        }

        else
        {
            volumeLeftArrow.SetActive(false);
            volumeText.text = "OFF";
        }
    }

    /// <summary>
    /// Function called to modify the volume of the game.
    /// </summary>
    /// <param name="leftArrow">True if we are lowering the volume.</param>
    public void VolumeManager(bool leftArrow)
    {
        if (leftArrow)
        {
            volume -= 5;
            AudioListener.volume = (volume/100f);
            volumeRightArrow.SetActive(true);

            if (volume > 0)
            {
                volumeText.text = volume.ToString() + "%";
            }

            else
            {
                volumeLeftArrow.SetActive(false);
                volumeText.text = "OFF";
            }
        }

        else
        {
            volume += 5;
            AudioListener.volume = (volume/100f);
            volumeLeftArrow.SetActive(true);
            volumeText.text = volume.ToString() + "%";


            if (volume >= 100)
            {
                volumeRightArrow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Close the game completely.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Function that loads the options from the PlayerPrefs.
    /// </summary>
    void LoadOptions()
    {
        int soundVolumeLoaded = PlayerPrefs.GetInt("GameVolume", 100);
        volume = soundVolumeLoaded;
    }

    /// <summary>
    /// Function that saves the options in the PlayerPrefs.
    /// </summary>
    public void SaveOptions()
    {
        PlayerPrefs.SetInt("GameVolume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Function to delete all saved high scores.
    /// </summary>
    public void ClearHighScores()
    {
        PlayerPrefs.SetInt("HighScore1", 0);
        PlayerPrefs.SetInt("HighScore2-1", 0);
        PlayerPrefs.SetInt("HighScore2-2", 0);
        PlayerPrefs.SetInt("HighScore3", 0);
        PlayerPrefs.SetInt("HighScore4", 0);
        PlayerPrefs.SetInt("HighScore5", 0);
        PlayerPrefs.SetInt("HighScore6", 0);
        PlayerPrefs.Save();
    }
}
