using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ui_controller : MonoBehaviour
{
    public TMP_Text score_text_obj;
    public TMP_Text multiplier_text_obj;
    public TMP_Text intensity_text_obj;
    public TMP_Text combo_text_obj;
    private game_controller game_Controller;
    public AudioMixer _MasterMixer;

    private float TimescaleDefault;
    public bool GamePaused = false;

    public GameObject lh_raycast;
    public GameObject rh_raycast;

    public TMP_Text hp1;
    public TMP_Text hp2;
    public TMP_Text hp3;
    public Color color_active;
    public Color color_inactive;

    public GameObject menu_canvas;
    public GameObject gameover_screen;

    public Slider SliderIntensity;

    // Start is called before the first frame update
    void Start()
    {
        game_Controller = GameObject.FindObjectOfType<game_controller>();
        lh_raycast.SetActive(false);
        rh_raycast.SetActive(false);
        menu_canvas.SetActive(false);
        gameover_screen.SetActive(false);
        SliderIntensity.value = game_Controller.intensity_current;
        SliderIntensity.minValue = game_Controller.intensity_min;
        SliderIntensity.maxValue = game_Controller.intensity_max;
        TimescaleDefault = Time.timeScale;
        UpdateUI();
    }

    public void UpdateUI()
    {
        score_text_obj.text = "" + game_Controller.score_current;
        multiplier_text_obj.text = String.Format("x{0}", game_Controller.score_multi_current.ToString("0.00"));
        intensity_text_obj.text = String.Format("i{0}", game_Controller.intensity_current.ToString("0.00"));
        combo_text_obj.text = String.Format("{0}", game_Controller.success_counter.ToString());

        switch (game_Controller.hp_current)
        {
            case 1:
                hp1.color = color_inactive;
                hp2.color = color_inactive;
                hp3.color = color_active;
                break;
            case 2:
                hp1.color = color_inactive;
                hp2.color = color_active;
                hp3.color = color_active;
                break;
            case 3:
                hp1.color = color_active;
                hp2.color = color_active;
                hp3.color = color_active;
                break;
            default:
                hp1.color = color_inactive;
                hp2.color = color_inactive;
                hp3.color = color_inactive;
                break;
        }
    }

    public void ChangeVolumeMusic(float value)
    {
        _MasterMixer.SetFloat("MusicVolume", Mathf.Log10(value)*20);
    }

    public void ChangeVolumeEffects(float value)
    {
        _MasterMixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20);
    }

    public void ChangeVolumeMaster(float value)
    {
        _MasterMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void ChangeIntensity(float value)
    {
        game_Controller.intensity_default = (int)value;
    }

    public void GameOver()
    {
        StartCoroutine(DelayedGameOver());
    }

    public void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0.0f;
        lh_raycast.SetActive(true);
        rh_raycast.SetActive(true);
        menu_canvas.SetActive(true);
    }

    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = TimescaleDefault;
        lh_raycast.SetActive(false);
        rh_raycast.SetActive(false);
        menu_canvas.SetActive(false);
        gameover_screen.SetActive(false);
        UpdateUI();
    }

    IEnumerator DelayedGameOver()
    {
        float t_start = Time.time;
        float t_end = t_start + 1.0f;
        var t_now = t_start;
        while (t_now < t_end)
        {
            yield return null;

            t_now = Time.time;
        }
        gameover_screen.SetActive(true);
        PauseGame();
    }
}
