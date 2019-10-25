using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstSceneController : MonoBehaviour, ISceneController, UserAction {
    GameObject player = null;
    PatrolFactory patrolFactory;
    int score = 0;
    int PlayerArea = 4;
    bool gameState = false;
    Dictionary<int, GameObject> patrols = new Dictionary<int, GameObject>();
    CCActionManager actionManager = null;

    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        patrolFactory = PatrolFactory.patrolFactory;
        actionManager = gameObject.AddComponent<CCActionManager>();
        LoadResources();

        if (player.GetComponent<Rigidbody>()) {
            player.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    public void LoadResources() {
        Instantiate(Resources.Load<GameObject>("Prefabs/Plane"), new Vector3(0, 0, 0), Quaternion.identity);
        player = Instantiate(Resources.Load<GameObject>("Prefabs/Cat"), new Vector3(0, 0, -2), Quaternion.identity) as GameObject;
        patrols = patrolFactory.GetPatrols();
    }
    
    void Update() {
        if(player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0) {
            player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
        }
        if (player.transform.position.y != 0) {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        }
    }

    void OnEnable() {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.Gameover += Gameover;
    }

    void OnDisable() {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.Gameover -= Gameover;
    }

    public int GetScore() {
        return score;
    }

    public bool GetGameState() {
        return gameState;
    }

    public void Restart() {
        player.GetComponent<Animator>().Play("Idle");
        patrolFactory.StopPatrols();
        gameState = true;
        score = 0;
        player.transform.position = new Vector3(0, 0, -2);
        player.transform.localEulerAngles = Vector3.zero;

        PlayerArea = 4;
        patrols[PlayerArea].GetComponent<Patrol>().follow_player = true;
        actionManager.CatchUp(patrols[PlayerArea], player);
        actionManager.StopAll();
        foreach (GameObject action in patrols.Values) {
            if (!action.GetComponent<Patrol>().follow_player) {
                actionManager.GoAround(action);
            }
        }
    }

    public void SetPlayerArea(int x) {
        if (PlayerArea != x && gameState) {
            patrols[PlayerArea].GetComponent<Animator>().SetBool("run", false);
            patrols[PlayerArea].GetComponent<Patrol>().follow_player = false;
            PlayerArea = x;
        }
    }

    void AddScore() {
        if (gameState) {
            ++score;
            patrols[PlayerArea].GetComponent<Patrol>().follow_player = true;
            actionManager.CatchUp(patrols[PlayerArea], player);
            patrols[PlayerArea].GetComponent<Animator>().SetBool("run", true);
        }
    }

    void Gameover() {
        actionManager.StopAll();
        patrols[PlayerArea].GetComponent<Patrol>().follow_player = false;
        player.GetComponent<Animator>().SetTrigger("death");
        gameState = false;
    }

    public void MovePlayer(Vector3 speed) {
        if (gameState && player != null) {
            if (speed.x != 0 || speed.z != 0) {
                player.GetComponent<Animator>().SetBool("run", true);
            }
            else {
                player.GetComponent<Animator>().SetBool("run", false);
            }

            player.transform.Rotate(0, speed.x * 135f * Time.deltaTime, 0);
            player.transform.Translate(new Vector3(0, 0, speed.z + System.Math.Abs(speed.x)) * 5f * Time.deltaTime);
        }
    }
}
