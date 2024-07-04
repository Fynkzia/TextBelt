using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Progress : MonoBehaviour
{
    [Inject] private DataService dataService;
    [Inject] private UIController uiController;
    [SerializeField]private Image progressBarValue;

    private float percentPerQuestion;
    private float _currentProgressBarValue = 0;
    private void OnEnable() {
        percentPerQuestion = 100 / dataService.GetAllQuestionsCount();
    }

    public void MoveCaterpillar() {
        _currentProgressBarValue += percentPerQuestion;
        progressBarValue.fillAmount = _currentProgressBarValue/100;
    }

    public void DeleteFruit() {
        if (uiController.fruitsBox.transform.childCount > 0) {
            Destroy(uiController.fruitsBox.transform.GetChild(0).gameObject);
        }
        else {
            Debug.Log("Try to restart");
        }
    }
}
