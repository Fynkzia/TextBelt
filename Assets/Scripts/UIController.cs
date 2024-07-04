using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button answerButtonPf;

    public GameObject fruitsBox;
    public Button playButton;
    public Image textBelt;
    public TextMeshProUGUI actualText;
    public GameObject question;
    public TextMeshProUGUI questionLabel;
    public GameObject answerBox;

    public GameObject speedUp;

    [HideInInspector] public int textSpeed;
    [HideInInspector] public int speedOfStateChange;
    [HideInInspector] public int answerBlockWidth;

    [HideInInspector] public Progress progress;
    [HideInInspector] public Animations animations;

    [Inject] private GameManager gameManager;
    [Inject] private DataService dataService;

    [HideInInspector]public int _duration;
    private float textBeltMixSize = 30;
    private float textBeltMaxSize = 100;
    private EventRegistry m_EventRegistry = new EventRegistry();
    public Action SubscribeOnFinished;
    public Action OnTextMoveFinished;
    public Action OnTextBeltAnimFinished;
    public Action OnAnswerButtonClick;

    private Dictionary<Button, bool> _answerButtons = new Dictionary<Button, bool>();
    private void OnEnable() {
        animations = playButton.GetComponent<Animations>();
        progress = GetComponent<Progress>();
       
    }
    private void Start() {
        playButton.onClick.AddListener(gameManager.MainButtonClick);
    }

    public void InitCurrentStepText() {
        int currentStepIndex = gameManager._currentStepIndex;
        _duration = dataService.GetCurrentStepText(currentStepIndex).Length / textSpeed;
        actualText.text = dataService.GetCurrentStepText(currentStepIndex);
    }

    public void InitAnswerButtons() {
        List<TextBlockData.Answer> answers = dataService.GetAnswers(gameManager._currentStepIndex, gameManager._currentQuestionIndex);
        for (int i = 0; i < answers.Count; i++) {
            var answerBtn = Instantiate(answerButtonPf,answerBox.transform);
            //answerBtn.style.width = answerBlockWidth; ???
            answerBtn.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].text;
            //answerBox.Add(answerBtn);
            _answerButtons.Add(answerBtn, answers[i].isRight);

            m_EventRegistry.RegisterClick(answerBtn, () => ClickAnswer(answerBtn));
        }
    }
    public void ClickAnswerRight(Button btn) {
        btn.GetComponent<Image>().color = Color.green;
        Debug.Log("right");
        progress.MoveCaterpillar();
        OnAnswerButtonClick?.Invoke();
    }
    public void ClickAnswerWrong(Button btn) {
        btn.GetComponent<Image>().color = Color.red;
        Debug.Log("wrong");
        progress.DeleteFruit();
    }

    public void ClickAnswer(Button btn) {
        if (_answerButtons[btn]) {
            ClickAnswerRight(btn);
        } else {
            ClickAnswerWrong(btn);
        }
    }
    public void ShowTextBelt() {
        StartCoroutine(TextBeltAnim(true));
        speedUp.SetActive(true);
    }

    public IEnumerator MoveText() {
        float elapsedTime = 0;

        while (elapsedTime < 1f) {
            actualText.rectTransform.localPosition = new Vector3(Mathf.Lerp(Screen.width/2, -Screen.width/2, elapsedTime), 0,0);
            elapsedTime += (1f / _duration) * Time.deltaTime;
            yield return null;
        }
        OnTextMoveFinished?.Invoke();
    }

    public void CloseTextBelt() {
        StartCoroutine(TextBeltAnim(false));
        actualText.rectTransform.localPosition = new Vector3(Screen.width/2, 0,0);
        speedUp.SetActive(false);
    }
    public IEnumerator TextBeltAnim(bool isExpand) {
        float elapsedTime = 0;
        float currentSizeY = isExpand ? textBeltMaxSize : textBeltMixSize;

        while (elapsedTime < 1f) {
            textBelt.rectTransform.sizeDelta = new Vector2(0,Mathf.Lerp(textBelt.rectTransform.sizeDelta.y, currentSizeY, elapsedTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnTextBeltAnimFinished?.Invoke();
    }

    public void InitQuestionText() {
        questionLabel.text = dataService.GetCurrentQuestionText(gameManager._currentStepIndex,gameManager._currentQuestionIndex);
    }

    public void InitAnswerBox() {
        foreach (Transform child in answerBox.transform) {
            Destroy(child.gameObject);
        }
        m_EventRegistry.Dispose();
        InitAnswerButtons();
    }
    public void ShowQuestionBox() {
        question.SetActive(true);
        answerBox.SetActive(true);
        //question.RemoveFromClassList("question_small");
        //answerBox.RemoveFromClassList("question_small");
    }

    public void CloseQuestionBox() {
        //question.AddToClassList("question_small");
        //answerBox.AddToClassList("question_small");
        //question.RegisterCallback<TransitionEndEvent>(HideQuestionBox);
        HideQuestionBox();
    }

    private void HideQuestionBox() {
        question.SetActive(false);
        answerBox.SetActive(false);
        //question.UnregisterCallback<TransitionEndEvent>(HideQuestionBox);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(gameManager.MainButtonClick);
    }
}
