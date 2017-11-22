using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Antiquarian : MonoBehaviour {

    public float age = 30;
    public float beardLength = 0;

    public Sprite sprite_awake;
    public Sprite sprite_sleeping;
    SpriteRenderer renderer;
    public Text sleepZZZ;

    public float hideTimer = 0;
    public bool here = true;
    public bool awake = true;

    public bool alive = true;
    public static Antiquarian instance;
    // Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (alive)
        {
            beardLength += 0.000001f * TimeHandler.DeltaTime;
            age += TimeHandler.DeltaTime / 3600 / 365 / 24;
        }
        if (hideTimer > 0)
        {
            hideTimer -= TimeHandler.DeltaTime;
        }
        here = hideTimer <= 0;
        renderer.enabled = here;
        awake = TimeHandler.hour <= 23 && TimeHandler.hour > 7;
        if (!awake && here)
        {
            renderer.sprite = sprite_sleeping;
            sleepZZZ.gameObject.SetActive(true);  
        }
        else
        {
            renderer.sprite = sprite_awake;
            sleepZZZ.gameObject.SetActive(false);
        }

        if(age > 70)
        {
            if (Random.value < (0.001f* TimeHandler.DeltaTime / 3600 / 24))
            {
                alive = false;
            }
        }
        
        else if (age > 40)
        {
            if (Random.value < (0.00001f * TimeHandler.DeltaTime/3600/24))
            {
                alive = false;
            }
        }
        if (!alive)
        {
            TimeHandler.instance.End();
            Debug.Log("You died at " + age);
            renderer.sprite = sprite_sleeping;
            sleepZZZ.gameObject.SetActive(false);
        }
	}
    public static void GoAway(float minutes)
    {
        instance.hideTimer = minutes * 60;
        instance.renderer.enabled = false;
    }
    public static void ComeBack()
    {
        instance.hideTimer = 0;
        instance.renderer.enabled = true;
    }
    bool overit = false;
    float timer = 0;
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        //  Debug.Log("Mouse is over " + gameObject.name);
        overit = true;
        timer += Time.deltaTime;
        if (timer > 1)
        {
            if (alive)
            {
                Alttext.Show((int)Input.mousePosition.x, (int)Input.mousePosition.y, "Me, age: " + age.ToString("0.0") + "\nbeard length: " + beardLength.ToString("0.0") + "mm");
            }
            else
            {
                Alttext.Show((int)Input.mousePosition.x, (int)Input.mousePosition.y, "Me, age: " + age.ToString("0.0") + "\nAnd I think i am dead.");
            }
        }
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        // Debug.Log("Mouse is no longer on " + gameObject.name);
        overit = false;
        timer = 0;
        Alttext.Hide();
    }

    public bool CanWork()
    {
        return here && awake;
    }
}
