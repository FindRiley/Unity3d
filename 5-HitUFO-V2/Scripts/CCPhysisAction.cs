using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPhysisAction : SSAction {
    public float speedx, speedy, speedz;
    public float gravity = 0.0f;

    private CCPhysisAction() { }
    public static CCPhysisAction GetAction(Vector3 speed) {
        CCPhysisAction action = ScriptableObject.CreateInstance<CCPhysisAction>();
        action.speedx = speed.x;
        action.speedy = speed.y;
        action.speedz = speed.z;
        return action;
    }

    public override void Update() {}

    public override void FixedUpdate() {

        if (transform.position.y < -20) {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
            destroy = true;
            callback.SSActionCallback(this);
        }
    }
    public override void Start() {
        if(!this.gameObject.GetComponent<Rigidbody>()) {
            this.gameObject.AddComponent<Rigidbody>();
        }
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(speedx, speedy, speedz), ForceMode.VelocityChange);
    }
}
