using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces {
    public interface ISceneController {}

    public interface UserAction {
        void Hit(Vector3 pos);
        int getScore();
        int getRound();
        bool getMode();
        void stopGame();
        void Restart();
        void switchMode();
    }

    public enum SSActionEventType: int { Started, Completed}

    public interface ISSActionCallback {
        void SSActionCallback(SSAction source);
    }

    public interface IActionManager {
        void MoveDisk(Disk disk);
    }
}