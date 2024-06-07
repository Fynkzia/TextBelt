using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Cysharp.Threading.Tasks;

public class TextMovementPhase : IGamePhase
{
    private UIController _uiController;
    private PhaseMachine _phaseMachine;
    private EventRegistry _eventRegistry;
    private ShowTextAction _showTextAction;

    public TextMovementPhase(UIController uiController, PhaseMachine phaseMachine, EventRegistry eventRegistry, ShowTextAction showTextAction) {
        _uiController = uiController;
        _phaseMachine = phaseMachine;
        _eventRegistry = eventRegistry;
        _showTextAction = showTextAction;
    }

    public void Enter() {
        _uiController.InitCurrentStepText();
        _showTextAction.Show(_uiController);
        _showTextAction.SubscribeOnFinished = () => {
            _phaseMachine.MoveToNextState();
            _showTextAction.UnsubscribeAll();
        };
    }

    public void Exit() {
    }

    public IGamePhase GetNextPhase() {
        return _phaseMachine.GetPhase<QuestionPhase>();
    }

}
