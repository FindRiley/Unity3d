using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstController : MonoBehaviour, ISceneController, UserAction {
    InteractGUI userGUI;
    public CoastController fromCoast;
    public CoastController toCoast;
    public BoatController boat;
    private MyCharacterController[] characters;

    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<InteractGUI>() as InteractGUI;
        characters = new MyCharacterController[6];
        LoadResources();
    }

    public void LoadResources() {
        GameObject water = Instantiate(Resources.Load("Prefabs/WaterProDaytime", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        water.name = "water";

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
        if (boat.isEmpty())
            return;
        boat.Moving();
        userGUI.status = check();
    }

    public void IsClicked(MyCharacterController characterCtrl) {
        if (characterCtrl.IsOnBoat()) {
            CoastController whichCoast;
            if (boat.getState() == -1) { // to->-1; from->1
                whichCoast = toCoast;
            }
            else {
                whichCoast = fromCoast;
            }

            boat.GetOffBoat(characterCtrl.getName());
            characterCtrl.moveToPosition(whichCoast.getEmptyPosition());
            characterCtrl.getOnCoast(whichCoast);
            whichCoast.getOnCoast(characterCtrl);
        }
        else {                                  // character on coast
            CoastController whichCoast = characterCtrl.getCoastController();

            if (boat.getEmptyIndex() == -1) {       // boat is full
                return;
            }

            if (whichCoast.getState() != boat.getState())   // boat is not on the side of character
                return;

            whichCoast.getOffCoast(characterCtrl.getName());
            characterCtrl.moveToPosition(boat.getEmptyPosition());
            characterCtrl.getOnBoat(boat);
            boat.GetOnBoat(characterCtrl);
        }
        userGUI.status = check();
    }
    int check() { // 0->not finish, 1->lose, 2->win
        int fromPriest = 0;
        int fromDevil = 0;
        int toPriest = 0;
        int toDevil = 0;

        int[] fromCount = fromCoast.getCharacterNum();
        fromPriest += fromCount[0];
        fromDevil += fromCount[1];

        int[] toCount = toCoast.getCharacterNum();
        toPriest += toCount[0];
        toDevil += toCount[1];

        if (toPriest + toDevil == 6)      // win
            return 2;

        int[] boatCount = boat.getCharacterNum();
        if (boat.getState() == -1) {  // boat at toCoast
            toPriest += boatCount[0];
            toDevil += boatCount[1];
        }
        else {  // boat at fromCoast
            fromPriest += boatCount[0];
            fromDevil += boatCount[1];
        }
        if (fromPriest > 0 && fromPriest < fromDevil) {      // lose
            return 1;
        }
        if (toPriest > 0 && toPriest < toDevil) {
            return 1;
        }
        return 0;           // not finish
    }

    public void Restart() {
        boat.reset();
        fromCoast.reset();
        toCoast.reset();
        for (int i = 0; i < characters.Length; i++) {
            characters[i].reset();
        }
    }
}
