using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreScreenHandler : MonoBehaviour {

    public Text perfect;
    public Text miss;
    public Text maxCombo;
    public Text score;
    public Text timeLeft;
    float timer;

    void Start() {
        perfect.text = PlayerPref.playerScore.perfect.ToString();
        miss.text = PlayerPref.playerScore.miss.ToString();
        maxCombo.text = PlayerPref.playerScore.maxCombo.ToString();
        score.text = PlayerPref.playerScore.score.ToString();
        timer = Time.time + 10;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Time.time >= timer)
            SceneManager.LoadScene(0 + PlayerPref.sceneValueOffset);

        timeLeft.text = Mathf.Ceil(timer - Time.time).ToString();
    }

}
