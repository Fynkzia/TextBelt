using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class DataService : MonoBehaviour { 

    [SerializeField] private List<TextBlockData> data = new List<TextBlockData>();
    [SerializeField] private int textSpeed;
    public ReactiveProperty<bool> currentTextChanged = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> currentQuestionChanged = new ReactiveProperty<bool>(false);

    private int _currentText;
    private int _currentQuestion;

    private void Start() {
        _currentText = 0;
        _currentQuestion = 0;
}
    public int GetDuration() { 
        return data[_currentText].text.Length / textSpeed;
    }

    public string GetCurrentText() {
        return data[_currentText].text;
    }

    public string GetCurrentQuestionText() {
        return data[_currentText].questions[_currentQuestion].text;
    }

    public List<TextBlockData.Answer> GetAnswers() {
        return data[_currentText].questions[_currentQuestion].answers;
    }

    public int GetAllQuestionsCount() {
        int count = 0;
        for (int i = 0; i < data.Count; i++) { 
            for (int j = 0;j < data[i].questions.Count; j++) {
                count++;
            }
        }
        return count;
    }

    public void GoToNext() {
        if (_currentQuestion < data[_currentText].questions.Count - 1) {
            _currentQuestion++;
            currentQuestionChanged.Value = true;
        }
        else if (_currentText < data.Count - 1) {
            _currentText++;
            _currentQuestion = 0;
            currentTextChanged.Value = true;
        }
        else {
            Debug.Log("End Of Data");
        }

        currentQuestionChanged.Value = false;
        currentTextChanged.Value = false;
    }

}
