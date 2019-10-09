using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces {
    public interface ISceneController {}

    public interface UserAction {
        void Hit(Vector3 pos);
        int getScore();
        void stopGame();
        int getRound();
        void Restart();
    }

    public enum SSActionEventType: int { Started, Completed}

    public interface ISSActionCallback {
        void SSActionCallback(SSAction source);
    }
}