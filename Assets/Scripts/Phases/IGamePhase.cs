using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IGamePhase
{   
    public void Enter();
    public void Exit();
    public void MainButtonClick();
    IGamePhase GetNextPhase();
}
