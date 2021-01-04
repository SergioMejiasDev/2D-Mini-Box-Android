using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

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

    [Header("Options")]
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] Slider soundSlider = null;
    float soundVolume;
    #endregion

    private void Start()
    {
        LoadOptions();
    }

    private void Update()
    {
        soundVolume = soundSlider.value;

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(soundVolume) * 25);
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
    /// Function that changes to the previous game.
    /// </summary>
    public void ArrowLeft()
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

    /// <summary>
    /// Function that changes to the next game.
    /// </summary>
    public void ArrowRight()
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
        if (PlayerPrefs.HasKey("GameVolume"))
        {
            float soundVolumeLoaded = PlayerPrefs.GetFloat("GameVolume");
            soundSlider.value = soundVolumeLoaded;
        }
    }

    /// <summary>
    /// Function that saves the options in the PlayerPrefs.
    /// </summary>
    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("GameVolume", soundVolume);
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
        PlayerPrefs.Save();
    }
}
