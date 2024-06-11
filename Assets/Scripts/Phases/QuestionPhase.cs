using System;
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
    public Action OnFinished { get; set; }
    public QuestionPhase(UIController uiController, PhaseMachine phaseMachine) {
        _uiController = uiController;
        _phaseMachine = phaseMachine;
    }
    public void Enter() {
        _uiController.InitQuestionText();
        _uiController.InitAnswerBox();
        _uiController.ShowQuestionBox();
        
    }

    public void Exit() {
        _uiController.CloseQuestionBox();
        _uiController.SubscribeOnFinished = () => {
            OnFinished?.Invoke();
        };
    }



    public IGamePhase GetNextPhase() {
        return _phaseMachine.GetPhase<QuestionPhase>();
    }
}
