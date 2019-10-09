using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class InteractGUI : MonoBehaviour
{
    private UserAction userAction;
    float startTime, leftTime;
    bool start;
    GUIStyle style;
    GUIStyle buttonStyle;

    void Start() {
        userAction = SSDirector.getInstance().currentSceneController as UserAction;
        style = new GUIStyle();
        style.fontSize = 25;
        
        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
        start = false;
        startTime = 0;
        leftTime = 60;
    }

    void OnGUI() {
        if (!start || (leftTime - (int)(Time.time - startTime)) < 0) {
            userAction.stopGame();
            leftTime = 60;
            start = false;
            GUI.Label(new Rect(10, Screen.height - 100, 200, 120), "Score: " + userAction.getScore().ToString() + "\nTime:  60" + "\nRound:  " + userAction.getRound(), style);
            if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 30, 80, 60), "Start", buttonStyle)) {
                start = true;
                userAction.Restart();
                startTime = Time.time;
            }
        }
        if (start) {
            GUI.Label(new Rect(10, Screen.height - 100, 200, 120), "Score: " + userAction.getScore().ToString() + "\nTime:  " + (leftTime - (int)(Time.time - startTime)).ToString() + "\nRound:  " + userAction.getRound(), style);
        }
    }
}