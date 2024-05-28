using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionPhase : GamePhase
{

    private GamePhaseMachine _phaseMachine;
   
    private EventRegistry m_EventRegistry = new EventRegistry();
    public QuestionPhase(GamePhaseMachine phaseMachine) : base("QuestionPhase", phaseMachine) {
        _phaseMachine = phaseMachine;
    }

    public override void Enter() {
        _phaseMachine.playButton.RegisterCallback<ClickEvent>(MainButtonClick);
        _phaseMachine.question.style.display = DisplayStyle.Flex;
        _phaseMachine.answerBox.style.display = DisplayStyle.Flex;
        _phaseMachine.question.RemoveFromClassList("question_small");
        _phaseMachine.answerBox.RemoveFromClassList("question_small");
    }
    private void MainButtonClick(ClickEvent e) {
        ChangePhaseAfterTransition(_phaseMachine.textMovementPhase);
    }

    private void ChangePhaseAfterTransition(GamePhase phase) {
        _phaseMachine.question.AddToClassList("question_small");
        _phaseMachine.answerBox.AddToClassList("question_small");
        m_EventRegistry.RegisterCallback<TransitionEndEvent>
        (_phaseMachine.question, evt => {
            _phaseMachine.ChangePhase(phase);
        });
    }
    public override void Exit() {
        _phaseMachine.question.style.display = DisplayStyle.None;
        _phaseMachine.answerBox.style.display = DisplayStyle.None;
        _phaseMachine.playButton.UnregisterCallback<ClickEvent>(MainButtonClick);
        m_EventRegistry.Dispose();
    }

    public void ClickAnswerRight(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.green;
        Debug.Log("right");
        _phaseMachine.progress.MoveCaterpillar();
        Next();
    }
    public void ClickAnswerWrong(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.red;
        Debug.Log("wrong");
        _phaseMachine.progress.DeleteFruit();
    }

    private void Next() {
        if (_phaseMachine.IsNextQuestion()) {
            ChangePhaseAfterTransition(_phaseMachine.questionPhase);
            _phaseMachine.RestartNewQuestion();
        }
        else if (_phaseMachine.IsNextStepText()) {
            ChangePhaseAfterTransition(_phaseMachine.textMovementPhase);
            _phaseMachine.RestartNewText();
        }
        else {
            ChangePhaseAfterTransition(_phaseMachine.defaultPhase);
            Debug.Log("End Of Data");
        }
    }
}
