using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerDataCreator : MonoBehaviour {

    public StepchartMover[] stepcharts;

    public AudioSource songPlayer;
    public float cRealTime;
    public float offset;
    public string dataPath;


    void Start () {
        for (var i = 0; i < stepcharts.Length; i++) {
            stepcharts[i].InitialiseStepchart();
            stepcharts[i].playerManager = this;
        }

        string endExt = ".wav";
        dataPath = Application.dataPath;

#if UNITY_ANDROID
        dataPath = Application.persistentDataPath;
        endExt = ".mp3";
#endif

        WWW song = new WWW("file:///" + Path.Combine(Path.Combine(dataPath, "Songs"), PlayerPref.songs[PlayerPref.songIndex] + endExt));

        while (!song.isDone) ;
        songPlayer.clip = song.GetAudioClip(false);

        songPlayer.pitch = PlayerPref.prefRush;
        songPlayer.Play();
        offset += Time.realtimeSinceStartup;
    }
	
	void Update () {
        cRealTime = Time.realtimeSinceStartup - offset;	
	}
}
