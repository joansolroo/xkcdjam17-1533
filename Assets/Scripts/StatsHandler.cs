using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class StatsHandler : MonoBehaviour {

    public GameObject ChartSystem;
    public static StatsHandler instance;

    public PieRenderer pie;
    public static int TIME_SLEEP = 0;
    public static int TIME_WORK = 1;
    public static int TIME_FAMILY = 2;
    public float[] timeDistribution = new float[3];
	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        pie.sectionsQuantities[1] = (timeDistribution[1]+ timeDistribution[2])/3;
        pie.sectionsQuantities[2] = timeDistribution[1];
        pie.sectionsQuantities[3] = timeDistribution[2];
    }
    public void ToggleCharts()
    {
        ChartSystem.SetActive(!ChartSystem.activeSelf);
    }

    
}
