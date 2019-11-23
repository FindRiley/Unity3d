using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    private float speed = 5f;
    private int direction = 0;
    private bool ifRun = false;
    public Animator animator;
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("IsRun", true);
    }

    // Update is called once per frame
    void Update() {
        direction = getAngle();
        transform.eulerAngles = new Vector3(0, direction, 0);
        if (ifRun) {
            direction = getAngle();
            transform.Translate(0, 0, speed * Time.deltaTime);
            animator.SetBool("IsRun", true);
        }
        else {
            animator.SetBool("IsRun", false);
        }
    }

    int getAngle () {
        ifRun = true;
        if (Input.GetKey(KeyCode.A))
            return (-90);
        else if (Input.GetKey(KeyCode.D))
            return 90;
        else if (Input.GetKey(KeyCode.W))
            return 0;
        else if (Input.GetKey(KeyCode.S))
            return 180;
        else {
            ifRun = false;
            return direction;
        }
    }
}
