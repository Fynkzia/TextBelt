using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowTextAction {
    private UIController _uiController;
    public Action SubscribeOnFinished;
    public void Show(UIController uiController) { 
        _uiController = uiController;
        _uiController.ShowTextBelt();
        _uiController.textBelt.RegisterCallback<TransitionEndEvent>(MoveText);
    }

    private void MoveText(TransitionEndEvent evt) {
        _uiController.MoveText();
        _uiController.textBelt.UnregisterCallback<TransitionEndEvent>(MoveText);
        _uiController.actualText.RegisterCallback<TransitionEndEvent>(CloseTransition);
    }

    private void CloseTransition(TransitionEndEvent evt) {
        _uiController.actualText.UnregisterCallback<TransitionEndEvent>(CloseTransition);
        _uiController.CloseTextBelt();

        _uiController.textBelt.RegisterCallback<TransitionEndEvent>(EventFinished);
    }

    public void UnsubscribeAll() {
        SubscribeOnFinished = null;
        _uiController.textBelt.UnregisterCallback<TransitionEndEvent>(EventFinished);
    }

    private void EventFinished(TransitionEndEvent evt) {
        SubscribeOnFinished?.Invoke();
    }
}
