using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class Alttext : MonoBehaviour
{

    bool overit = false;
    float timer = 0;
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over "+gameObject.name);
        overit = true;
        timer += Time.deltaTime;
        if(timer > 1)
        {
            Alttext.Show((int)Input.mousePosition.x, (int)Input.mousePosition.y, "HI ");
        }
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on " + gameObject.name);
        overit = false;
        timer = 0;
        Alttext.Hide();
    }
    public bool handler = false;
    static Text textbox;
    private void Start()
    {
        if (handler)
        {
            textbox = GetComponent<Text>();
            gameObject.SetActive(false);
        }
    }
    
    public static void Show(int x, int y, string text)
    {
        textbox.text = text;
        textbox.gameObject.SetActive(true);
        textbox.transform.position = new Vector3(x+20, y+40, +5);
    }
    public static void Hide()
    {
        textbox.gameObject.SetActive(false);
    }
}
