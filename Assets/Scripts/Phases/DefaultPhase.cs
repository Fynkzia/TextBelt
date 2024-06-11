using System;

public class DefaultPhase : IGamePhase
{
    private UIController _uiController;
    private PhaseMachine _phaseMachine;
    public Action OnFinished { get; set; }

    public DefaultPhase(UIController uiController, PhaseMachine phaseMachine) { 
        _uiController = uiController;
        _phaseMachine = phaseMachine;  
    }
    public void Enter() {
        _uiController.animations.enabled = true;
    }
    public void Exit() {
        _uiController.animations.enabled = false;
        OnFinished?.Invoke();
    }

    public IGamePhase GetNextPhase() {
        return _phaseMachine.GetPhase<TextMovementPhase>();
    }
}
