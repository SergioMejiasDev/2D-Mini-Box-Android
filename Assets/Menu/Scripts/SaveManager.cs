using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Class that manages saving and loading of scores and options in a binary file.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;

    [Header("Options")]
    public string activeLanguage = "EN";
    public string activeRegion = "eu";
    public bool firstTimeLanguage = false;
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
    public int[] score11 = new int[2] { 0, 0 };
    public int score12 = 0;

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

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    /// <summary>
    /// Function to save options values.
    /// </summary>
    public void LoadOptions()
    {
        OptionsData data = new OptionsData();

        string path = Application.persistentDataPath + "/Options.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as OptionsData;
            stream.Close();

            activeLanguage = data.activeLanguage;
            activeRegion = data.activeRegion;
            firstTimeLanguage = data.firstTimeLanguage;
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
            firstTimeLanguage = firstTimeLanguage,
            gameVolume = gameVolume,
        };

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Options.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }

    /// <summary>
    /// Function to save scores.
    /// </summary>
    public void LoadScores()
    {
        ScoreData data = new ScoreData();

        string path = Application.persistentDataPath + "/Scores.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as ScoreData;
            stream.Close();

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
            score11 = data.score11;
            score12 = data.score12;
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
            score10 = score10,
            score11 = score11,
            score12 = score12
        };

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Scores.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
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
        score11 = new int[] { 0, 0 };
        score12 = 0;

        SaveScores();
    }

    /// <summary>
    /// Function called to rescue the scores saved in the JSON file before using the saving in bynary.
    /// </summary>
    public void RescueJson()
    {
        ScoreData data = new ScoreData();

        string json;

        string path = Application.persistentDataPath + "/Scores.json";

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
            score11 = data.score11;
            score12 = data.score12;
        }

        SaveScores();
    }
}