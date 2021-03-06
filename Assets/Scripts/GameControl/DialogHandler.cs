﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class DialogHandler : MonoBehaviour
{

    public GameObject dialogOptions;
    public GameObject dialogYes;
    public GameObject dialogNo;

    public GameObject dialogDoorbell;
    float eventTimer = 0;

    public GameObject dialogFood;
    public GameObject dialogSleep;
    bool Qsleep = false;
    public GameObject dialogBeard;
    bool Qshave = false;

    public GameObject dialogPlay;
    bool Qplay = false;
    public float love = 100;
    bool family = true;

    public Toggle TGLNotify;
    enum EventType { NOTHING, BED, FOOD, PLAY, SHAVE }
    EventType currentEvent = EventType.NOTHING;
    // Use this for initialization
    void Start()
    {
        dialogDoorbell.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (TimeHandler.hour == 1)
        {
            Qsleep = false;
            Qplay = false;
        }
        if (TimeHandler.hour >= 9 && TimeHandler.hour <= 11)
        {
            // PERHAPS FEDEX WILL BRING A PACKAGE
            if (!dialogDoorbell.activeSelf && Random.value < 0.05 && eventTimer <= 0)
            {
                dialogDoorbell.SetActive(true);
                eventTimer = 60 * 60;
                NotifyStartEvent();
            }
        }
        if (!Qsleep && love > 0 && family && TimeHandler.hour >= 22 && TimeHandler.hour <= 23)
        {
            //GO TO BED
            eventTimer = 60 * 60;
            currentEvent = EventType.BED;
            ShowOptions();
            dialogSleep.SetActive(true);
            Qsleep = true;
            NotifyStartEvent();
        }
        if (!Qplay && love > 0 && family && TimeHandler.hour >= 18 && TimeHandler.hour <= 20 && Random.value <0.02f)
        {
            //GO TO BED
            eventTimer = 60 * 60;
            currentEvent = EventType.PLAY;
            ShowOptions();
            dialogPlay.SetActive(true);
            Qplay = true;
            NotifyStartEvent();
        }

        // YOU SHOULD SHAVE
        if (love > 0 && Antiquarian.instance.beardLength > 100 && eventTimer <= 0 && Random.value < 0.01f)
        {
            eventTimer = 60 * 60;
            currentEvent = EventType.SHAVE;
            dialogBeard.SetActive(true);
            Qshave = true;
            NotifyStartEvent();
        }

        if (eventTimer <= 0)
        {
            bool failed = false;
            if (dialogDoorbell.activeSelf)
            {
                dialogDoorbell.SetActive(false);
                failed = true;
            }
            if (dialogSleep.activeSelf)
            {
                dialogSleep.SetActive(false);
                failed = true;
            }
            if (dialogBeard.activeSelf)
            {
                dialogBeard.SetActive(false);
                failed = true;
            }
            if (dialogPlay.activeSelf)
            {
                dialogPlay.SetActive(false);
                failed = true;
            }
            HideOptions();
            if (failed)
                love -= 5;
        }
        else
        {
            eventTimer -= TimeHandler.DeltaTime;
        }
    }

    public void AntiquarianSaysYes()
    {
        Debug.Log("YES!");
        StartCoroutine(SelectAnswer(true));
        //HideOptions();
    }

    public void AntiquiarianSaysNo()
    {
        Debug.Log("No...");
        StartCoroutine(SelectAnswer(false));

        // HideOptions();
    }
    public void AntiquiarianPicksPackage()
    {
        Debug.Log("Package!");
        dialogDoorbell.SetActive(false);
        float delay = Random.Range(0, 2);
        Antiquarian.GoAway(delay);
        Spawner.Spawn(0);
        NotifyEndEvent();

    }
    void NotifyStartEvent()
    {
        if (TGLNotify.isOn)
        {
            TimeHandler.instance.pause();
        }
    }
    void NotifyEndEvent()
    {
        if (TGLNotify.isOn)
        {
            TimeHandler.instance.pause(false);
        }
    }
    IEnumerator SelectAnswer(bool yes)
    {
        if (yes)
        {

            dialogNo.SetActive(false);
            yield return new WaitForSeconds(2);
            if (currentEvent == EventType.BED) { Antiquarian.GoAway(10 * 60); }
            if (currentEvent == EventType.FOOD) { Antiquarian.GoAway(60); }
            if (currentEvent == EventType.PLAY) { Antiquarian.GoAway(60); }
            if (currentEvent == EventType.SHAVE)
            {
                Antiquarian.GoAway(30);
                Antiquarian.instance.beardLength = 0;
                Qshave = false;
            }
            dialogYes.SetActive(false);
            love += 2;
        }
        else
        {
            dialogYes.SetActive(false);
            yield return new WaitForSeconds(2);
            dialogNo.SetActive(false);
            love -= 5;
        }
        if (dialogSleep.activeSelf) dialogSleep.SetActive(false);
        if (dialogBeard.activeSelf) dialogBeard.SetActive(false);
        if (dialogBeard.activeSelf) dialogBeard.SetActive(false);
        
        if (dialogPlay.activeSelf) dialogPlay.SetActive(false);

        NotifyEndEvent();
    }

    void ShowOptions()
    {
        dialogOptions.SetActive(true);
        dialogNo.SetActive(true);
        dialogYes.SetActive(true);
    }
    void HideOptions()
    {
        dialogOptions.SetActive(false);
    }
}
