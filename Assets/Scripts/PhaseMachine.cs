using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMachine
{
    public IGamePhase currentPhase { get; private set; }
    private Dictionary<Type,IGamePhase> _phases = new Dictionary<Type, IGamePhase>();

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
    public IGamePhase GetPhase<T>() { 
        return _phases[typeof(T)];
    }
    public void AddPhase<T>(IGamePhase newPhase) { 
        _phases.Add(typeof(T), newPhase);
    }

}
