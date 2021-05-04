using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text yourScoreTxt;
    public static ScoreManager SCORE;
    private int score;
    public Text scoreTxt;
    
    void Start()
    {
        SCORE = GetComponent<ScoreManager>();
    }

    public int Score {
        get {
            return score;
        }

        set {
            score = value;
            scoreTxt.text = score.ToString();
        }
    }
    
}
