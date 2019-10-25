using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory {
    public static PatrolFactory patrolFactory = new PatrolFactory();
    private Dictionary<int, GameObject> used = new Dictionary<int, GameObject>();
    int[] pos_x = { -12, 3, 12 };     // patrols' positions
    int[] pos_z = { 12, 3, -12 };
    private PatrolFactory() { }

    public Dictionary<int, GameObject> GetPatrols() {
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                GameObject newPatrol = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Monster"));
                newPatrol.AddComponent<Patrol>();
                newPatrol.transform.position = new Vector3(pos_x[j], -0.3f, pos_z[i]);
                newPatrol.GetComponent<Patrol>().area = i * 3 + j;
                newPatrol.SetActive(true);
                used.Add(i * 3 + j, newPatrol);
            }
        }
        return used;
    }

    public void StopPatrols() {
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                used[i * 3 + j].transform.position = new Vector3(pos_x[j], -0.3f, pos_z[i]);
            }
        }
    }
}
