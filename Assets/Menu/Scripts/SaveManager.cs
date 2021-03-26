using System.IO;
using UnityEngine;

/// <summary>
/// Class with the possible options variables that can be saved.
/// </summary>
public class OptionsData
{
    public string activeLanguage;
    public string activeRegion;
    public bool firsTimeLanguage;
    public int gameVolume;
}

/// <summary>
/// Class with the possible score variables that can be saved.
/// </summary>
public class ScoreData
{
    public int score1;
    public int[] score2 = new int[2];
    public int score3;
    public int score4;
    public int score5;
    public int score6;
    public int score7;
    public int score8;
    public int score9;
    public int score10;
}

/// <summary>
/// Class that manages saving and loading of scores in a JSON file.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;

    [Header("Options")]
    public string activeLanguage = "EN";
    public string activeRegion = "eu";
    public bool firsTimeLanguage = false;
    public int gameVolume = 100;

    [Header("Scores")]
    public int score1 = 0;
    public int[] score2 = new int[2] { 0, 0 };
    public int score3 = 0;
    public int score4 = 0;
    public int score5 = 0;
    public int score6 = 0;
    public int score7 = 0;
    public int score8 = 0;
    public int score9 = 0;
    public int score10 = 0;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Menu/SaveManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            saveManager = this;

            DontDestroyOnLoad(gameObject);

            LoadOptions();
            LoadScores();
        }
    }

    /// <summary>
    /// Function to save options values.
    /// </summary>
    public void LoadOptions()
    {
        OptionsData data = new OptionsData();

        string json;

        string path = Application.persistentDataPath + "/Saves/Options.json";

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }

            JsonUtility.FromJsonOverwrite(json, data);

            activeLanguage = data.activeLanguage;
            activeRegion = data.activeRegion;
            firsTimeLanguage = data.firsTimeLanguage;
            gameVolume = data.gameVolume;
        }
    }

    /// <summary>
    /// Function to load options values.
    /// </summary>
    public void SaveOptions()
    {
        OptionsData data = new OptionsData
        {
            activeLanguage = activeLanguage,
            activeRegion = activeRegion,
            firsTimeLanguage = firsTimeLanguage,
            gameVolume = gameVolume,
        };

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath + "/Saves/Options.json";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    /// <summary>
    /// Function to save scores.
    /// </summary>
    public void LoadScores()
    {
        ScoreData data = new ScoreData();
        
        string json;

        string path = Application.persistentDataPath + "/Saves/Scores.json";

        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }

            JsonUtility.FromJsonOverwrite(json, data);

            score1 = data.score1;
            score2 = data.score2;
            score3 = data.score3;
            score4 = data.score4;
            score5 = data.score5;
            score6 = data.score6;
            score7 = data.score7;
            score8 = data.score8;
            score9 = data.score9;
            score10 = data.score10;
        }
    }

    /// <summary>
    /// Function to load scores.
    /// </summary>
    public void SaveScores()
    {
        ScoreData data = new ScoreData
        {
            score1 = score1,
            score2 = score2,
            score3 = score3,
            score4 = score4,
            score5 = score5,
            score6 = score6,
            score7 = score7,
            score8 = score8,
            score9 = score9,
            score10 = score10
        };

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath + "/Saves/Scores.json";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }

    /// <summary>
    /// Function to delete all scores.
    /// </summary>
    public void DeleteScores()
    {
        score1 = 0;
        score2 = new int[] { 0, 0 };
        score3 = 0;
        score4 = 0;
        score5 = 0;
        score6 = 0;
        score7 = 0;
        score8 = 0;
        score9 = 0;
        score10 = 0;

        SaveScores();
    }

    /// <summary>
    /// Function called to rescue the scores saved in the PlayerPrefs before using the saving in JSON.
    /// </summary>
    public void RescuePlayerPrefs()
    {
        score1 = PlayerPrefs.GetInt("HighScore1", 0);
        score2[0] = PlayerPrefs.GetInt("HighScore2-1", 0);
        score2[1] = PlayerPrefs.GetInt("HighScore2-2", 0);
        score3 = PlayerPrefs.GetInt("HighScore3", 0);
        score4 = PlayerPrefs.GetInt("HighScore4", 0);
        score5 = PlayerPrefs.GetInt("HighScore5", 0);
        score6 = PlayerPrefs.GetInt("HighScore6", 0);
        score7 = PlayerPrefs.GetInt("HighScore7", 0);
        score8 = PlayerPrefs.GetInt("HighScore8", 0);
        score9 = PlayerPrefs.GetInt("HighScore9", 0);
        score10 = PlayerPrefs.GetInt("HighScore10", 0);

        SaveScores();
    }
}
