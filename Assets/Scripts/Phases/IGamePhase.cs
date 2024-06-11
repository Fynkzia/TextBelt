using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IGamePhase
{   Action OnFinished { get; set; }
    public void Enter();
    public void Exit();
    IGamePhase GetNextPhase();
}
