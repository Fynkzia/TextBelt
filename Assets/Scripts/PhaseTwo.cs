using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class PhaseTwo {
    private VisualElement _textBelt;
    private VisualElement _overlay;
    private Label _actualText;
    private int _duration;
    private EventRegistry m_EventRegistry = new EventRegistry();
    public ReactiveProperty<bool> PhaseFinished = new ReactiveProperty<bool>(false);
    public ReactiveProperty<bool> PhaseReseted = new ReactiveProperty<bool>(false);
    public PhaseTwo(VisualElement textBelt, Label actualText, VisualElement overlay,int duration) { 
        _textBelt = textBelt;
        _duration = duration;
        _actualText = actualText;
        _overlay = overlay;
    }
    public void StartPhase() {
        m_EventRegistry.Dispose();
        PhaseFinished.Value = false;
        PhaseReseted.Value = false;
        _overlay.style.overflow = Overflow.Visible;
        _textBelt.AddToClassList("textbelt__open");
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_textBelt, MoveText);
    }

    private void MoveText(TransitionEndEvent evt) {
        m_EventRegistry.Dispose();
        _actualText.style.transitionDuration = new List<TimeValue> { new(_duration, TimeUnit.Second) };
        _actualText.AddToClassList("actualText_endPos");
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_actualText, SetToNextPhase);
    }
    public void SetToNextPhase(TransitionEndEvent evt) {
        Close();
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_textBelt,
            (
                evt => PhaseFinished.Value = true
            ));
    }

    private void Close() {
        m_EventRegistry.Dispose();
        _actualText.style.transitionDuration = new List<TimeValue>() { new TimeValue(1, TimeUnit.Second) };
        _textBelt.RemoveFromClassList("textbelt__open");
        _actualText.RemoveFromClassList("actualText_endPos");
        _overlay.style.overflow = Overflow.Hidden;
    }

    public void ResetPhase() {
        Close();
        m_EventRegistry.RegisterCallback<TransitionEndEvent>(_textBelt,
            (
                evt => PhaseReseted.Value = true
            ));
    }
}
