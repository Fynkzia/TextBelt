using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMachine : MonoBehaviour
{
    GamePhase currentPhase;

    private void Start() {
        currentPhase = GetInitialPhase();
        if (currentPhase != null) {
            currentPhase.Enter();
        }
    }

    public void ChangePhase(GamePhase newPhase) {
        currentPhase.Exit();

        currentPhase = newPhase;
        currentPhase.Enter();
    }

    protected virtual GamePhase GetInitialPhase() {
        return null;
    }
}
