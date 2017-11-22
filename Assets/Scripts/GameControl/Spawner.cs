using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {


    public static Spawner instance;
    public GameObject[] options;

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Spawn(float delaySeconds)
    {
        instance.StartCoroutine(instance.SpawnDelayed(delaySeconds));
        
    }

    IEnumerator SpawnDelayed(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        GameObject obj = GameObject.Instantiate(instance.options[Random.Range(0, instance.options.Length)]);
        obj.transform.parent = instance.transform;
        obj.transform.localPosition = Vector3.zero;
    }
}
