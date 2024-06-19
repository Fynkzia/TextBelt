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
    [Inject] private UIController uiController;
    private VisualElement root;
    private MainSettings mainSettings;

    private string fileDataPath = "MainData";

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
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
        root.Q<Label>("ActualText").style.fontSize = mainSettings.TextSize;
        //root.Q<Label>("ActualText").style.unityFont = ???;
        root.Q<Label>("ActualText").style.wordSpacing = mainSettings.WordSpacing;
        root.Q<VisualElement>("Question").style.height = mainSettings.QuestionBlockHeight;
        root.Q<Label>("QuestionLabel").style.fontSize = mainSettings.QuestionBlockTextSize;
        //root.Q<Label>("QuestionLabel").style.unityTextAlign = TextAnchor.MiddleCenter;
        uiController.answerBlockWidth = mainSettings.AnswerBlockWidth;
    }

    private void SetFruits(int numberOfFruits) { 
        var fruitsBox = root.Q<VisualElement>("FruitsBox");
        for (int i = 0; i < numberOfFruits; i++) {
            var fruit = new VisualElement();
            fruit.AddToClassList("fruit");
            fruitsBox.Add(fruit);
        }
    }
}
