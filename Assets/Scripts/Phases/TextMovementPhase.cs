using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Cysharp.Threading.Tasks;

public class TextMovementPhase : IGamePhase
{
    //[Inject] 
    private UIController _uiController;
    private EventRegistry m_EventRegistry = new EventRegistry();

    public bool phaseFinished;
    public TextMovementPhase(UIController uiController) {
        _uiController = uiController;
    }
    public void Enter() {
        phaseFinished = false;
        m_EventRegistry.Dispose();
        _uiController.ShowTextBelt();
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_uiController.textBelt, MoveText);

    }

    private void MoveText(TransitionEndEvent evt) {
        m_EventRegistry.Dispose();
        _uiController.MoveText();
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_uiController.actualText, 
            evt => Exit()
        );
    }

     private void CloseTransition() {
        m_EventRegistry.Dispose();
        _uiController.CloseTextBelt();

        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_uiController.textBelt,
            evt => phaseFinished = true
        );
    }

    public void Exit() {
        m_EventRegistry.Dispose();
        CloseTransition();
        EndExitTransition();
    }

    public IGamePhase GetNextPhase() {
        return new QuestionPhase(_uiController);
    }
    private async UniTask EndExitTransition() {
        await UniTask.WaitUntil(() => phaseFinished == true);
        Debug.Log("Task finished");
    }
}
