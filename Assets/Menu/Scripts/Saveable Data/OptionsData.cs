using System;

/// <summary>
/// Class with the possible options variables that can be saved.
/// </summary>
[Serializable]
public class OptionsData
{
    public string activeLanguage;
    public string activeRegion;
    public bool firstTimeLanguage;
    public int gameVolume;
}