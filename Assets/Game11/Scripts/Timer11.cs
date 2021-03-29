using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the Game 11 timer.
/// </summary>
public class Timer11 : MonoBehaviour
{
    float startTime;

    float timerControl;
    string secs;
    string millisecs;
    string timerString;

    [SerializeField] Text timerText = null;
    
    void OnEnable()
    {
        startTime = Time.time;
    }

    void Update()
    {
        timerControl = Time.time - startTime;
        secs = ((int)timerControl).ToString();
        millisecs = ((int)(timerControl * 100) % 100).ToString("00");

        timerString = string.Format("{00}:{01}", secs, millisecs);

        timerText.text = timerString;
    }

    /// <summary>
    /// Function that resets the timer at the end of each lap.
    /// </summary>
    public void ResetTimer()
    {
        GameManager11.manager.SaveHighScore(int.Parse(secs), int.Parse(millisecs));

        startTime = Time.time;
    }
}
