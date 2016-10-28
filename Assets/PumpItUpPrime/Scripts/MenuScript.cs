using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public string path;
    public Text dataPath;

    public Text songTitle;
    public Text currSpeed;
    public Text currRush;

    // Use this for initialization
    void Start() {
        path = Application.dataPath;

#if UNITY_ANDROID
		path = Application.persistentDataPath;;
#endif

        dataPath.text = "Put stepchart files here: " + path;

        if (!PlayerPref.songsRegisted) {
            PlayerPref.songsRegisted = true;

            DirectoryInfo stepchartDirectory = new DirectoryInfo(Path.Combine(path, "Stepcharts"));
            FileInfo[] stepcharts = stepchartDirectory.GetFiles("*.txt");

            PlayerPref.songs = new string[stepcharts.Length];

            for (var i = 0; i < PlayerPref.songs.Length; i++) {
                PlayerPref.songs[i] = stepcharts[i].FullName.Substring(path.Length + 12, stepcharts[i].FullName.Length - path.Length - 12 - 4);//Song's name.
            }

            PlayerPref.prefRush = 1;
            PlayerPref.prefSpeed = 2;
        }

        PlayerPref.playerScore.perfect = 0;
        PlayerPref.playerScore.miss = 0;
        PlayerPref.playerScore.maxCombo = 0;
        PlayerPref.playerScore.score = 0;

        RefreshUI();
    }

    public void ChangeSpeed(float value) {
        if (PlayerPref.prefSpeed + value > 0 && PlayerPref.prefSpeed + value < 7)
            PlayerPref.prefSpeed += value;
        RefreshUI();
    }

    public void ChangeRush(float value) {
        if (PlayerPref.prefRush + value > 0.7f && PlayerPref.prefRush + value < 1.6f)
            PlayerPref.prefRush += value;
        RefreshUI();
    }

    public void ChangeMusicMenu(int value) {
        if (PlayerPref.songIndex + value > -1 && PlayerPref.songIndex + value < PlayerPref.songs.Length) {
            PlayerPref.songIndex += value;
        }
        RefreshUI();
    }

    void RefreshUI() {
        songTitle.text = PlayerPref.songs[PlayerPref.songIndex];
        currSpeed.text = PlayerPref.prefSpeed.ToString();
        currRush.text = PlayerPref.prefRush.ToString();
    }

    public void LoadLevel() {
        SceneManager.LoadScene(1 + PlayerPref.sceneValueOffset);
    }
}
