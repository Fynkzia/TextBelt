using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataService : MonoBehaviour { 

    [SerializeField] private List<TextBlockData> data = new List<TextBlockData>();

    public string GetCurrentStepText(int currentStepIndex) {
        return data[currentStepIndex].text;
    }

    public string GetCurrentQuestionText(int currentStepIndex, int currentStepQuestion) {
        return data[currentStepIndex].questions[currentStepQuestion].text;
    }

    public List<TextBlockData.Answer> GetAnswers(int currentStepIndex, int currentStepQuestion) {
        return data[currentStepIndex].questions[currentStepQuestion].answers;
    }

    public bool HasStepText(int currentStepIndex) {
        return currentStepIndex <= data.Count - 1;
    }

    public bool HasQuestionText(int currentStepIndex, int currentStepQuestion) {
        return currentStepQuestion <= data[currentStepIndex].questions.Count - 1;
    }

    public int GetAllQuestionsCount() {
        return data.Sum(x => x.questions.Count);
    }

}
