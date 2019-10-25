using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.UI;

public class InterfaceGUI : MonoBehaviour {
    UserAction userAction;
    ISceneController SceneController;
    public GameObject t;
    bool start = false;
    float startTime;
    float lastTime;

    void Start() {
        lastTime = 0;
        userAction = SSDirector.getInstance().currentSceneController as UserAction;
        SceneController = SSDirector.getInstance().currentSceneController as ISceneController;
    }

    private void OnGUI() {
        if (start) {
            lastTime = ((int)(Time.time - startTime));
            GUI.Label(new Rect(50, 30, 200, 30), "Score: " + (userAction.GetScore() / 4).ToString() + "  Time: " + lastTime.ToString());
            if (!userAction.GetGameState()) {
                start = false;
            }
        }
        else {
            GUI.Label(new Rect(50, 30, 200, 30), "Score: " + (userAction.GetScore() / 4).ToString() + "  Time: " + lastTime.ToString());
            if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 100, 50), "Start")) {
                start = true;
                userAction.Restart();
                startTime = Time.time;
            }
        }
    }

    private void Update() {
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");
        userAction.MovePlayer(new Vector3(X, 0, Z));
    }
}
