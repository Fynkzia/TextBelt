using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class QuestionPhase : IGamePhase
{
    private UIController _uiController;
    private PhaseMachine _phaseMachine;
    private EventRegistry _eventRegistry;
    public QuestionPhase(UIController uiController, PhaseMachine phaseMachine, EventRegistry eventRegistry) {
        _uiController = uiController;
        _phaseMachine = phaseMachine;
        _eventRegistry = eventRegistry;
    }
    public void Enter() {
        _uiController.InitQuestionText();
        _uiController.InitAnswerBox();
        _uiController.ShowQuestionBox();
        
    }

    public void Exit() {
        _uiController.CloseQuestionBox();
        //_eventRegistry.Dispose();
    }



    public IGamePhase GetNextPhase() {
        /*if (_phaseMachine.IsNextQuestion()) {
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
        }*/

        return _phaseMachine.GetPhase<QuestionPhase>();
    }
}
