using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    readonly float speed = 20;

    Vector3 target, middle;
    int moveState = 0; // 0->no need to move, 1->object moving , 2->boat moving to dest

    void Update() {
        if (moveState == 1) {
            transform.position = Vector3.MoveTowards(transform.position, middle, speed * Time.deltaTime);
            if (transform.position == middle) {
                moveState = 2;
            }
        } else if (moveState == 2) {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position == target) {
                moveState = 0;
            }
        }
    }

    public void SetDestination (Vector3 dest) {
        if (moveState != 0) return;
        target = dest;
        middle = dest;
        if (dest.y == transform.position.y) { //move boat
            moveState = 2;
        } else {
            moveState = 1;
            if (transform.position.y < target.y) {  // move character from coast to boat
                middle.x = transform.position.x;
            } else if (transform.position.y > target.y) { // move character from boat to coast
                middle.y = transform.position.y;
            }
        }
    }

    public void Reset() {
        moveState = 0;
    }
}
