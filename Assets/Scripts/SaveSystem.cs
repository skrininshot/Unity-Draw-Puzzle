using UnityEngine;

public static class SaveSystem
{
    public static int passedLevels
    {
        get
        {
            return PlayerPrefs.GetInt("Passed Levels", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Passed Levels", value);
        }
    }

    public static int lastPlayedLevel
    {
        get
        {
            return PlayerPrefs.GetInt("Last Played Level", 1);
        }
        set
        {
            PlayerPrefs.SetInt("Last Played Level", value);
        }
    }

    public static void ClearData()
    {
        passedLevels = 1;
        lastPlayedLevel = 1;
    }
}
