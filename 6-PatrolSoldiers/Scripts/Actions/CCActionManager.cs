using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class CCActionManager : SSActionManager, SSActionCallback {
    public SSActionEventType Complete = SSActionEventType.Completed;
    Dictionary<int, CCMoveAroundAction> actions = new Dictionary<int, CCMoveAroundAction>();

    public void CatchUp(GameObject patrol, GameObject player) {
        if (actions.ContainsKey(patrol.GetComponent<Patrol>().area)) {
            actions[patrol.GetComponent<Patrol>().area].destroy = true;
        }
        CCCatchUpAction action = CCCatchUpAction.getAction(player, 1.5f);
        addAction(patrol.gameObject, action, this);
    }
    public void GoAround(GameObject patrol) {
        CCMoveAroundAction action = CCMoveAroundAction.getAction(1.2f, GetNewPos(patrol));
        actions.Add(patrol.GetComponent<Patrol>().area, action);
        addAction(patrol.gameObject, action, this);
    }

    float[] centerX = { -10, 0, 10};
    float[] centerZ = { 10, 0, -10};
    public Vector3 GetNewPos(GameObject patrol) {
        Vector3 pos = patrol.transform.position;
        int area = patrol.GetComponent<Patrol>().area;
        float stepX = Random.Range(-2f, 2f);
        float stepZ = Random.Range(-2f, 2f);

        if (System.Math.Abs(stepX + pos.x - centerX[area % 3]) > 4) {
            stepX = -stepX;
        }
        if (System.Math.Abs(stepZ + pos.z - centerZ[area / 3]) > 4) {
            stepZ = -stepZ;
        }
        return new Vector3(stepX + pos.x, 0, stepZ + pos.z);
    }

    public void StopAll() {
        foreach(CCMoveAroundAction action in actions.Values) {
            action.destroy = true;
        }
        actions.Clear();
    }

    public void SSActionCallback(SSAction source) {
        if(actions.ContainsKey(source.gameobject.GetComponent<Patrol>().area)) {
            actions.Remove(source.gameobject.GetComponent<Patrol>().area);
        }
        GoAround(source.gameobject);
    }
}
