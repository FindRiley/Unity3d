using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager {
    public static GameEventManager Instance = new GameEventManager();

    // 计分委托
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;
    public delegate void GameoverEvent();
    public static event GameoverEvent Gameover;

    private GameEventManager() { }

    public void PlayerEscape() {
        if (ScoreChange != null) {
            ScoreChange();
        } 
    }

    public void GameOver() {
        if(Gameover != null) {
            Gameover();
        }
    }
}