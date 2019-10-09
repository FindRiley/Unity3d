using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstController : MonoBehaviour, ISceneController, UserAction {
    public InteractGUI userGUI;
    public GameObject board;
    public int round, trial, flames;

    FirstActionManager firstActionManager;
    DiskFactory diskFactory;
    bool start = false;
    int score;
    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<InteractGUI>() as InteractGUI;
        diskFactory = DiskFactory.diskFactory;
        round = 1;
        score = 0;
        flames = 60;
    }

    void Start() {
        firstActionManager = gameObject.AddComponent<FirstActionManager>() as FirstActionManager;
        board = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/board"));
    }
    public void Update() {
        --flames;
        if (flames <= 0 && start == true) {
            flames = Random.Range(60 - round * 3, 60 - round * 1);
            ++trial;
            Disk disk = diskFactory.GetDisk(round);
            firstActionManager.MoveDisk(disk);
            if (trial >= 10) {
                trial = 0;
                ++round;
            }
        }
        if (Input.GetButtonDown("Fire1")) {
            Hit(Input.mousePosition);
        }
    }
    
    public void Hit(Vector3 pos) {
        Ray ray = Camera.main.ScreenPointToRay(pos);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; ++i) {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<Disk>() != null) {
                if (Mathf.Abs(hit.collider.gameObject.GetComponent<Disk>().getSpeedX()) > diskFactory.baseSpeedX + 6 * round) {
                    score += 3;
                } else {
                    score += 1;
                }
                hit.collider.gameObject.transform.position = new Vector3(0, -30, 0);
            }
        }
    }

    public int getScore() {
        return score;
    }

    public void Restart() {
        score = 0;
        round = 1;
        start = true;
    }

    public int getRound() {
        return round;
    }

    public void stopGame() {
        start = false;
    }
}
