using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePhase
{
    public string name;
    protected PhaseMachine phaseMachine;

    public GamePhase(string name, PhaseMachine phaseMachine) {
        this.name = name;
        this.phaseMachine = phaseMachine;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }
}
