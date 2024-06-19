using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class TextMovementPhase : IGamePhase {
    private UIController _uiController;
    private PhaseMachine _phaseMachine;
    private ShowTextAction _showTextAction;

    public TextMovementPhase(UIController uiController, PhaseMachine phaseMachine, ShowTextAction showTextAction) {
        _uiController = uiController;
        _phaseMachine = phaseMachine;
        _showTextAction = showTextAction;
    }

    public void Enter() {
        _uiController.InitCurrentStepText();
        _showTextAction.Show();

        _uiController.speedUp.RegisterCallback<MouseDownEvent>(_uiController.AddSpeed);
        _uiController.speedUp.RegisterCallback<MouseUpEvent>(_uiController.RemoveSpeed);

        _showTextAction.SubscribeOnFinished = () => {
            _phaseMachine.MoveToNextState();
            _showTextAction.UnsubscribeAll();
        };
    }

    public void Exit() {

        _uiController.speedUp.UnregisterCallback<MouseDownEvent>(_uiController.AddSpeed);
        _uiController.speedUp.UnregisterCallback<MouseUpEvent>(_uiController.RemoveSpeed);
    }
    public void MainButtonClick() { }
    public IGamePhase GetNextPhase() {
        return _phaseMachine.GetPhase<QuestionPhase>();
    }

}
