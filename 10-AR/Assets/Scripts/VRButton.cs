using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

[System.Obsolete]
public class VRButton : MonoBehaviour, IVirtualButtonEventHandler {
    
    public GameObject ani;
    VirtualButtonBehaviour[] vbbs;

    void Start() {
        vbbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbbs.Length; i++) {
            vbbs[i].RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour myButton) {
        switch (myButton.VirtualButtonName) {
            case "walk":
                ani.GetComponent<Animator>().SetBool("jump", false);
                ani.GetComponent<Animator>().SetBool("eat", false);
                ani.GetComponent<Animator>().SetBool("walk", true);
                Debug.Log("walk");
                break;
            case "jump":
                ani.GetComponent<Animator>().SetBool("walk", false);
                ani.GetComponent<Animator>().SetBool("eat", false);
                ani.GetComponent<Animator>().SetBool("jump", true);
                Debug.Log("jump");
                break;
            case "eat":
                ani.GetComponent<Animator>().SetBool("jump", false);
                ani.GetComponent<Animator>().SetBool("walk", false);
                ani.GetComponent<Animator>().SetBool("eat", true);
                Debug.Log("eat");
                break;
            case "idle":
                ani.GetComponent<Animator>().SetBool("walk", false);
                ani.GetComponent<Animator>().SetBool("jump", false);
                ani.GetComponent<Animator>().SetBool("eat", false);
                Debug.Log("idle");
                break;
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour myButton) {
        
    }
    

    void Update() {

    }
}