using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public int area;
    public bool follow_player;

    private void Start() {
        follow_player = false;
        if (gameObject.GetComponent<Rigidbody>()) {
            gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    void Update() {
        if (gameObject.transform.localEulerAngles.x != 0 || gameObject.transform.localEulerAngles.z != 0) {
            gameObject.transform.localEulerAngles = new Vector3(0, gameObject.transform.localEulerAngles.y, 0);
        }
        if (gameObject.transform.position.y != -1f) {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -0.3f, gameObject.transform.position.z);
        }
    }
}
