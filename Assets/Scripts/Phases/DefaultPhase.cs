using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class DefaultPhase : IGamePhase
{
    //[Inject] 
    private UIController _uiController;
    public DefaultPhase(UIController uiController) { 
        _uiController = uiController;
    }
    public void Enter() {
        Debug.Log("call");
        _uiController.animations.enabled = true;
    }
    public void Exit() {
        _uiController.animations.enabled = false;
    }

    public IGamePhase GetNextPhase() {
        return new TextMovementPhase(_uiController);
    }
}
