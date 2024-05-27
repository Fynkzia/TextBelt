using System;
using System.Collections.Generic;
using System.Management;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public enum Phase { 
    None,
    Two,
    Three,
}
public class Phases : MonoBehaviour
{
    [Inject] private DataService dataService;
    private Progress progress;
    private PhaseTwo phaseTwo;
    private PhaseThree phaseThree;
    private Button playButton;
    private VisualElement textBelt;
    private VisualElement overlay;
    private Label actualText;
    private VisualElement question;
    private Label questionLabel;
    private GroupBox answerBox;

    private int duration;
    private EventRegistry m_EventRegistry = new EventRegistry();
    private Phase _currentPhase = Phase.None;
    private Animations animations;

    private void OnEnable() {
        GetAllComponents();
        progress = GetComponent<Progress>();
        dataService.currentTextChanged
            .Where(x => x == true)
            .Subscribe(x => RestartNewText());
        dataService.currentQuestionChanged
            .Where(x => x == true)
            .Subscribe(x => RestartNewQuestion());
        OnStart();
        playButton.RegisterCallback<ClickEvent>(MainButtonClick);
    }

    private void OnStart() {
        InitDataText();
        InitDataQuestion();
        phaseTwo = new PhaseTwo(textBelt, actualText, overlay,duration);
        phaseThree = new PhaseThree(question, answerBox);
        SubscribeToPhases();
    }

    private void SubscribeToPhases() {
        phaseTwo.PhaseFinished
            .Where(x => x == true)
            .Subscribe(x => {
                _currentPhase = Phase.Three;
                phaseThree.StartPhase();
            });

        phaseTwo.PhaseReseted
            .Where(x => x == true)
            .Delay(TimeSpan.FromSeconds(1))
            .Subscribe(x => {
                _currentPhase = Phase.Two;
                phaseTwo.StartPhase();
            });

        phaseThree.PhaseReseted
            .Where(x => x == true)
            .Subscribe(x => {
                _currentPhase = Phase.Two;
                phaseTwo.StartPhase();
            });
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
    }

    private void MainButtonClick(ClickEvent e) { 
        if(_currentPhase == Phase.Two) {
            phaseTwo.ResetPhase();
        }
        if(_currentPhase == Phase.Three) {
            phaseThree.ResetPhase();
        }
        if(_currentPhase == Phase.None) {
            _currentPhase = Phase.Two;
            phaseTwo.StartPhase();
            animations.enabled = false;
        }
    }

    private void InitDataText() {
        duration = dataService.GetDuration();
        actualText.text = dataService.GetCurrentText();
    }

    private void InitDataQuestion() { 
        questionLabel.text = dataService.GetCurrentQuestionText();
        InitAnswerButtons(dataService.GetAnswers());
    }

    private void InitAnswerButtons(List<TextBlockData.Answer> answers) {
        for (int i = 0; i < answers.Count; i++) {
            var answerBtn = new Button();
            answerBtn.AddToClassList("answerBtn");
            answerBtn.text = answers[i].text;
            answerBox.Add(answerBtn);
            if (answers[i].isRight) {
                m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, ClickAnswerRight);
            }
            else { m_EventRegistry.RegisterCallback<ClickEvent>(answerBtn, ClickAnswerWrong); 
            }
        }
    }

    public void ClickAnswerRight(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.green;
        Debug.Log("right");
        progress.MoveCaterpillar();
        OnEnd();
        dataService.GoToNext();
    }
    public void ClickAnswerWrong(ClickEvent e) {
        Button btn = e.currentTarget as Button;
        btn.style.backgroundColor = Color.red;
        Debug.Log("wrong");
        progress.DeleteFruit();
    }

    private void OnEnd() {
        phaseThree.ResetPhase(false);
        answerBox.Clear();
        m_EventRegistry.Dispose();
    }

    private void RestartNewText() {
        OnStart();
        _currentPhase = Phase.None;
        animations.enabled = true;
    }

    private void RestartNewQuestion() { 
        InitDataQuestion();
        phaseThree = new PhaseThree(question, answerBox);
        phaseThree.StartPhase();
        phaseThree.PhaseReseted
            .Where(x => x == true)
            .Subscribe(x => {
                _currentPhase = Phase.Two;
                phaseTwo.StartPhase();
            });
    }
}
