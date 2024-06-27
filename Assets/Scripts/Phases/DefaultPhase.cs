using System;

public class DefaultPhase : IGamePhase
{
    private UIController _uiController;
    private PhaseMachine _phaseMachine;

    public DefaultPhase(UIController uiController, PhaseMachine phaseMachine) { 
        _uiController = uiController;
        _phaseMachine = phaseMachine;  
    }
    public void Enter() {
        _uiController.animations.enabled = true;
    }
    public void Exit() {
        _uiController.animations.enabled = false;
    }

    public void MainButtonClick() { 
        _phaseMachine.MoveToNextState();
    }

    public IGamePhase GetNextPhase() {
        return _phaseMachine.GetPhase<TextMovementPhase>();
    }
}
