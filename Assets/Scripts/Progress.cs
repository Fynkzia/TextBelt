using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class Progress : MonoBehaviour
{
    [Inject] private DataService dataService;
    private GroupBox fruitsBox;
    private VisualElement progressBarValue;

    private float percentPerQuestion;
    private float _currentProgressBarValue = 0;
    private void OnEnable() {
        var root = GetComponent<UIDocument>().rootVisualElement;
        fruitsBox = root.Q<GroupBox>("FruitsBox");

        progressBarValue = root.Q<VisualElement>("ProgressBarValue");
        percentPerQuestion = 100 / dataService.GetAllQuestionsCount();
    }

    public void MoveCaterpillar() {
        _currentProgressBarValue += percentPerQuestion;
        progressBarValue.style.width = new Length(_currentProgressBarValue, LengthUnit.Percent);
    }

    public void DeleteFruit() {
        if (fruitsBox.childCount > 0) {
            fruitsBox.RemoveAt(fruitsBox.childCount - 1);
        }
        else {
            Debug.Log("Try to restart");
        }
    }
}
