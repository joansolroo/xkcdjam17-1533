using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class StatsHandler : MonoBehaviour {

    public GameObject ChartSystem;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ToggleCharts()
    {
        ChartSystem.SetActive(!ChartSystem.activeSelf);
    }
}
