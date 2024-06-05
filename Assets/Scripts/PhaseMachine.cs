using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhaseMachine
{
    private IGamePhase currentPhase;

    public void ChangeState(IGamePhase newPhase) {
        if (currentPhase != null) {
            currentPhase.Exit();
        }

        currentPhase = newPhase;

        if (currentPhase != null) {
            currentPhase.Enter();
        }
    }

    public void MoveToNextState() {
        if (currentPhase != null) {
            ChangeState(currentPhase.GetNextPhase());
        }
    }
}
