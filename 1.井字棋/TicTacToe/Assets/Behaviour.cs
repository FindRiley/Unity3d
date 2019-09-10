using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    private int[,] table = new int[3, 3];
    private int count;
    private bool flag;
    private string[] piece = { "o", "X" };
    private Texture2D img;
    private GUIStyle style = new GUIStyle();

    void Start() {
        Reset();
    }

    void OnGUI() {
        GUI.Label(new Rect(0, 0, 100, 100), img);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 30;
        style.normal.textColor = new Color((float)0.14, (float)0.12, (float)0.22, (float)0.8);
        GUISkin skin = GUI.skin;

        if (isWin()) {
            flag = true;
        }
        else if (count >= 9) GUI.Label(new Rect(300, 60, 150, 40), "Even", style);
        else
            GUI.Label(new Rect(300, 60, 150, 40), "Playing...", style);
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                if (!flag && table[i, j] == -1 && GUI.Button(new Rect(300 + i * 50, 100 + j * 50, 50, 50), "")) {
                    table[i, j] = count % 2;
                    ++count;
                }
                if (table[i, j] >= 0) {
                    GUI.Label(new Rect(300 + i * 50, 100 + j * 50, 50, 50), piece[table[i, j]], style);
                }
            }
        }
        if (GUI.Button(new Rect(335, 260, 80, 50), "Restart")) {
            Reset();
        }
    }

    private void Reset() {
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                table[i, j] = -1;
        count = 0;
        flag = false;
    }

    private bool isWin() {
        string temp = "";
        for (int i = 0; i < 3; ++i) {
            if (table[i, 0] != -1 && table[i, 0] == table[i, 1] && table[i, 0] == table[i, 2])
                temp = piece[table[i, 0]];
        }
        for (int j = 0; j < 3; ++j) {
            if (table[0, j] != -1 && table[0, j] == table[1, j] && table[0, j] == table[2, j])
                temp = piece[table[0, j]];
        }
        if (table[1, 1] != -1 && ((table[1, 1] == table[0, 0] && table[1, 1] == table[2, 2])
            || (table[1, 1] == table[0, 2] && table[1, 1] == table[2, 0])))
            temp = piece[table[1, 1]];
        if (temp != "") {
            GUI.Label(new Rect(300, 60, 150, 40), temp + " WINS!", style);
            return true;
        }
        return false;
    }
}
