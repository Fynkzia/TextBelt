using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class UIController : MonoBehaviour
{
    public Button playButton;
    public VisualElement textBelt;
    public VisualElement overlay;
    public Label actualText;
    public VisualElement question;
    public Label questionLabel;
    public GroupBox answerBox;

    [HideInInspector] public Progress progress;
    [HideInInspector] public Animations animations;

    [Inject] private GameManager gameManager;
    [Inject] private DataService dataService;
    private int _duration;
    private EventRegistry m_EventRegistry = new EventRegistry();
    private void OnEnable() {
        GetAllComponents();
        animations = GetComponent<Animations>();
        progress = GetComponent<Progress>();
       
    }
    private void Start() {
        playButton.RegisterCallback<ClickEvent>(e => gameManager.MainButtonClick());
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
    }
    public void InitCurrentStepText() {
        int currentStepIndex = gameManager._currentStepIndex;
        _duration = dataService.GetCurrentStepText(currentStepIndex).Length / gameManager.textSpeed;
        actualText.text = dataService.GetCurrentStepText(currentStepIndex);
    }

    public void InitAnswerButtons() {
        List<TextBlockData.Answer> answers = dataService.GetAnswers(gameManager._currentStepIndex, gameManager._currentQuestionIndex);
        for (int i = 0; i < answers.Count; i++) {
            var answerBtn = new Button();
            answerBtn.AddToClassList("answerBtn");
            answerBtn.text = answers[i].text;
            answerBox.Add(answerBtn);
            if (answers[i].isRight) {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, ClickAnswerRight);
            }
            else {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, ClickAnswerWrong);
            }
        }
    }
    public void ClickAnswerRight(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.green;
        Debug.Log("right");
        progress.MoveCaterpillar();
        //Next();//event for gm??
    }
    public void ClickAnswerWrong(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.red;
        Debug.Log("wrong");
        progress.DeleteFruit();//event for gm??
    }
    public void ShowTextBelt() {
        overlay.style.overflow = Overflow.Visible;
        textBelt.AddToClassList("textbelt__open");
    }

    public void MoveText() {
        actualText.style.transitionDuration = new List<TimeValue> { new(_duration, TimeUnit.Second) };
        actualText.AddToClassList("actualText_endPos");
    }

    public void CloseTextBelt() {
        actualText.style.transitionDuration = new List<TimeValue>() { new TimeValue(1, TimeUnit.Second) };
        textBelt.RemoveFromClassList("textbelt__open");
        actualText.RemoveFromClassList("actualText_endPos");
        overlay.style.overflow = Overflow.Hidden;
    }
    public void InitQuestionText() {
        questionLabel.text = dataService.GetCurrentQuestionText(gameManager._currentStepIndex,gameManager._currentQuestionIndex);
    }

    public void InitAnswerBox() {
        answerBox.Clear();
        InitAnswerButtons();
    }
    public void ShowQuestionBox() {
        question.style.display = DisplayStyle.Flex;
        answerBox.style.display = DisplayStyle.Flex;
        question.RemoveFromClassList("question_small");
        answerBox.RemoveFromClassList("question_small");
    }

    public void CloseQuestionBox() {
        question.AddToClassList("question_small");
        answerBox.AddToClassList("question_small");
        question.RegisterCallback<TransitionEndEvent>(HideQuestionBox);
    }

    private void HideQuestionBox(TransitionEndEvent evt) {
        question.style.display = DisplayStyle.None;
        answerBox.style.display = DisplayStyle.None;
        question.UnregisterCallback<TransitionEndEvent>(HideQuestionBox);
    }

    private void OnDestroy() {
        playButton.UnregisterCallback<ClickEvent>(e => gameManager.MainButtonClick());
    }
}
