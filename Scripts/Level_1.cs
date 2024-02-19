using System.Collections;
using System.Collections.Generic;

public static class Level_1 {

    public static float high_score = 1000;
    public static int medal_level = 0;
    public static float time_limit = 70f;
    public static int gold = 0;
    public static bool high_score_scored = false;

    public static int complete(float time_taken) {

        gold = 0;

        if (time_taken < high_score) {
            high_score = time_taken;
            high_score_scored = true;
        }
        else {
            high_score_scored = false;
        }

        if (time_limit * 0.6 > time_taken) {
            if (medal_level == 0){
                gold = 3;
            }
            else if (medal_level == 1) {
                gold = 2;
            }
            else if (medal_level == 2) {
                gold = 1;
            }
            medal_level = 3;
        }
        else if (time_limit * 0.75 > time_taken && medal_level <= 2) {
            if (medal_level == 0){
                gold = 2;
            }
            else if (medal_level == 1) {
                gold = 1;
            }
            medal_level = 2;
        }
        else if (medal_level == 0){
            gold = 1;
            medal_level = 1;
        }

        return(gold); 
    }

}