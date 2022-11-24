using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_controller : MonoBehaviour
{
    public Text score_text_obj;
    public Text multiplier_text_obj;
    public float score_multi_min = 1.0f;
    public float score_multi_max = 16.0f;
    public float score_multi_current;
    private float success_counter = 0;
    private int score_current;

    // Start is called before the first frame update
    void Start()
    {
        score_multi_current = score_multi_min;
        UpdateUI();
    }

    private void UpdateUI()
    {
        score_text_obj.text = "SCORE: " + score_current;
        multiplier_text_obj.text = String.Format("MULTI: {0}x", score_multi_current.ToString("0.00"));
    }

    public void ChangeScore(int score, float multi_increase)
    {
        if (score >= 0) {
            score_current += (int)(score_multi_current * score);
            success_counter += 1;
            score_multi_current = Math.Min(score_multi_max, score_multi_current + multi_increase);
        }
        else
        {
            success_counter = 0;
            score_multi_current = score_multi_min;
        }
        UpdateUI();
    }
}
