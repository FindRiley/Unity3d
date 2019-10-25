using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCCatchUpAction : SSAction {
    GameObject target;
    float speed;

    private CCCatchUpAction() { }
    public static CCCatchUpAction getAction(GameObject target, float speed) {
        CCCatchUpAction action = ScriptableObject.CreateInstance<CCCatchUpAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update() {
        if (gameobject.GetComponent<Patrol>().follow_player == false || transform.position == target.transform.position) {
            destroy = true;
            callBack.SSActionCallback(this);
        }
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - gameobject.transform.position, Vector3.up);
        gameobject.transform.rotation = rotation;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    public override void Start() {
        Quaternion rotation = Quaternion.LookRotation(target.transform.position - gameobject.transform.position, Vector3.up);
        gameobject.transform.rotation = rotation;
    }
}
