using GoogleImporter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class ConfigsProvider : MonoBehaviour
{
    [SerializeField] private GameObject fruitPf;
    [Inject] private UIController uiController;
    private MainSettings mainSettings;

    private string fileDataPath = "MainData";

    void Start()
    {
        mainSettings = LoadSettings().Main[0];
        SetConfigs();
    }

    public GameSettings LoadSettings() {
        var jsonLoaded = Resources.Load<TextAsset>(fileDataPath);
        var gameSettings = !string.IsNullOrEmpty(jsonLoaded.text)
           ? JsonUtility.FromJson<GameSettings>(jsonLoaded.text)
           : new GameSettings();
        return gameSettings;
    }
    private void SetConfigs() {
        SetFruits(mainSettings.NumberOfFruits);
        uiController.speedOfStateChange = mainSettings.SpeedOfStateChange;
        uiController.textSpeed = mainSettings.TextSpeed;
        uiController.actualText.fontSize = mainSettings.TextSize;
        //root.Q<Label>("ActualText").style.unityFont = ???;
        uiController.actualText.wordSpacing = mainSettings.WordSpacing;
        //uiController.question.rectTransform.sizeDelta = new Vector2(250,mainSettings.QuestionBlockHeight);
        uiController.questionLabel.fontSize = mainSettings.QuestionBlockTextSize;
        uiController.questionLabel.alignment = TMPro.TextAlignmentOptions.Midline;
        uiController.answerBlockWidth = mainSettings.AnswerBlockWidth;
    }

    private void SetFruits(int numberOfFruits) { 
        for (int i = 0; i < numberOfFruits; i++) {
            Instantiate(fruitPf, uiController.fruitsBox.transform);
        }
    }
}
