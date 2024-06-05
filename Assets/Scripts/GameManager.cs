using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int textSpeed;

    [HideInInspector] public DefaultPhase defaultPhase;
    [HideInInspector] public TextMovementPhase textMovementPhase;
    [HideInInspector] public QuestionPhase questionPhase;

    [Inject] private DataService dataService;
    [Inject] private UIController uiController;

    [HideInInspector] public int duration;
    private EventRegistry m_EventRegistry = new EventRegistry();

    private int _currentStepIndex;
    private int _currentQuestionIndex;

    private PhaseMachine phaseMachine;
    public static GameManager instance;

    private void OnEnable() {
       
        _currentStepIndex = 0;
        _currentQuestionIndex = 0;
        InitDataText();
        //InitDataQuestion();        
    }

    private void Start() {
        if (instance == null) { 
            instance = this; 
        }
        PhaseMachineInit();
        phaseMachine.ChangeState(defaultPhase);
        
    }

    private void PhaseMachineInit() {
        defaultPhase = new DefaultPhase(uiController);
        textMovementPhase = new TextMovementPhase(uiController);
        questionPhase = new QuestionPhase(uiController);
        phaseMachine = new PhaseMachine();
    }

    public void MainButtonClick() {
        phaseMachine.ChangeState(textMovementPhase);
    }

    private void InitDataText() {
        duration = GetDuration();
        //actualText.text = dataService.GetCurrentStepText(_currentStepIndex);
    }

    private void InitDataQuestion() {
        //questionLabel.text = dataService.GetCurrentQuestionText(_currentStepIndex,_currentQuestionIndex);
        InitAnswerButtons(dataService.GetAnswers(_currentStepIndex, _currentQuestionIndex));
    }
    public int GetDuration() {
        return dataService.GetCurrentStepText(_currentStepIndex).Length / textSpeed;
    }

    private void InitAnswerButtons(List<TextBlockData.Answer> answers) {
        for (int i = 0; i < answers.Count; i++) {
            var answerBtn = new Button();
            answerBtn.AddToClassList("answerBtn");
            answerBtn.text = answers[i].text;
            //answerBox.Add(answerBtn);
            if (answers[i].isRight) {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, questionPhase.ClickAnswerRight);
            }
            else {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, questionPhase.ClickAnswerWrong);
            }
        }
    }

    private void OnEnd() {
        //answerBox.Clear();
        m_EventRegistry.Dispose();
    }

    public void RestartNewText() {
        _currentStepIndex++;
        _currentQuestionIndex = 0;
        OnEnd();
        InitDataText();
        InitDataQuestion();
    }

    public void RestartNewQuestion() {
        _currentQuestionIndex++;
        OnEnd();
        InitDataQuestion();
    }

    public bool IsNextQuestion() {
        return dataService.HasQuestionText(_currentStepIndex, _currentQuestionIndex + 1);
    }

    public bool IsNextStepText() {
        return dataService.HasStepText(_currentStepIndex + 1);
    }

}
