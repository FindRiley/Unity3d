using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveAroundAction : SSAction {
    Vector3 target;
    float speed;

    private CCMoveAroundAction() { }

    public static CCMoveAroundAction getAction(float speed_, Vector3 target_) {
        CCMoveAroundAction action = ScriptableObject.CreateInstance<CCMoveAroundAction>();
        action.target = target_;
        action.speed = speed_;
        return action;
    }

    public override void Update() {
        if (this.transform.position == target) {
            destroy = true;
            callBack.SSActionCallback(this);
        }
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
     }

    public override void Start() {
        Quaternion rotation = Quaternion.LookRotation(target - transform.position, Vector3.up);
        transform.rotation = rotation;
    }
}
