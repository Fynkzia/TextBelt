using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowTextAction {
    private UIController _uiController;
    public Action SubscribeOnFinished;
    public ShowTextAction(UIController uiController) {
        _uiController = uiController;
    }

    public void Show() { 
        _uiController.ShowTextBelt();
        _uiController.textBelt.RegisterCallback<TransitionEndEvent>(MoveText);
    }

    private void MoveText(TransitionEndEvent evt) {
        _uiController.textBelt.UnregisterCallback<TransitionEndEvent>(MoveText);
        _uiController.StartCoroutine(_uiController.MoveText());
        _uiController.OnTextMoveFinished = () => CloseTransition();
    }

    private void CloseTransition() {
        _uiController.CloseTextBelt();

        _uiController.textBelt.RegisterCallback<TransitionEndEvent>(EventFinished);
    }

    public void UnsubscribeAll() {
        SubscribeOnFinished = null;
        _uiController.OnTextMoveFinished = null;
        _uiController.textBelt.UnregisterCallback<TransitionEndEvent>(EventFinished);
    }

    private void EventFinished(TransitionEndEvent evt) {
        SubscribeOnFinished?.Invoke();
    }
}
