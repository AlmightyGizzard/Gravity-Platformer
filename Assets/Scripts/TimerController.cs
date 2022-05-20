using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{

    public static TimerController instance;
    private TimeSpan timeplaying;
    public bool timerGoing;

    public TextMeshProUGUI timerText;

    private float elapsedTime;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timerText.text = "Time: 00:00:00";
        timerGoing = false;
    }

    public void BeginTimer()
    {
        timerGoing = true;
        elapsedTime = 0f;
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timeplaying = TimeSpan.FromSeconds(elapsedTime);
            string tps = "Time: " + timeplaying.ToString("mm':'s'.'ff");
            timerText.text = tps;

            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
