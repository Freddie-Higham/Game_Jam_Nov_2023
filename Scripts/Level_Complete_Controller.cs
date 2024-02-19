using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Level_Complete_Controller : MonoBehaviour
{

    public TextMeshProUGUI TIME_TEXT;
    public TextMeshProUGUI GOLD_TEXT;
    public GameObject MEDAL;
    public Sprite gold_sprite;
    public Sprite silver_sprite;
    public Sprite bronze_sprite;
    private float time_limit;
    private float time_taken;
    private int gold;
    private bool high_score_scored;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void LevelCompleteInfo(float timer, int level_number) {

        Cursor.visible = true;

        if (level_number == 1) {
            time_limit = Level_1.time_limit;
            time_taken = time_limit - timer;
            Level_1.complete(time_taken);
            gold = Level_1.gold;
            high_score_scored = Level_1.high_score_scored;
        }
        else if (level_number == 2) {
            time_limit = Level_2.time_limit;
            time_taken = time_limit - timer;
            Level_2.complete(time_taken);
            gold = Level_2.gold;
            high_score_scored = Level_2.high_score_scored;
        }
        else if (level_number == 3) {
            time_limit = Level_3.time_limit;
            time_taken = time_limit - timer;
            Level_3.complete(time_taken);
            gold = Level_3.gold;
            high_score_scored = Level_3.high_score_scored;
        }

        Gold.gold += gold;

        //Debug.Log($"gold: {Level_1.gold}, medal_level: {Level_1.medal_level}, time_taken: {time_taken}");

        GOLD_TEXT.text = $"+{gold} gold";

        if (high_score_scored){
            TIME_TEXT.text =$"Time: {Math.Floor(time_taken)}s (high score!)";
        }
        else {
            TIME_TEXT.text =$"Time: {Math.Floor(time_taken)}s";
        }


        if (time_limit * 0.6 > time_taken) {
            MEDAL.GetComponent<SpriteRenderer>().sprite = gold_sprite;
        }
        else if (time_limit * 0.75 > time_taken) {
            MEDAL.GetComponent<SpriteRenderer>().sprite = silver_sprite;
        }
        else{
            MEDAL.GetComponent<SpriteRenderer>().sprite = bronze_sprite;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
