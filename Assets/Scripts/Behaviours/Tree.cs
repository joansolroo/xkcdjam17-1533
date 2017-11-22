using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Antique))]
public class Tree : MonoBehaviour {

    Antique antique;

    public GameObject squirrel;
	// Use this for initialization
	void Start () {
        antique = GetComponent<Antique>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localScale = Vector3.one * Mathf.Min(1, antique.currentAge / 3600 / 24 / 365/3);
        if (antique.currentAge > 5*3600*24*365)
        {
            if (Random.value < Mathf.Min(0.01f,0.0001f* TimeHandler.DeltaTime))
            {
                GameObject.Instantiate(squirrel);
                squirrel.transform.localPosition = this.transform.TransformPoint(0, 2f, 0);
            }
        }
	}
}
