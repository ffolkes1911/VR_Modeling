using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class game_controller : MonoBehaviour
{
    private ui_controller ui_Controller;
    public float score_multi_min = 1.0f;
    public float score_multi_max = 16.0f;
    public float score_multi_current;
    public float intensity_current;
    public float intensity_default;
    public float intensity_min { get; private set; }
    public float intensity_max { get; private set; }
    public float success_counter { get; private set; }
    public int score_current { get; private set; }
    public int hp_min = 1;
    public int hp_max = 3;
    public int hp_current;

    public float wall_speed { get; private set; }

    public float t_spawn_max = 2.5f;
    public float t_spawn_min = 0.7f;
    private float t_spawn_left;
    public bool spawn_walls = true;

    private List<wall_controller> walls;
    private List<wall_controller> active_walls = new List<wall_controller>();

    public AudioSource effects_source;
    public AudioClip audio_hit_ok;
    public AudioClip audio_hit_fail;

    // Start is called before the first frame update
    void Awake()
    {
        init_params();
        intensity_default = intensity_min;
        intensity_current = intensity_default;

        ui_Controller = GameObject.Find("Canvas").GetComponent<ui_controller>();
        var wall_parent = GameObject.Find("Walls");
        walls = GameObject.FindObjectsOfType<wall_controller>().ToList();
        // duplicate walls 
        foreach (wall_controller wall in walls)
        {
            Instantiate(wall.gameObject, wall_parent.transform);
            Instantiate(wall.gameObject, wall_parent.transform);
            Instantiate(wall.gameObject, wall_parent.transform);
        }
        walls = GameObject.FindObjectsOfType<wall_controller>().ToList();
    }

    void FixedUpdate()
    {
        t_spawn_left -= Time.fixedDeltaTime;
        if (spawn_walls && t_spawn_left < 0.0f)
        {
            var intensity_spawn_mult = 1 - (intensity_min / intensity_max * intensity_current);
            t_spawn_left = (t_spawn_max - t_spawn_min) * intensity_spawn_mult + t_spawn_min; ;

            SpawnWall();
        }
    }

    private void init_params()
    {
        spawn_walls = true;
        score_multi_current = score_multi_min;
        success_counter = 0;
        score_current = 0;
        intensity_min = 1.0f;
        intensity_max = 20.0f;
        intensity_current = intensity_default;
        hp_current = hp_max;
        wall_speed = 5.0f;
        t_spawn_left = t_spawn_max;
    }

    public void StartGame()
    {
        init_params();
        DespawnAllWalls();
        ui_Controller.ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SpawnWall()
    {
        var i = UnityEngine.Random.Range(0, walls.Count);
        var wall = walls.ElementAt(i);
        wall.SpawnWall(intensity_current);
        active_walls.Add(wall);
        walls.Remove(wall);
    }

    public void DespawnWall(wall_controller wall)
    {
        active_walls.Remove(wall);
        walls.Add(wall);
    }

    public void DespawnAllWalls()
    {
        for (int i = active_walls.Count-1; i >= 0; i--)
        {
            //DespawnWall(walls[i]);
            active_walls[i].DespawnWall();
        }
    }

    public void ChangeScore(int score, float multi_increase)
    {
        if (score >= 0)
        {
            score_current += (int)(score_multi_current * score);
            success_counter += 1;
            score_multi_current = Math.Min(score_multi_max, score_multi_current + multi_increase);
            effects_source.PlayOneShot(audio_hit_ok);

            if (success_counter % 4 == 0)
                intensity_current = MathF.Min(intensity_max, intensity_current + 0.25f);
        }
        else
        {
            if (success_counter <= 0)
                intensity_current = MathF.Max(intensity_min, intensity_current - 0.5f);
                SetHPto(hp_current - 1);

            // not a bug, give one chance for player to retain intensity
            success_counter = 0;
            score_multi_current = score_multi_min;
            DespawnAllWalls();
            effects_source.PlayOneShot(audio_hit_fail);
        }
        ui_Controller.UpdateUI();
    }

    public void SetHPto(int hp)
    {
        hp_current = hp;
        ui_Controller.UpdateUI();

        if (hp_current < hp_min)
        {
            spawn_walls = false;
            ui_Controller.GameOver();
        }
    }
}
