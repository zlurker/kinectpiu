using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StepchartMover : MonoBehaviour {

    [System.Serializable]
    public struct BPMData {
        public float beat;
        public float bpm;
        public float time;

        public BPMData(float givenBeat, float givenBPM, float calculatedTime) {
            beat = givenBeat;
            bpm = givenBPM;
            time = calculatedTime;
        }
    }

    [System.Serializable]
    public struct SpeedData {
        public float beat;
        public float speed;
        public float timeForChange;
        public float time;

        public SpeedData(float givenBeat, float givenSpeed, float givenTimeForChange, float givenTime) {
            beat = givenBeat;
            speed = givenSpeed;
            timeForChange = givenTimeForChange;
            time = givenTime;
        }
    }

    [System.Serializable]
    public struct BeatsInfo {
        public float beatTiming;
        public GameObject[] beats;

        public BeatsInfo(float givenBeatTiming, GameObject[] givenBeats) {
            beatTiming = givenBeatTiming;
            beats = givenBeats;
        }

    }

    public StepchartReader stepchartBuilder;
    public float offset;
    public float rush;
    public AudioSource song;
    public Animation grade;
    public Text gradeT;
    public Text comboT;
    public KeyCode[] controls;
    public float allowanceTime;

    public List<BPMData> bpmData;
    public List<SpeedData> speedData;
    public List<BeatsInfo> beats;

    [HideInInspector]
    public float endBpm;
    [HideInInspector]
    public float totalDist;
    float bpm;
    float cRealTime;
    float endTime;
    float dOffset;
    float timerForLongBeat;

    int currentBpm;
    int currentBeat;
    int currentSpeed;

    public float prevSpeed;

    int combo;

    float endLongBeatTime;
    int longBeatsActive;
    int[] beatsActive = new int[10];
    public int[] holdingDown = new int[10];

    public float iniLength;
    public SpriteRenderer sprite;
    public Transform[] legs;

    public Text points;
    public float totalPoints;

    void Start() {
        //for (var i = 0; i < legs.Length; i++)
        //KinectManager.Instance.legs[i] = legs[i];
        stepchartBuilder.songName = PlayerPref.songName;
        stepchartBuilder.speed = PlayerPref.prefSpeed;
        song.clip = PlayerPref.song;

        stepchartBuilder.CreateTimingData();
        stepchartBuilder.CreateStepchart();

        offset = PlayerPref.songOffset;
        offset += Time.realtimeSinceStartup;

        rush = PlayerPref.prefRush;
        longBeatsActive = 0;
        currentBeat = 0;
        currentSpeed = 0;
        currentBpm = 0;
        bpm *= rush;
        song.pitch = rush;
        bpm = bpmData[0].bpm;
        endTime = (endBpm / bpm) * 60;
        song.Play();
    }

    void Update() {
        cRealTime = Time.realtimeSinceStartup - offset;

        if (currentBpm < bpmData.Count)
            if (bpmData[currentBpm].time / rush < cRealTime) { //Bpm changer
                ChangeBpm(bpmData[currentBpm].bpm, bpmData[currentBpm].beat);
                currentBpm++;
            }

        if (currentSpeed < speedData.Count)
            if (speedData[currentSpeed].time / rush < cRealTime) { //Speed changer
                if (currentSpeed -1 > -1)
                prevSpeed = speedData[currentSpeed-1].speed;
                currentSpeed++;
            }

        if (currentSpeed - 1 > 0)
            ChangeSpeed(speedData[currentSpeed - 1].speed, speedData[currentSpeed - 1].time / rush, speedData[currentSpeed - 1].timeForChange / rush); // I already put in rush.

        transform.position = new Vector2(2, ((cRealTime - dOffset) / endTime) * (totalDist * transform.localScale.y)); //Movement

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(1);

        #region Judgement
        // --------------------------------- Everything below is judgement/ input-----------------------------------------------------//
        if (currentBeat < beats.Count)
            if ((beats[currentBeat].beatTiming + (allowanceTime)) / rush <= cRealTime) { //Considered as Late.     
                BeatHandler();
                BeatScore(-1);
            } //when player can start tapping.

        for (var i = 0; i < controls.Length; i++) {
            if (Input.GetKeyDown(controls[i])) {
                CheckNormalBeats(i);
            }
            if (Input.GetKeyUp(controls[i])) {
                holdingDown[i] = 0;
            }
        }

        if (timerForLongBeat < cRealTime) {
            int longBeatsLeft = 0;
            bool hasLongBeat = false;
            timerForLongBeat = cRealTime + (0.05f / rush);

            for (var i = 0; i < beatsActive.Length; i++) {
                if (beatsActive[i] == 1) {
                    hasLongBeat = true;
                    if (holdingDown[i] != 1) {
                        longBeatsLeft++;
                    }
                }
            }
            if (hasLongBeat)
                if (longBeatsLeft == 0)
                    BeatScore(1);
                else
                    BeatScore(-1);
        }
        #endregion

    }

    void ChangeSpeed(float speedToChange, float startTime, float givenTime) {
        if (cRealTime > startTime + givenTime) {
            if (transform.localScale.y != speedToChange) {
                transform.localScale = new Vector3(1,speedToChange);
                Debug.Log(transform.localScale.y);
                float scaleValue = 0;
                scaleValue = iniLength / sprite.bounds.extents.y;

                foreach (Transform child in transform) {
                    if (child != sprite.transform && child.tag != "LongBeat")
                        child.localScale = new Vector2(2, 2 * scaleValue);
                }
            }
        }
        //transform.localScale = new Vector2(1, speedToChange);
        else {
            transform.localScale = new Vector2(1, prevSpeed + ((speedToChange - prevSpeed) * ((cRealTime - startTime) / givenTime)));
            Debug.Log(transform.localScale.y);
            float scaleValue = 0;
            scaleValue = iniLength / sprite.bounds.extents.y;

            foreach (Transform child in transform) {
                if (child != sprite.transform && child.tag != "LongBeat")
                    child.localScale = new Vector2(2, 2 * scaleValue);
            }
        }
    }

    void ChangeBpm(float bpmToChange, float currentBeat) {
        float tempOffset = 0;
        bpmToChange *= rush;

        tempOffset = (cRealTime - dOffset) - ((currentBeat / bpm) * 60);
        dOffset += (cRealTime - dOffset) - (((currentBeat / bpmToChange) * 60) + tempOffset); //Adds the offset value that will offset time to transition bpm.

        endTime = (endBpm / bpmToChange) * 60; //Changes ending time.
        bpm = bpmToChange; //Changes the BPM.
    }

    public void CheckNormalBeats(int toCheck) {
        int numberOfBeatsLeft = 0;
        holdingDown[toCheck] = 1;
        if (currentBeat < beats.Count)
            if (((beats[currentBeat].beatTiming - allowanceTime) / rush <= cRealTime)) {

                if (beats[currentBeat].beats[toCheck]) {
                    beats[currentBeat].beats[toCheck].SetActive(false);
                    beats[currentBeat].beats[toCheck] = null;
                }

                foreach (GameObject beat in beats[currentBeat].beats) {
                    if (beat) {
                        if (beat.name == "1") {
                            numberOfBeatsLeft++;
                        }
                    }
                }

                if (numberOfBeatsLeft == 0) {
                    BeatHandler();
                    BeatScore(1);
                }
            }
    }

    void BeatScore(int givenCombo) {
        grade.Stop();
        grade.Play();

        if (givenCombo > 0) {
            if (combo < 0)
                combo = 0;
            else
                combo++;

            totalPoints += 1000;
            gradeT.text = "PERFECT";

            string tempPoints = totalPoints.ToString();

            if (10 - tempPoints.Length > 0) {
                for (var i = 0; i < 10 - tempPoints.Length; i++) {
                    tempPoints = "0" + tempPoints;
                }
            }

            points.text = tempPoints;
        } else {
            if (combo > 0)
                combo = 0;
            else
                combo--;

            gradeT.text = "MISS";
        }

        comboT.text = Mathf.Abs(combo).ToString();
    }

    void BeatHandler() {
        for (var i = 0; i < beats[currentBeat].beats.Length; i++) {
            if (beats[currentBeat].beats[i] != null) {
                if (beats[currentBeat].beats[i].name == "2") {
                    beatsActive[i] = 1;
                }

                if (beats[currentBeat].beats[i].name == "3") {
                    endLongBeatTime = beats[currentBeat].beatTiming;
                    beatsActive[i] = 0;
                }
            }
        }
        currentBeat++;
    }
}




