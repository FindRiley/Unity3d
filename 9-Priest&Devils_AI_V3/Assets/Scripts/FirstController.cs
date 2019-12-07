using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public enum BoatAction { P, D, PP, DD, PD};

public struct NextPass {
    public bool isFrom;
    public BoatAction boataction;
}

public class FirstController : MonoBehaviour, ISceneController, UserAction {
    InteractGUI userGUI;
    public GameObject terrain;
    public CoastController fromCoast;
    public CoastController toCoast;
    public BoatController boat;
    public Judge judge;
    public int gameStatus;

    MyCharacterController[] characters;
    FirstActionManager firstActionManager;
    NextPass nextpass;
    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<InteractGUI>() as InteractGUI;
        characters = new MyCharacterController[6];
        LoadResources();
        judge = gameObject.AddComponent<Judge>() as Judge;
        nextpass.isFrom = true;
        gameStatus = 0; // 0->get on boat; 1->move boat; 2->get off boat
    }

    void Start() {
        firstActionManager = gameObject.AddComponent<FirstActionManager>() as FirstActionManager;
    }

    public void LoadResources() {
        GameObject water = Instantiate(Resources.Load("Prefabs/WaterProDaytime", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        water.name = "water";
        terrain = Instantiate(Resources.Load("Prefabs/Terrain", typeof(GameObject)), new Vector3(-20, -3, -21), Quaternion.identity, null) as GameObject;

        fromCoast = new CoastController("from");
        toCoast = new CoastController("to");
        boat = new BoatController();

        for (int i = 0; i < 3; ++i) {
            MyCharacterController priest = new MyCharacterController("Priest");
            priest.setName("Priest" + i);
            priest.setPosition(fromCoast.getEmptyPosition());
            priest.getOnCoast(fromCoast);
            fromCoast.getOnCoast(priest);
            characters[i] = priest;
        }

        for (int i = 0; i < 3; ++i) {
            MyCharacterController devil = new MyCharacterController("Devil");
            devil.setName("Devil" + i);
            devil.setPosition(fromCoast.getEmptyPosition());
            devil.getOnCoast(fromCoast);
            fromCoast.getOnCoast(devil);
            characters[i + 3] = devil;
        }
    }
    public void MoveBoat() {
        if (boat.isEmpty() || firstActionManager.Complete == SSActionEventType.Started) 
            return;
        firstActionManager.MoveBoat(boat);
        userGUI.status = judge.check(fromCoast, toCoast, boat);
        gameStatus = 2; // need get off boat
    }

    public void IsClicked(MyCharacterController characterCtrl) {
        if (firstActionManager.Complete == SSActionEventType.Started) return;
        if (characterCtrl.IsOnBoat()) {
            CoastController whichCoast;
            if (boat.getState() == -1) { // to->-1; from->1
                whichCoast = toCoast;
            }
            else {
                whichCoast = fromCoast;
            }

            boat.GetOffBoat(characterCtrl.getName());
            firstActionManager.MoveCharacter(characterCtrl, whichCoast.getEmptyPosition());
            characterCtrl.getOnCoast(whichCoast);
            whichCoast.getOnCoast(characterCtrl);

            if (boat.isEmpty()) {
                gameStatus = 0; // need get on boat
            }
        }
        else { // character on coast
            CoastController whichCoast = characterCtrl.getCoastController();

            if (boat.getEmptyIndex() == -1) {       // boat is full
                gameStatus = 1;
                return;
            }

            if (whichCoast.getState() != boat.getState())   // boat and character are on different side
                return;

            Debug.Log("get on boat:");
            Debug.Log(characterCtrl.getName());
            whichCoast.getOffCoast(characterCtrl.getName());
            firstActionManager.MoveCharacter(characterCtrl, boat.getEmptyPosition());
            characterCtrl.getOnBoat(boat);
            boat.GetOnBoat(characterCtrl);

            if (boat.getEmptyIndex() == -1)
                gameStatus = 1; // need to move
        }
        userGUI.status = judge.check(fromCoast, toCoast, boat);
    }

    public void GetNextPass () {
        int[] fromCount = fromCoast.getCharacterNum();
        int from_priest = fromCount[0];
        int from_devil = fromCount[1];

        if (nextpass.isFrom && 3 == from_priest && 3 == from_devil) {
            nextpass.boataction = BoatAction.DD;
        }
        else if (!nextpass.isFrom && 3 == from_priest && 1 == from_devil) {
            nextpass.boataction = BoatAction.D;
        }
        else if (!nextpass.isFrom && 3 == from_priest && 2 == from_devil) {
            nextpass.boataction = BoatAction.D;
        }
        else if (!nextpass.isFrom && 2 == from_priest && 2 == from_devil) {
            nextpass.boataction = BoatAction.P;
        }
        else if (nextpass.isFrom && 3 == from_priest && 2 == from_devil) {
            nextpass.boataction = BoatAction.DD;
        }
        else if (!nextpass.isFrom && 3 == from_priest && 0 == from_devil) {
            nextpass.boataction = BoatAction.D;
        }
        else if (nextpass.isFrom && 3 == from_priest && 1 == from_devil) {
            nextpass.boataction = BoatAction.PP;
        }
        else if (!nextpass.isFrom && 1 == from_priest && 1 == from_devil) {
            nextpass.boataction = BoatAction.PD;
        }
        else if (nextpass.isFrom && 2 == from_priest && 2 == from_devil) {
            nextpass.boataction = BoatAction.PP;
        }
        else if (!nextpass.isFrom && 0 == from_priest && 2 == from_devil) {
            nextpass.boataction = BoatAction.D;
        }
        else if (nextpass.isFrom && 0 == from_priest && 3 == from_devil) {
            nextpass.boataction = BoatAction.DD;
        }
        else if (!nextpass.isFrom && 0 == from_priest && 1 == from_devil) {
            nextpass.boataction = BoatAction.D;
        }
        else if (nextpass.isFrom && 0 == from_priest && 2 == from_devil) {
            nextpass.boataction = BoatAction.DD;
        }
        if (nextpass.isFrom && 1 == from_priest && 1 == from_devil) {
            nextpass.boataction = BoatAction.PD;
        }
    }

    IEnumerator Second(MyCharacterController c) {
        yield return new WaitForSeconds(0.3f);
        IsClicked(c);
    }

    public void NextMove() {
        Debug.Log("gameStatus:");
        Debug.Log(gameStatus);

        int[] fromCount = fromCoast.getCharacterNum();
        int from_priest = fromCount[0];
        int from_devil = fromCount[1];

        if (gameStatus == 0 && boat.isEmpty()) { // get on boat
            if (boat.getState() == 1)
                nextpass.isFrom = true;
            else
                nextpass.isFrom = false;
            GetNextPass();
            Debug.Log("next:");
            Debug.Log(nextpass.isFrom);
            Debug.Log(nextpass.boataction);

            CoastController coastPointer;
            if (nextpass.isFrom == true)
                coastPointer = fromCoast;
            else
                coastPointer = toCoast;

            if (nextpass.boataction == BoatAction.PP) {
                IsClicked(coastPointer.FindCharacterOnCoast(0));
                StartCoroutine(Second(coastPointer.FindCharacterOnCoast(0)));
            }
            else if (nextpass.boataction == BoatAction.P) {
                IsClicked(coastPointer.FindCharacterOnCoast(0));
            }
            else if (nextpass.boataction == BoatAction.DD) {
                IsClicked(coastPointer.FindCharacterOnCoast(1));
                StartCoroutine(Second(coastPointer.FindCharacterOnCoast(1)));
            }
            else if (nextpass.boataction == BoatAction.D) {
                IsClicked(coastPointer.FindCharacterOnCoast(1));
            }
            else if (nextpass.boataction == BoatAction.PD) {
                IsClicked(coastPointer.FindCharacterOnCoast(0));
                StartCoroutine(Second(coastPointer.FindCharacterOnCoast(1)));
            }
            gameStatus = 1;
        }
        else if (gameStatus == 1 && (from_devil == from_priest
            || from_devil == 0 || from_priest == 0
            || from_devil == 3 || from_priest == 3)) { // move boat
            MoveBoat();
            gameStatus = 2;
        }
        else if (gameStatus == 2 || !boat.isEmpty()) { // get off boat
            MyCharacterController[] passenger = boat.GetPassengerOnBoat();
            for (int i = 0; i < passenger.Length; ++i) {
                if (passenger[i] != null)
                    StartCoroutine(Second(passenger[i]));
            }
            gameStatus = 0;
        }
    }

    public void Restart() {
        boat.reset();
        fromCoast.reset();
        toCoast.reset();
        gameStatus = 0;
        nextpass.isFrom = true;

        for (int i = 0; i < characters.Length; i++) {
            characters[i].reset();
        }
    }
}
