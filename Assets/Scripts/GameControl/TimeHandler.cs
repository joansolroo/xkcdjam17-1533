using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class TimeHandler : MonoBehaviour
{
    /*public class ExtendedTime
    {
        long year = 0;
        public System.DateTime time;
    }*/
    long year = 0;
    public System.DateTime time;

    public Text timeText;

    bool paused = false;
    float speed = 1;

    public static TimeHandler instance;
    public AudioSource music;

    public Button BTNPause;
    public Button BTNPlay;
    public Button BTNFF;
    // Use this for initialization
    void Start()
    {
        time = System.DateTime.Now;

        instance = this;
        if (speed < 3600 * 24 && (hour > 19 || hour < 7))
        {
            AmbientLight.intensity = 1;
        }
        else
        {
            AmbientLight.intensity = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Pause();
        }
        if (!paused)
        {
            timeText.text = (time.Year + year).ToString("0000") + "/" + time.Month.ToString("00") + "/" + time.Day.ToString("00")
                + " " + time.Hour.ToString("00") + ":" + time.Minute.ToString("00") + ":" + time.Second.ToString("00");

            if (time.Year > 9000)
            {
                time.AddYears(-9000);
                year += 9000;
            }
            time = time.AddSeconds(DeltaTime);

            music.pitch = Mathf.MoveTowards(music.pitch, 0.25f * Mathf.Max(1.5f, Mathf.Pow(speed, 0.1f)), 1f * Time.deltaTime);
        }
        else
        {
            music.pitch = Mathf.MoveTowards(music.pitch, 0.2f, 1f * Time.deltaTime);
        }

        if (speed < 3600 * 24)
        {
            if (hour > 19 || hour < 7)

            {
                AmbientLight.intensity = Mathf.MoveTowards(AmbientLight.intensity, 1, DeltaTime / 3600);
            }

            else
            {
                AmbientLight.intensity = Mathf.MoveTowards(AmbientLight.intensity, 0, DeltaTime / 3600);
            }
        }
        else
        {
            AmbientLight.intensity = Mathf.MoveTowards(AmbientLight.intensity, 1f/speed, DeltaTime / 3600);
        }
        if (!Antiquarian.instance.alive)
        {
            AmbientLight.intensity = Mathf.MoveTowards(AmbientLight.intensity, 0.25f, Time.deltaTime / 4);
        }
    }

    public static float DeltaTime
    {
        get
        {
            return instance.paused ? 0 : Time.deltaTime * instance.speed;
        }
    }

    public static int hour
    {
        get
        {
            return instance.time.Hour;
        }
    }

    public void End()
    {
        paused = true;
    }
    public void Pause()
    {
        paused = !paused;
    }

    public void Play()
    {
        if (Antiquarian.instance.alive)
        {
            paused = false;
            speed = 1;
        }
    }

    public float MaxSpeedDays = 365;
    public void Faster()
    {
        if (Antiquarian.instance.alive)
        {
            speed *= 2;
            speed = Mathf.Min(speed, 3600 * 24 * MaxSpeedDays);
        }
    }

}
