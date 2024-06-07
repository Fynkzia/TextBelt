using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class DefaultPhase : IGamePhase
{
    private UIController _uiController;
    private PhaseMachine _phaseMachine;
    private EventRegistry _eventRegistry;

    public DefaultPhase(UIController uiController, PhaseMachine phaseMachine, EventRegistry eventRegistry) { 
        _uiController = uiController;
        _phaseMachine = phaseMachine;  
        _eventRegistry = eventRegistry;
    }
    public void Enter() {
        _uiController.animations.enabled = true;
    }
    public void Exit() {
        _uiController.animations.enabled = false;
    }

    public IGamePhase GetNextPhase() {
        return _phaseMachine.GetPhase<TextMovementPhase>();
    }
}
