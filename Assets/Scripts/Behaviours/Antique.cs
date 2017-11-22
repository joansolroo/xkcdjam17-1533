using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antique : MonoBehaviour
{

    public enum AntiqueType { Classic, Biodegradable, Underapreciated, Alive/*, Wave, Retro*/};

    //This is what you need to show in the inspector.
    public AntiqueType type;

    public float initialValue;
    public float minValue;
    public float resellValue;
    public float intensity;
    // Use this for initialization
    void Start()
    {
        resellValue = initialValue;
    }
     
    public float currentAge;
    public float maxAge;
    
    // Update is called once per frame
    void Update()
    {
        currentAge += TimeHandler.DeltaTime;
        if (type == AntiqueType.Classic)
        {
            resellValue = Mathf.Max(minValue, resellValue + Random.Range(-0.001f, 0.005f) * initialValue * TimeHandler.DeltaTime / 365 * intensity);
        }
        else if (type == AntiqueType.Alive)
        {
            if (maxAge > currentAge)
            {
                resellValue = Mathf.Max(minValue, resellValue + Random.Range(-0.001f, 0.005f) * initialValue * TimeHandler.DeltaTime/365*intensity);
            }
            else
            {
                resellValue = minValue;
            }
        }
        else if (type == AntiqueType.Biodegradable)
        {
            resellValue = Mathf.Max(minValue, resellValue - Random.Range(0.01f, 0.05f) * initialValue * TimeHandler.DeltaTime);
        }
        else if (type == AntiqueType.Underapreciated)
        {
            resellValue = Mathf.Max(minValue, resellValue + Random.Range(-0.1f, 1.15f) * initialValue * TimeHandler.DeltaTime);
        }
        
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
            Alttext.Show((int)Input.mousePosition.x, (int)Input.mousePosition.y, ""+gameObject.name+ ":"+ resellValue.ToString("$0.00"));
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
}
