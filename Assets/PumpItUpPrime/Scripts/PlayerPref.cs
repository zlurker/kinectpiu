using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ScoreData {
    public float perfect;
    public float miss;
    public float maxCombo;
    public float score;
}

public static class PlayerPref  {
    public static int sceneValueOffset;
      
    public static bool songsRegisted;   
    public static string[] songs;
    public static int songIndex;
    public static float prefRush;

    public static ScoreData playerScore;
    public static float prefSpeed;       
}
