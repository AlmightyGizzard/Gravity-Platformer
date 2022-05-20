using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject destination;
    GameObject player;
    Player pScript;
    GameObject cameraMain;
    public Vector3 cameraDestination;
    public bool right;
    public bool down;
    public bool up;
    public bool stopwatch;
    public int gravityState;
    public bool death;
    public AudioManager audio;



    private void Awake()
    {
        cameraMain = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        pScript = player.GetComponent<Player>();
        audio = FindObjectOfType<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && !(stopwatch))
        {
            int modX = (right) ? 3 : -3;
            int modY = ((up) ? 3 : 0) + ((down) ? -3 : 0);
            Debug.Log("Collision!");
            Vector3 pos = destination.transform.position;
            switch (gravityState)
            {
                case 0:
                    break;
                case 1:
                    Debug.Log("Setting gravity to: Normal");
                    pScript.gravityReversed = false;
                    break;
                case 2:
                    Debug.Log("Setting gravity to: Reversed");
                    pScript.gravityReversed = true;
                    break;
            }
            if (death)
            {
                Debug.Log("You died");
                audio.Play("Death");
            }
            player.transform.position = new Vector3(pos.x+modX, pos.y+modY, pos.z);
            cameraMain.transform.position = cameraDestination;
            
            
        }
        else if (collision.gameObject.name == "Player" && stopwatch)
        {
            if (TimerController.instance.timerGoing)
            {
                TimerController.instance.EndTimer();
            }
            else
            {
                TimerController.instance.BeginTimer();
            }
            
        }
    }
}
