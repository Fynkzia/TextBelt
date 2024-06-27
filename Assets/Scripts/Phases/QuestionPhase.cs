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
    private GameManager _gameManager;
    public QuestionPhase(UIController uiController, PhaseMachine phaseMachine, GameManager gameManager) {
        _uiController = uiController;
        _phaseMachine = phaseMachine;
        _gameManager = gameManager;
    }
    public void Enter() {
        _uiController.InitQuestionText();
        _uiController.InitAnswerBox();
        _uiController.ShowQuestionBox();
        
    }

    public void Exit() {
        _uiController.CloseQuestionBox();
    }

    public void MainButtonClick() {
        _phaseMachine.ChangeState(_phaseMachine.GetPhase<TextMovementPhase>());
    }

    public IGamePhase GetNextPhase() {
        if (_gameManager.IsNextQuestion()) {
            _gameManager.RestartNewQuestion();
            return _phaseMachine.GetPhase<QuestionPhase>();
        }
        else if (_gameManager.IsNextStepText()) {
            _gameManager.RestartNewText();
            return _phaseMachine.GetPhase<TextMovementPhase>();
        }
        else {
            Debug.Log("End Of Data");
            return _phaseMachine.GetPhase<DefaultPhase>();
        }
    }
}
