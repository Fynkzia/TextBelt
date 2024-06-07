using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    public int textSpeed;

/*    [HideInInspector] public DefaultPhase defaultPhase;
    [HideInInspector] public TextMovementPhase textMovementPhase;
    [HideInInspector] public QuestionPhase questionPhase;*/

    [Inject] private DataService dataService;
    [Inject] private DiContainer _container;
    [Inject] private PhaseMachine _phaseMachine;

    /*[Inject] private DefaultPhase _defaultPhase;
    [Inject] private TextMovementPhase _movementPhase;
    [Inject] private QuestionPhase _questionPhase;*/

    [Inject]private EventRegistry _EventRegistry;

    public int _currentStepIndex { get; private set; }
    public int _currentQuestionIndex { get; private set; }

    //private PhaseMachine phaseMachine;

    private void OnEnable() {
        _currentStepIndex = 0;
        _currentQuestionIndex = 0;     
    }

    private void Start() {
        PhaseMachineInit();
        //phaseMachine.ChangeState(defaultPhase);
        
    }

    private void PhaseMachineInit() {
        var uiController = _container.Resolve<UIController>();
        var eventRegistry = _container.Resolve<EventRegistry>();
        var showTextAction = _container.Resolve<ShowTextAction>();

        var defaultPhase = new DefaultPhase(uiController, _phaseMachine, eventRegistry);
        var textMovementPhase = new TextMovementPhase(uiController, _phaseMachine, eventRegistry, showTextAction);
        var questionPhase = new QuestionPhase(uiController, _phaseMachine, eventRegistry);
        _phaseMachine.AddPhase<DefaultPhase>(defaultPhase);
        _phaseMachine.AddPhase<TextMovementPhase>(textMovementPhase);
        _phaseMachine.AddPhase<QuestionPhase>(questionPhase);

        _phaseMachine.ChangeState(_phaseMachine.GetPhase<DefaultPhase>());
    }

    public void MainButtonClick() {
        if (_phaseMachine.currentPhase != _phaseMachine.GetPhase<TextMovementPhase>()) { 
            _phaseMachine.ChangeState(_phaseMachine.GetPhase<TextMovementPhase>());
        }
    }

    

    private void OnEnd() {
        //answerBox.Clear();
        //_eventRegistry.Dispose();
    }

    public void RestartNewText() {
        _currentStepIndex++;
        _currentQuestionIndex = 0;
        OnEnd();
    }

    public void RestartNewQuestion() {
        _currentQuestionIndex++;
        OnEnd();
    }

    public bool IsNextQuestion() {
        return dataService.HasQuestionText(_currentStepIndex, _currentQuestionIndex + 1);
    }

    public bool IsNextStepText() {
        return dataService.HasStepText(_currentStepIndex + 1);
    }

}
