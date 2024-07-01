using System;

public class ShowTextAction {
    private UIController _uiController;
    public Action SubscribeOnFinished;
    public ShowTextAction(UIController uiController) {
        _uiController = uiController;
    }

    public void Show() { 
        _uiController.ShowTextBelt();
        _uiController.OnTextBeltAnimFinished = () => MoveText();
    }

    private void MoveText() {
        _uiController.StartCoroutine(_uiController.MoveText());
        _uiController.OnTextMoveFinished = () => CloseTransition();
    }

    private void CloseTransition() {
        _uiController.OnTextBeltAnimFinished = () => EventFinished();
        _uiController.CloseTextBelt();
    }

    public void UnsubscribeAll() {
        SubscribeOnFinished = null;
        _uiController.OnTextMoveFinished = null;
        _uiController.OnTextBeltAnimFinished = null;
    }

    private void EventFinished() {
        SubscribeOnFinished?.Invoke();
    }
}
