using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            this.gameObject.GetComponent<Patrol>().follow_player = false;
            GameEventManager.Instance.GameOver();
        }
    }
}
