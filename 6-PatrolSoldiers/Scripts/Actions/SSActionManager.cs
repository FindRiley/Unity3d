using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Update() {
        foreach(SSAction action in waitingAdd) {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();

        foreach(SSAction action in actions.Values) {
            if (action.destroy) {
                waitingDelete.Add(action.GetInstanceID());
            }
            else if (action.enable) {
                action.Update();
            }
        }

        foreach(int key in waitingDelete) {
            SSAction action = actions[key];
            actions.Remove(key);
            Destroy(action);
        }
        waitingDelete.Clear();
    }

    public void addAction(GameObject gameobject, SSAction action, SSActionCallback controller) {
        action.gameobject = gameobject;
        action.transform = gameobject.transform;
        action.callBack = controller;
        waitingAdd.Add(action);
        action.Start();
    }
}
