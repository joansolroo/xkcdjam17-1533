using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

    public Camera mainCamera;
    public Camera postProcessingCamera;
    public RenderTexture renderTexture;
    public GameObject targetQuad;
	// Use this for initialization
	void Start () {
        renderTexture = new RenderTexture(Screen.currentResolution.width, Screen.currentResolution.height, 16);
        mainCamera.targetTexture = renderTexture;
        targetQuad.GetComponent<Renderer>().material.mainTexture = renderTexture;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
