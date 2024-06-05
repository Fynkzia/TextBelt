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

    //[Inject]
    //private GameManager gameManager;
    private void OnEnable() {
        GetAllComponents();
        animations = GetComponent<Animations>();
        progress = GetComponent<Progress>();
       
    }

    private void Start() {
        playButton.RegisterCallback<ClickEvent>(e => GameManager.instance.MainButtonClick());
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

    public void ShowTextBelt() {
        overlay.style.overflow = Overflow.Visible;
        textBelt.AddToClassList("textbelt__open");
    }

    public void MoveText() {
        actualText.style.transitionDuration = new List<TimeValue> { new(GameManager.instance.duration, TimeUnit.Second) };
        actualText.AddToClassList("actualText_endPos");
    }

    public void CloseTextBelt() {
        actualText.style.transitionDuration = new List<TimeValue>() { new TimeValue(1, TimeUnit.Second) };
        textBelt.RemoveFromClassList("textbelt__open");
        actualText.RemoveFromClassList("actualText_endPos");
        overlay.style.overflow = Overflow.Hidden;
    }
    public void InitQuestionText() { 
    
    }

    public void InitAnswerBox() {

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
        /*m_EventRegistry.RegisterCallback<TransitionEndEvent>
        (question, evt => {
            question.style.display = DisplayStyle.None;
            answerBox.style.display = DisplayStyle.None;
        });*/
    }

    private void OnDestroy() {
        playButton.UnregisterCallback<ClickEvent>(e => GameManager.instance.MainButtonClick());
    }
}
