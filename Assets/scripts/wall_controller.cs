using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class wall_controller : MonoBehaviour
{
    private Transform point_start;
    private Transform point_end;
    private Transform point_hide;
    private Transform point_fade;
    private Vector3 move_direction;
    private Vector3 spawn_direction;

    public float speed = 5;
    private float speed_max = 13f;
    private float speed_cur;
    private bool active = false;
    private bool despawning = false;
    private bool spawning = false;
    public float t_fadein = 3.5f;
    public float t_fadeout = 0.3f;


    private game_controller game_Controller;
    private int score_value = 100;
    private float score_multi_increase = 0.25f;

    private hand_sphere_trigger[] spheres;
    private wall_trigger[] walls;
    private List<int> active_walls = new List<int>();
    private List<int> active_spheres = new List<int>();

    void Start()
    {
        point_start = GameObject.Find("Point Start").transform;
        point_end = GameObject.Find("Point End").transform;
        point_hide = GameObject.Find("Point Hide").transform;
        point_fade = GameObject.Find("Point Fade").transform;

        move_direction = Vector3.Normalize(point_start.position - point_end.position);
        spawn_direction = Vector3.Normalize(point_hide.position - point_start.position);
        game_Controller = GameObject.FindObjectOfType<game_controller>();
        speed = game_Controller.wall_speed;

        walls = GetChildComponentByName<Transform>("Col_negative").GetComponentsInChildren<wall_trigger>();
        spheres = GetChildComponentByName<Transform>("Col_positive").GetComponentsInChildren<hand_sphere_trigger>();

        //for (int i = 0; i < walls.Length; i++)
        //{
        //    active_walls.Add(i);
        //}
        //for (int i = 0; i < spheres.Length; i++)
        //{
        //    active_spheres.Add(i);
        //}
        
        transform.position = point_hide.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            transform.position += speed_cur * Time.deltaTime * -move_direction;
            speed_cur *= 1.02f;
            speed_cur = Mathf.Min(speed_cur, speed_max);
            if (!despawning && transform.position.z < point_fade.position.z)
            {
                DespawnWall();
            }
            if (transform.position.z < point_end.position.z)
            {
                speed_cur = speed;
                active = false;
            }
        }
        else if (spawning)
        {
            transform.position += speed_cur * 2 * Time.deltaTime * -spawn_direction;
            speed_cur *= 1.06f;
            if (transform.position.y < point_end.position.y)
            {
                speed_cur = speed;
                transform.position = point_start.position;
                spawning = false;
                active = true;
            }
        }
    }

    public void SpawnWall(float intensity)
    {
        speed_cur = speed;
        spawning = true;
        speed = game_Controller.wall_speed;
        transform.position = point_hide.position;
        ActivateWalls(intensity);
        ActivateSpheres(intensity);
        EnableColliders(true);
        StartCoroutine(FadeInObject());
    }

    public void ActivateWalls(float intensity)
    {
        float threshold = 20 + 5.0f * intensity;
        for (int i = 0; i < walls.Length; i++)
        {
            var roll = UnityEngine.Random.Range(0.0f, 100.0f);
            if (roll < threshold)
            {
                active_walls.Add(i);
                walls[i].Spawn();
            }
        }
    }

    public void ActivateSpheres(float intensity)
    {
        float threshold = 50 + 5.0f * intensity;
        Type[] box_types = { Type.LeftHand, Type.RightHand };
        foreach (var box_type in box_types)
        {
            while (true)
            {
                var i_rnd = UnityEngine.Random.Range(0, spheres.Length);

                if (spheres[i_rnd].box_type != box_type)
                    continue;

                var roll = UnityEngine.Random.Range(0.0f, 100.0f);
                if (roll >= threshold)
                    break;

                active_spheres.Add(i_rnd);
                spheres[i_rnd].Spawn();
                break;
            }
        }
    }

    public void DespawnWall()
    {
        if (!despawning)
        {
            despawning = true;
            EnableColliders(false);
            StartCoroutine(FadeOutObject());
        }
        else
        {
            Debug.LogWarning("Wall despawn collision");
        }
    }

    public void ResetWall()
    {
        spawning = false;
        active = false;
        despawning = false;
        transform.position = point_hide.position;
        foreach (var i in active_walls)
        {
            var box = walls[i];
            box.Despawn();
        }
        foreach (var i in active_spheres)
        {
            var box = spheres[i];
            box.Despawn();
        }

        active_walls = new List<int>();
        active_spheres = new List<int>();
    }

    public void EnableColliders(bool enable)
    {
        foreach (var i in active_walls)
        {
            var box = walls[i];
            box.col.enabled = enable;
        }
        foreach (var i in active_spheres)
        {
            var box = spheres[i];
            box.col.enabled = enable;
        }

    }
    //##############################
    public void hit_fail()
    {
        game_Controller.ChangeScore(-score_value, 0);
        //DespawnWall();

        Debug.Log("HIT FAIL");
    }
    public void hit_success()
    {
        game_Controller.ChangeScore(score_value, score_multi_increase);
        Debug.Log("HIT SUCCESS");
    }
    public void hit_neutral()
    {
        game_Controller.ChangeScore(0, 0);
        Debug.Log("HIT NEUTRAL");
    }
//###########################
    private T GetChildComponentByName<T>(string name) where T : Component
    {
        foreach (T component in GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }

    private void SetWallTransparency(float mult)
    {
        foreach (var i in active_walls)
        {
            var wall = walls[i];
            wall.rend.material.color =
                new Color(wall.default_color.r, wall.default_color.g, wall.default_color.b, wall.default_color.a * mult);
        }
        foreach (var i in active_spheres)
        {
            var sphere = spheres[i];
            sphere.rend.material.color =
                new Color(sphere.default_color.r, sphere.default_color.g, sphere.default_color.b, sphere.default_color.a * mult);
        }
    }


    IEnumerator FadeInObject()
    {
        float t_start = Time.time;
        float t_end = t_start + t_fadein;
        var t_now = t_start;
        while (!despawning && t_now < t_end)
        {
            var mult = (t_now - t_start) / t_fadein;
            SetWallTransparency((float)Math.Pow((double)mult, 6));
            yield return null;

            t_now = Time.time;
        }
        if (!despawning)
            SetWallTransparency(1.0f);
    }
    IEnumerator FadeOutObject()
    {
        float t_start = Time.time;
        float t_end = t_start + t_fadeout;
        var t_now = t_start;
        // loop over 1 second backwards
        while (t_now < t_end)
        {
            var mult = (t_now - t_start) / t_fadeout;
            SetWallTransparency(1.0f - mult);
            yield return null;

            t_now = Time.time;
        }
        SetWallTransparency(0.0f);
        game_Controller.DespawnWall(this);
        ResetWall();
    }

}
