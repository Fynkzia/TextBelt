using GoogleImporter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private DataService dataService;
    [Inject] private DiContainer _container;
    [Inject] private PhaseMachine _phaseMachine;

    public int _currentStepIndex { get; private set; }
    public int _currentQuestionIndex { get; private set; }

    private void OnEnable() {
        _currentStepIndex = 0;
        _currentQuestionIndex = 0;     
    }

    private void Start() {
        PhaseMachineInit();
    }

    private void PhaseMachineInit() {
        var uiController = _container.Resolve<UIController>();
        var showTextAction = _container.Resolve<ShowTextAction>();

        var defaultPhase = new DefaultPhase(uiController, _phaseMachine);
        var textMovementPhase = new TextMovementPhase(uiController, _phaseMachine, showTextAction);
        var questionPhase = new QuestionPhase(uiController, _phaseMachine, this);
        _phaseMachine.AddPhase<DefaultPhase>(defaultPhase);
        _phaseMachine.AddPhase<TextMovementPhase>(textMovementPhase);
        _phaseMachine.AddPhase<QuestionPhase>(questionPhase);

        _phaseMachine.ChangeState(_phaseMachine.GetPhase<DefaultPhase>());
        uiController.OnAnswerButtonClick += () => _phaseMachine.MoveToNextState();
    }

    public void MainButtonClick() {
        _phaseMachine.currentPhase.MainButtonClick();
    }

    public void RestartNewText() {
        _currentStepIndex++;
        _currentQuestionIndex = 0;
    }

    public void RestartNewQuestion() {
        _currentQuestionIndex++;
    }

    public bool IsNextQuestion() {
        return dataService.HasQuestionText(_currentStepIndex, _currentQuestionIndex + 1);
    }

    public bool IsNextStepText() {
        return dataService.HasStepText(_currentStepIndex + 1);
    }

}
