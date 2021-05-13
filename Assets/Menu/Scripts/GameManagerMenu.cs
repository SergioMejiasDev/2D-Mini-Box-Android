using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that manages the main menu functions.
/// </summary>
public class GameManagerMenu : MonoBehaviour
{
    #region Variables
    [Header("Panels")]
    [SerializeField] GameObject[] panels = null;

    [Header("Games")]
    [SerializeField] GameObject[] games = null;
    int activeGame = 0;

    [Header("Volume")]
    int volume;
    [SerializeField] Text volumeText = null;
    [SerializeField] GameObject volumeLeftArrow = null;
    [SerializeField] GameObject volumeRightArrow = null;

    [Header("Credits")]
    [SerializeField] GameObject[] creditsPanels = null;
    int activeCredits = 0;

    [Header("Region")]
    [SerializeField] Text regionText = null;
    [SerializeField] GameObject[] regions = null;
    int activeRegionButton = 0;
    #endregion

    private void Start()
    {
        LetterBoxer.AddLetterBoxingCamera();

        CheckVolume();

        UpdateRegionButton();

        if (!SaveManager.saveManager.firstTimeLanguage)
        {
            SaveManager.saveManager.RescueJson();
            OpenPanel(panels[5]);
        }
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
    /// <param name="leftArrow">True if we press the left arrow button, false if we press the right.</param>
    public void ArrowGames(bool leftArrow)
    {
        if (leftArrow)
        {
            activeGame -= 1;

            if (activeGame < 0)
            {
                activeGame = games.Length - 1;
            }
        }

        else
        {
            activeGame += 1;

            if (activeGame >= games.Length)
            {
                activeGame = 0;
            }
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
            AudioListener.volume = (volume / 100f);
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
            AudioListener.volume = (volume / 100f);
            volumeLeftArrow.SetActive(true);
            volumeText.text = volume.ToString() + "%";


            if (volume >= 100)
            {
                volumeRightArrow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Function that loads the audio options.
    /// </summary>
    void LoadOptions()
    {
        volume = SaveManager.saveManager.gameVolume;
    }

    /// <summary>
    /// Function that saves the audio options.
    /// </summary>
    public void SaveOptions()
    {
        SaveManager.saveManager.gameVolume = volume;
        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Function that allows changing the language from the options menu.
    /// </summary>
    /// <param name="newLanguage">The code of the language that we want to activate.</param>
    public void ChangeLanguage(string newLanguage)
    {
        MultilanguageManager.multilanguageManager.ChangeLanguage(newLanguage);
    }

    /// <summary>
    /// Function to delete all saved high scores.
    /// </summary>
    public void ClearHighScores()
    {
        SaveManager.saveManager.DeleteScores();
    }

    /// <summary>
    /// Function called to update the server region.
    /// </summary>
    /// <param name="token">The region code.</param>
    public void UpdateRegion(string token)
    {
        SaveManager.saveManager.activeRegion = token;
        SaveManager.saveManager.SaveOptions();

        UpdateRegionButton();
    }

    /// <summary>
    /// Function that updates the text of the active region accordingly.
    /// </summary>
    void UpdateRegionButton()
    {
        string activeRegion = SaveManager.saveManager.activeRegion;

        switch (activeRegion)
        {
            case "asia":
                regionText.text = "Asia";
                break;
            case "au":
                regionText.text = "Australia";
                break;
            case "cae":
                regionText.text = "Canada East";
                break;
            case "eu":
                regionText.text = "Europe";
                break;
            case "in":
                regionText.text = "India";
                break;
            case "jp":
                regionText.text = "Japan";
                break;
            case "rue":
                regionText.text = "Russia East";
                break;
            case "ru":
                regionText.text = "Russia West";
                break;
            case "za":
                regionText.text = "South Africa";
                break;
            case "sa":
                regionText.text = "South America";
                break;
            case "kr":
                regionText.text = "South Korea";
                break;
            case "us":
                regionText.text = "USA East";
                break;
            case "usw":
                regionText.text = "USA West";
                break;
        }
    }

    /// <summary>
    /// Function to scroll through the menu of available regions.
    /// </summary>
    /// <param name="leftArrow">True if we press the left arrow button, false if we press the right.</param>
    public void ArrowRegions(bool leftArrow)
    {
        if (leftArrow)
        {
            activeRegionButton -= 1;

            if (activeRegionButton < 0)
            {
                activeRegionButton = regions.Length - 1;
            }
        }

        else
        {
            activeRegionButton += 1;

            if (activeRegionButton >= regions.Length)
            {
                activeRegionButton = 0;
            }
        }

        for (int i = 0; i < regions.Length; i++)
        {
            regions[i].SetActive(false);
        }

        regions[activeRegionButton].SetActive(true);
    }

    /// <summary>
    /// Function to scroll through the menu of credits.
    /// </summary>
    public void ArrowCredits()
    {
        activeCredits += 1;

        for (int i = 0; i < creditsPanels.Length; i++)
        {
            creditsPanels[i].SetActive(false);
        }

        if (activeCredits == 3)
        {
            activeCredits = 0;
            OpenPanel(panels[0]);
        }

        creditsPanels[activeCredits].SetActive(true);
    }

    /// <summary>
    /// Function called to open an external link.
    /// </summary>
    /// <param name="link">Link that we want to open according to those referenced below.</param>
    public void OpenURL(string link)
    {
        switch (link)
        {
            case "GooglePlay":
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.SergioMejias.MiniBox2D");
                break;
            case "ItchIo":
                Application.OpenURL("https://sergiomejias.itch.io/2d-mini-box");
                break;
            case "GitHub":
                Application.OpenURL("https://github.com/SergioMejiasDev/2D-Mini-Box-Android");
                break;
        }
    }
}
