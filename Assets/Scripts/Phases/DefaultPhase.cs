using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DefaultPhase : GamePhase
{
    private GamePhaseMachine _phaseMachine;
    public DefaultPhase(GamePhaseMachine phaseMachine) : base("DefaultPhase", phaseMachine) {
        _phaseMachine = phaseMachine;
    }

    public override void Enter() {
        _phaseMachine.animations.enabled = true;
        _phaseMachine.playButton.RegisterCallback<ClickEvent>(MainButtonClick);
    }

    private void MainButtonClick(ClickEvent e) {
        _phaseMachine.ChangePhase(_phaseMachine.textMovementPhase);
    }
    public override void Exit() {
        _phaseMachine.animations.enabled = false;
        _phaseMachine.playButton.UnregisterCallback<ClickEvent>(MainButtonClick);
    }
}
