using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class QuestionPhase : IGamePhase
{
    //[Inject]
    private UIController _uiController;
    private EventRegistry m_EventRegistry = new EventRegistry();

    public QuestionPhase(UIController uiController) {
        _uiController = uiController;
    }
    public void Enter() {
        _uiController.InitQuestionText();
        _uiController.InitAnswerBox();
        _uiController.ShowQuestionBox();
        
    }

    private void ChangePhaseAfterTransition(IGamePhase phase) {
        
    }
    public void Exit() {
        
        m_EventRegistry.Dispose();
    }

    public void ClickAnswerRight(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.green;
        Debug.Log("right");
        //_phaseMachine.progress.MoveCaterpillar();
        //Next();
    }
    public void ClickAnswerWrong(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.red;
        Debug.Log("wrong");
        //_phaseMachine.progress.DeleteFruit();
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

        return new QuestionPhase(_uiController);
    }
}
