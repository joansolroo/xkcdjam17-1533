using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Reloadscene : MonoBehaviour 

{

void Update()
{
	ReloadLevel ();
}

void ReloadLevel ()

{
	if(Input.anyKeyDown)
	{
        SceneManager.LoadScene("scena 1 v3");

    }
}

 
}