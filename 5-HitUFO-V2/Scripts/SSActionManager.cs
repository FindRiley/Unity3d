using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class SSActionManager : MonoBehaviour {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();   // actions need to be run
    private List<int> waitingDelete = new List<int>();                  // actions need to be deleted

    protected void Update() {
        foreach (SSAction action in waitingAdd) {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction action = kv.Value;
            if (action.destroy) {       // delete this action
                waitingDelete.Add(action.GetInstanceID());
            }
            else if (action.enable) {   // run this action
                action.Update();
                action.FixedUpdate();
            }
        }

        foreach (int key in waitingDelete) {
            SSAction action = actions[key];
            actions.Remove(key);
            Destroy(action);
        }
        waitingDelete.Clear();
    }

    /* add action to a gameobject */
    public void addAction(GameObject gameObject, SSAction action, ISSActionCallback manager) {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}
