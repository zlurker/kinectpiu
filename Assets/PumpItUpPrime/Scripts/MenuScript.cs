using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    [System.Serializable]
    public struct SongData {
        public string songName;
        public AudioClip music;
        public float offset;
    }

    public SongData[] songs;
    public int currentSongPick;
    public float speed;
    public float rush;

    public Text songTitle;
    public Text currSpeed;
    public Text currRush;

	// Use this for initialization
	void Start () {
        currentSongPick = 0;
        
	    if (PlayerPref.prefRush == 0) {
            PlayerPref.prefRush = 1;
            PlayerPref.prefSpeed = 2;
        }

        speed = PlayerPref.prefSpeed;
        rush = PlayerPref.prefRush;

        RefreshUI();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeSpeed(float value) {
            if (speed + value > 0 && speed + value < 7)
                speed += value;
        RefreshUI();
          
    }

    public void ChangeRush(float value) {
        if (rush + value > 0.7f && rush+ value <1.6f)
            rush += value;
        RefreshUI();
    }

    public void ChangeMusicMenu(int value) {
        if (currentSongPick + value > -1 && currentSongPick + value < songs.Length) {
            currentSongPick += value;
        }
        RefreshUI();
    }

    void RefreshUI() {
        songTitle.text = songs[currentSongPick].songName;
        currSpeed.text = speed.ToString();
        currRush.text = rush.ToString();
    }

    public void LoadLevel() {
        PlayerPref.prefRush = rush;
        PlayerPref.prefSpeed = speed;

        PlayerPref.songName = songs[currentSongPick].songName;
        PlayerPref.song = songs[currentSongPick].music;
        PlayerPref.songOffset = songs[currentSongPick].offset;
        SceneManager.LoadScene("DemoLevel");
    }
}
