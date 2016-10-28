using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StepchartMover : MonoBehaviour {

    #region Data Structures
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
    public struct WarpInfo {
        public float beat;
        public float warp;
        public float time;

        public WarpInfo(float givenBeat, float givenWarp, float givenTime) {
            beat = givenBeat;
            warp = givenWarp;
            time = givenTime;
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
    public struct ScrollData {
        public float beat;
        public float scroll;
        public float time;
        public float dist;

        public ScrollData(float givenBeat, float givenScroll, float givenTime, float givenDist) {
            beat = givenBeat;
            scroll = givenScroll;
            time = givenTime;
            dist = givenDist;
        }
    }

    [System.Serializable]
    public struct BeatsInfo {
        public float beatTiming;
        public int[] beats;

        public BeatsInfo(float givenBeatTiming, int[] givenBeats) {
            beatTiming = givenBeatTiming;
            beats = givenBeats;
        }
    }

    [System.Serializable]
    public struct LaneInfo {
        public List<int> beatPositions;
        public int currentBeatInLane;
    }

    #endregion

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
    public List<ScrollData> scrollData;
    public List<BeatsInfo> beats;
    public List<WarpInfo> warps;
    public LaneInfo[] lanesInfo;

    [HideInInspector]
    public float endBpm;
    [HideInInspector]
    public float totalDist;
    [HideInInspector]
    public float beatScale;
    float bpm;
    float cRealTime;
    float endTime;
    float dOffset;
    float timerForLongBeat;

    float prevBeat;
    float prevDist;

    int currentBpm;
    int currentBeat;
    int currentSpeed;
    int currentScroll;

    public float prevSpeed;

    int combo;

    public float iniLength;
    public SpriteRenderer sprite;
    public Transform[] legs;

    public Text points;
    public float totalPoints;
    public PlayerDataCreator playerManager;
    public Transform sequenceZone;

    public void InitialiseStepchart() {
        for (var i = 0; i < legs.Length; i++)
            KinectManager.Instance.legs[i] = legs[i];

        stepchartBuilder.speed = PlayerPref.prefSpeed;
        stepchartBuilder.stepchartMover = this;
        stepchartBuilder.CreateTimingData();
        stepchartBuilder.CreateStepchart(sequenceZone);

        rush = PlayerPref.prefRush;
        currentBeat = 0;
        currentSpeed = 0;
        currentBpm = 0;
        currentScroll = 0;
        prevBeat = 0;
        prevSpeed = 0;

        bpm = bpmData[0].bpm;
        bpm *= rush;
        endTime = (endBpm / bpm) * 60;

        endBpm = scrollData[scrollData.Count - 1].beat;
        totalDist = scrollData[scrollData.Count - 1].dist;
    }

    void Update() {
        cRealTime = playerManager.cRealTime - offset;

        #region Timing Checks
        while (currentBpm < bpmData.Count && bpmData[currentBpm].time / rush < cRealTime) { //Bpm changer
            ChangeBpm(bpmData[currentBpm].bpm, bpmData[currentBpm].beat);
            currentBpm++;
        }

        while (currentSpeed < speedData.Count && speedData[currentSpeed].time / rush < cRealTime) { //Speed changer
            if (currentSpeed - 1 > -1)
                prevSpeed = speedData[currentSpeed - 1].speed;
            currentSpeed++;
        }

        while (currentScroll < scrollData.Count - 1 && scrollData[currentScroll].time / rush < cRealTime) {
            currentScroll++;

            endBpm = scrollData[currentScroll].beat - scrollData[currentScroll - 1].beat;
            endTime = (endBpm / bpm) * 60;
            prevBeat = scrollData[currentScroll - 1].beat;
            prevDist = scrollData[currentScroll - 1].dist;

            totalDist = scrollData[currentScroll].dist - prevDist;
        }
        #endregion

        #region Stepchart Movement
        if (currentSpeed - 1 > 0)
            ChangeSpeed(speedData[currentSpeed - 1].speed, speedData[currentSpeed - 1].time / rush, speedData[currentSpeed - 1].timeForChange / rush);

        transform.position = new Vector2(transform.position.x, (prevDist + (((cRealTime - dOffset - ((prevBeat / bpm) * 60)) / endTime) * (totalDist))) * transform.localScale.y); //Movement
        #endregion

        #region Judgement
        while (currentBeat < beats.Count && (beats[currentBeat].beatTiming + (allowanceTime)) / rush <= cRealTime) { //Considered as Late.     

            float missedBeats = 0;
            for (var i = 0; i < beats[currentBeat].beats.Length; i++) {
                if (beats[currentBeat].beats[i] > 0) {
                    lanesInfo[i].currentBeatInLane++;
                    missedBeats++;
                }
            }

            if (missedBeats > 0) {
                BeatScore(-1);
                PlayerPref.playerScore.miss++;
            }
            currentBeat++;
        }

        if (!(currentBeat < beats.Count))
            if ((beats[beats.Count - 1].beatTiming / rush) + 3 < cRealTime)
                SceneManager.LoadScene(2 + PlayerPref.sceneValueOffset);

        #endregion
    }

    #region Stepchart Effects
    void ChangeSpeed(float speedToChange, float startTime, float givenTime) {
        if (cRealTime > startTime + givenTime) {
            if (transform.localScale.y != speedToChange) {
                transform.localScale = new Vector3(1, speedToChange);
                float scaleValue = 0;
                scaleValue = iniLength / sprite.bounds.extents.y;

                foreach (Transform child in transform) {
                    if (child != sprite.transform && child.tag != "LongBeat")
                        child.localScale = new Vector2(beatScale, beatScale * scaleValue);
                }
            }
        } else {
            transform.localScale = new Vector2(1, prevSpeed + ((speedToChange - prevSpeed) * ((cRealTime - startTime) / givenTime)));
            float scaleValue = 0;
            scaleValue = iniLength / sprite.bounds.extents.y;

            foreach (Transform child in transform) {
                if (child != sprite.transform && child.tag != "LongBeat")
                    child.localScale = new Vector2(beatScale, beatScale * scaleValue);
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
    #endregion

    #region Beat Handler
    public void BeatInput(int inputValue, int beat) {
        if (lanesInfo[beat].currentBeatInLane < lanesInfo[beat].beatPositions.Count)
            if ((beats[lanesInfo[beat].beatPositions[lanesInfo[beat].currentBeatInLane]].beatTiming - allowanceTime) / rush <= cRealTime)
                if (beats[lanesInfo[beat].beatPositions[lanesInfo[beat].currentBeatInLane]].beats[beat] - inputValue <= 0) {
                    beats[lanesInfo[beat].beatPositions[lanesInfo[beat].currentBeatInLane]].beats[beat] = 0;

                    int missedBeats = 0;

                    foreach (int beatValue in beats[lanesInfo[beat].beatPositions[lanesInfo[beat].currentBeatInLane]].beats)
                        missedBeats += beatValue;

                    if (!(missedBeats > 0)) {
                        BeatScore(1);
                        PlayerPref.playerScore.perfect++;
                    }
                    lanesInfo[beat].currentBeatInLane++;
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

            if (combo > PlayerPref.playerScore.maxCombo)
                PlayerPref.playerScore.maxCombo = combo;

            PlayerPref.playerScore.score += 1000;
            gradeT.text = "PERFECT";
        } else {
            if (combo > 0)
                combo = 0;
            else
                combo--;

            gradeT.text = "MISS";
        }



        comboT.text = Mathf.Abs(combo).ToString();
    }
    #endregion

    #region Input handler
    public void ExitLevel() {
        SceneManager.LoadScene("Menu");
    }
    #endregion
}