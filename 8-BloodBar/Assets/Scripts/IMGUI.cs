using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI : MonoBehaviour {
    Rect bloodBar, upButton, downButton;
    float length = 100f;
    float curValue, value;
    void Start() {
        bloodBar = new Rect(0, 0, 60, 20);
        upButton = new Rect(Screen.width / 2 - 50, 50, 40, 20);
        downButton = new Rect(Screen.width / 2, 50, 40, 20);
        curValue = value = 0;
    }

    void OnGUI() {
        Vector2 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        playerPos.y = Screen.height - playerPos.y - length;
        bloodBar.center = playerPos + new Vector2(0, 0);

        if (GUI.Button(upButton, "加血")) {
            curValue += 0.1f;
        }
        if (GUI.Button(downButton, "减血")) {
            curValue -= 0.1f;
        }
        if (curValue > 1f) {
            curValue = 1f;
        }
        if (curValue < 0) {
            curValue = 0;
        }

        value = Mathf.Lerp(value, curValue, 0.05f);

        if (playerPos.x < Screen.width && playerPos.x > 0
            && playerPos.y < Screen.height && playerPos.y > 0) {
            GUI.HorizontalScrollbar(bloodBar, 0.0f, value, 0.0f, 1.0f);
        }
    }
}
