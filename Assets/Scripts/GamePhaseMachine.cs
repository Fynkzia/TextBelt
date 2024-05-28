using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class GamePhaseMachine : PhaseMachine
{
    [SerializeField] private int textSpeed;

    [HideInInspector] public DefaultPhase defaultPhase;
    [HideInInspector] public TextMovementPhase textMovementPhase;
    [HideInInspector] public QuestionPhase questionPhase;

    [Inject] private DataService dataService;
    [HideInInspector] public Progress progress;
    public Button playButton;
    public VisualElement textBelt;
    public VisualElement overlay;
    public Label actualText;
    public VisualElement question;
    public Label questionLabel;
    public GroupBox answerBox;

    [HideInInspector] public int duration;
    private EventRegistry m_EventRegistry = new EventRegistry();
    [HideInInspector] public Animations animations;

    private int _currentStepIndex;
    private int _currentQuestionIndex;

    private void Awake() {
        defaultPhase = new DefaultPhase(this);
        textMovementPhase = new TextMovementPhase(this);
        questionPhase = new QuestionPhase(this);

    }
    private void OnEnable() {
        GetAllComponents();
        _currentStepIndex = 0;
        _currentQuestionIndex = 0;
        InitDataText();
        InitDataQuestion();        
    }
    private void GetAllComponents() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        playButton = root.Q<Button>("PlayButton");

        textBelt = root.Q<VisualElement>("TextBelt");
        overlay = root.Q<VisualElement>("Overlay");
        actualText = root.Q<Label>("ActualText");

        question = root.Q<VisualElement>("Question");
        questionLabel = root.Q<Label>("QuestionLabel");
        answerBox = root.Q<GroupBox>("AnswerBox");

        animations = GetComponent<Animations>();
        progress = GetComponent<Progress>();
    }

    private void InitDataText() {
        duration = GetDuration();
        actualText.text = dataService.GetCurrentStepText(_currentStepIndex);
    }

    private void InitDataQuestion() {
        questionLabel.text = dataService.GetCurrentQuestionText(_currentStepIndex,_currentQuestionIndex);
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
            answerBox.Add(answerBtn);
            if (answers[i].isRight) {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, questionPhase.ClickAnswerRight);
            }
            else {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, questionPhase.ClickAnswerWrong);
            }
        }
    }

    private void OnEnd() {
        answerBox.Clear();
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

    protected override GamePhase GetInitialPhase() {
        return defaultPhase;
    }
}
