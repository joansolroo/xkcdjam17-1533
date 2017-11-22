using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour {

    public static Wallet instance;

    public float money;
    public UnityEngine.UI.Text moneyText;
    // Use this for initialization
    void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        moneyText.text = money.ToString("$0.00");
	}
}
