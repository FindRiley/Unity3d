using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMoveToAction : SSAction {
    public float speedx, speedy, speedz;
    public float gravity = -10.0f;

    private CCMoveToAction() {}
    public static CCMoveToAction GetAction(Vector3 speed) {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.speedx = speed.x;
        action.speedy = speed.y;
        action.speedz = speed.z;
        return action;
    }

    public override void Update() {
        this.transform.position += new Vector3(speedx, speedy -0.5f * gravity * Time.deltaTime, speedz) * Time.deltaTime;
        speedy += gravity * Time.deltaTime;
        if (transform.position.y < -20) {
            destroy = true;
            callback.SSActionCallback(this);
        }
    }
    public override void Start() {}
}
