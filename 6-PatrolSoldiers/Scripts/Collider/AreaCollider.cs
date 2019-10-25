using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    public int sign = 0;
    FirstSceneController sceneController;
    void Start() {
        sceneController = SSDirector.getInstance().currentSceneController as FirstSceneController;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            sceneController.SetPlayerArea(sign);
            GameEventManager.Instance.PlayerEscape();
        }
    }

    void  OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player")
            GameEventManager.Instance.PlayerEscape();
    }
}
