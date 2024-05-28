using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class TextMovementPhase : GamePhase
{
    private GamePhaseMachine _phaseMachine;
    private EventRegistry m_EventRegistry = new EventRegistry();
    public TextMovementPhase(GamePhaseMachine phaseMachine) : base("TextMovementPhase", phaseMachine) {
        _phaseMachine = phaseMachine;
    }

    public override void Enter() {
        _phaseMachine.playButton.RegisterCallback<ClickEvent>(MainButtonClick);
        m_EventRegistry.Dispose();
        _phaseMachine.overlay.style.overflow = Overflow.Visible;
        _phaseMachine.textBelt.AddToClassList("textbelt__open");
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_phaseMachine.textBelt, MoveText);
    }

    private void MoveText(TransitionEndEvent evt) {
        m_EventRegistry.Dispose();
        _phaseMachine.actualText.style.transitionDuration = new List<TimeValue> { new(_phaseMachine.duration, TimeUnit.Second) };
        _phaseMachine.actualText.AddToClassList("actualText_endPos");
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_phaseMachine.actualText, 
            evt => ChangePhaseAfterTransition(_phaseMachine.questionPhase)
        );
    }
    private void MainButtonClick(ClickEvent e) {
        ChangePhaseAfterTransition(_phaseMachine.textMovementPhase);
    }

    private void ChangePhaseAfterTransition(GamePhase phase) {
        m_EventRegistry.Dispose();
        _phaseMachine.actualText.style.transitionDuration = new List<TimeValue>() { new TimeValue(1, TimeUnit.Second) };
        _phaseMachine.textBelt.RemoveFromClassList("textbelt__open");
        _phaseMachine.actualText.RemoveFromClassList("actualText_endPos");
        _phaseMachine.overlay.style.overflow = Overflow.Hidden;
        _phaseMachine.playButton.UnregisterCallback<ClickEvent>(MainButtonClick);

        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_phaseMachine.textBelt,
            evt => _phaseMachine.ChangePhase(phase)
        );
    } 

    public override void Exit() {
        m_EventRegistry.Dispose();
    }
}
