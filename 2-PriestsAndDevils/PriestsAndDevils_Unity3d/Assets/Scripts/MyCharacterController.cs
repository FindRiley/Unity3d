﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController {
    readonly GameObject character;
    readonly Move move;
    readonly ClickGUI clickGUI;
    readonly int characterType; // 0->Priest, 1->Devil

    bool isOnBoat = false;
    CoastController coastController;

    public MyCharacterController(string type) {
        if (type == "Priest") {
            character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), new Vector3(3.5f, 0.75f, 0), Quaternion.identity, null) as GameObject;
            characterType = 0;
        } else {
            character = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), new Vector3(6.5f, 0.75f, 0), Quaternion.identity, null) as GameObject;
            characterType = 1;
        }
        move = character.AddComponent(typeof(Move)) as Move;
        clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
        clickGUI.setController(this);
    }

    public void setName(string name) {
        character.name = name;
    }

    public void setPosition(Vector3 pos) {
        character.transform.position = pos;
    }

    public void moveToPosition(Vector3 destination) {
        move.SetDestination(destination);
    }

    public int getType() {
        return characterType;
    }

    public string getName() {
        return character.name;
    }

    public void getOnBoat(BoatController boatCtrl) {
        coastController = null;
        character.transform.parent = boatCtrl.getGameobj().transform;
        isOnBoat = true;
    }

    public void getOnCoast(CoastController coastCtrl) {
        coastController = coastCtrl;
        character.transform.parent = null;
        isOnBoat = false;
    }

    public bool IsOnBoat() {
        return isOnBoat;
    }

    public CoastController getCoastController() {
        return coastController;
    }

    public void reset() {
        move.Reset();
        coastController = (SSDirector.getInstance().currentSceneController as FirstController).fromCoast;
        getOnCoast(coastController);
        setPosition(coastController.getEmptyPosition());
        coastController.getOnCoast(this);
    }
}
