using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDown : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject effect;

    void Start() {
        effect.SetActive(false);
    }

    // Update is called once per frame
    public void OnMouseDown() {
        if (effect.active) {
            effect.SetActive(false);
        }
        else
            effect.SetActive(true);
    }

    void OnGUI() {
        GUI.Label(new Rect(Screen.width / 2 - 50, 30, 160, 50), "Click the Magic Ball");
    }
}
