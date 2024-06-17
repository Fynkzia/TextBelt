using System;
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

    public VisualElement speedUp;

    public int textSpeed;
    public int speedOfStateChange;
    public int answerBlockWidth;

    [HideInInspector] public Progress progress;
    [HideInInspector] public Animations animations;

    [Inject] private GameManager gameManager;
    [Inject] private DataService dataService;

    public int _duration;
    private int speedTextBoost = 2;
    private EventRegistry m_EventRegistry = new EventRegistry();
    public Action SubscribeOnFinished;
    public Action OnTextMoveFinished;
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

        speedUp = root.Q<VisualElement>("SpeedUp");
    }
    public void InitCurrentStepText() {
        int currentStepIndex = gameManager._currentStepIndex;
        _duration = dataService.GetCurrentStepText(currentStepIndex).Length / textSpeed;
        actualText.text = dataService.GetCurrentStepText(currentStepIndex);
    }

    public void InitAnswerButtons() {
        List<TextBlockData.Answer> answers = dataService.GetAnswers(gameManager._currentStepIndex, gameManager._currentQuestionIndex);
        for (int i = 0; i < answers.Count; i++) {
            var answerBtn = new Button();
            answerBtn.AddToClassList("answerBtn");
            answerBtn.style.width = answerBlockWidth;
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
        gameManager.Next();
    }
    public void ClickAnswerWrong(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.red;
        Debug.Log("wrong");
        progress.DeleteFruit();
    }
    public void ShowTextBelt() {
        overlay.style.overflow = Overflow.Visible;
        textBelt.AddToClassList("textbelt__open");
        speedUp.style.display = DisplayStyle.Flex;
    }

    public IEnumerator MoveText() {
        float elapsedTime = 0;

        while (elapsedTime < 1f) {
            actualText.style.translate = new Translate(Length.Percent(Mathf.Lerp(0, -100, elapsedTime)), 0);
            elapsedTime += (1f / _duration) * Time.deltaTime;
            yield return null;
        }
        OnTextMoveFinished?.Invoke();
    }

    public void CloseTextBelt() {
        actualText.style.transitionDuration = new List<TimeValue>() { new TimeValue(speedOfStateChange, TimeUnit.Second) };
        textBelt.RemoveFromClassList("textbelt__open");
        actualText.style.translate = new Translate(0, 0);
        overlay.style.overflow = Overflow.Hidden;
        speedUp.style.display = DisplayStyle.None;
    }
    public void InitQuestionText() {
        questionLabel.text = dataService.GetCurrentQuestionText(gameManager._currentStepIndex,gameManager._currentQuestionIndex);
    }

    public void InitAnswerBox() {
        answerBox.Clear();
        m_EventRegistry.Dispose();
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
        SubscribeOnFinished?.Invoke();
        question.UnregisterCallback<TransitionEndEvent>(HideQuestionBox);
    }

    private void OnDestroy() {
        playButton.UnregisterCallback<ClickEvent>(e => gameManager.MainButtonClick());
    }

    public void AddSpeed(MouseDownEvent evt) {
        _duration /= speedTextBoost;
    }

    public void RemoveSpeed(MouseUpEvent evt) {
        _duration *= speedTextBoost;
    }
}
