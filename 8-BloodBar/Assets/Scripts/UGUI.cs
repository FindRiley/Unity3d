using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UGUI : MonoBehaviour
{
    float dis = 3f;
    public Slider bloodBar;

    // Update is called once per frame
    void Update() {
        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + dis, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        bloodBar.transform.position = screenPos;
    }

    void OnGUI() {
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 30, 40, 20), "加血")) {
            bloodBar.value += 2f;
        }
        else if (GUI.Button(new Rect(Screen.width / 2, 30, 40, 20), "减血")) {
            bloodBar.value -= 2f;
        }
    }
}
